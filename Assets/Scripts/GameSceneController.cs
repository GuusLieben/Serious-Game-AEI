using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GameSceneController : MonoBehaviour
{
    // Positions of chairs on the Y and Z axis
    private readonly Dictionary<int, Vector2> _rowPositions = new Dictionary<int, Vector2>
    {
        {4, new Vector2(5f, -5f)},
        {3, new Vector2(4.25f, -2.75f)},
        {2, new Vector2(3.5f, -0.5f)},
        {1, new Vector2(3f, 1.75f)},
        {0, new Vector2(2.25f, 4f)},
    };

    // Positions of chairs on the X axis
    private readonly Dictionary<int, float> _chairPositions = new Dictionary<int, float>
    {
        {4, -5.25f},
        {3, -2.5f},
        {2, 0},
        {1, 2.5f},
        {0, 5.25f}
    };

    // Touch control settings
    private Vector3 _firstPoint;
    private Vector3 _secondPoint;
    private float _xAngle;
    private float _yAngle;
    private float _xAngleTemp;
    private float _yAngleTemp;

    // Primary text display
    private TMP_Text _gameText;
    private IEnumerable<TMP_Text> _timers;
    private int _remainingGameTime = -1;

    private float _elapsed;
    private bool _gameActive = true;
    private string _gameCode;
    private GameStatus _gameStatus;
    private bool _indicatingPlayer;

    // Reference to last seat in 2D space (0,0 .. 4,4)
    private Vector2 _lastSeat;

    private const string PortrayText = "Beeld uit:\n{0},\n{1},\n{2}";
    private const string NextPerson = "Geef de telefoon aan:\n{0}";
    private const string TeamWon = "{0} heeft gewonnen!";
    private const string WaitingFor = "{0} is aan zet!";

    [SerializeField] private int chairShiftSpeed = 5;
    [SerializeField] private int secondsPerRound = 30;

    [SerializeField]
    private string url = "https://avans-schalm-appserver.azurewebsites.net/api/game/status?gameCode={0}";

    private void Start()
    {
        _lastSeat = new Vector2(2, 4);

        _timers = GameObject.FindGameObjectsWithTag("TimerText").Select(t => t.GetComponent<TMP_Text>());
        _gameText = GameObject.FindGameObjectWithTag("PrimarySceneText").GetComponent<TMP_Text>();
        _gameCode = PlayerPrefs.GetString("GAME_CODE");
        _xAngle = 0;
        _yAngle = 0;
        transform.rotation = Quaternion.Euler(_yAngle, _xAngle, 0);

        SetText("Waiting for server");
        SetTimers("");

        StartPolling();
    }

    private void Update()
    {
        SetChair(_lastSeat);
        UpdateTimer();

        if (Input.touchCount <= 0) return;

        if (Input.GetTouch(0).phase == TouchPhase.Began)
        {
            _firstPoint = Input.GetTouch(0).position;
            _xAngleTemp = _xAngle;
            _yAngleTemp = _yAngle;
            return;
        }

        if (Input.GetTouch(0).phase != TouchPhase.Moved) return;

        _secondPoint = Input.GetTouch(0).position;
        _xAngle = _xAngleTemp + (_secondPoint.x - _firstPoint.x) * 180 / Screen.width;
        _yAngle = _yAngleTemp + (_secondPoint.y - _firstPoint.y) * 90 / Screen.height;
        transform.rotation = Quaternion.Euler(_yAngle, _xAngle, 0.0f);
    }

    private void UpdateTimer()
    {
        _elapsed += Time.deltaTime;
        if (_elapsed < 1) return;

        _remainingGameTime -= (int) _elapsed;
        _elapsed = 0;

        if (_remainingGameTime < 0)
        {
            SetTimers("");
            return;
        }

        if (_remainingGameTime == 0)
        {
            SetText("Round ended");
            if (!_gameStatus.currentTeamId.ToString().Equals(PlayerPrefs.GetString("TEAM_ID")))
            {
                SceneManager.LoadScene("JudgingScene");
            }
        }

        SetTimers(_remainingGameTime + "");
    }

    private void SetTimers(string text)
    {
        foreach (var timer in _timers) timer.text = text;
    }

    private void SetText(string text)
    {
        _gameText.text = text;
    }

    private void SetChair(Vector2 position)
    {
        if (transform.position.Equals(position)) return;

        var chair = _chairPositions[(int) position.x];
        var row = _rowPositions[(int) position.y];
        var seat = new Vector3(chair, row.x, row.y);

        var delta = _lastSeat.y - position.y;
        if (delta < 0) delta = -delta;

        var speed = (chairShiftSpeed - delta) * Time.deltaTime;
        _lastSeat = position;
        transform.position = Vector3.Lerp(transform.position, seat, speed);
    }

    private void Portray(string piece, string relation, string emotion)
    {
        SetText(string.Format(PortrayText, piece, relation, emotion));
        PlayerPrefs.SetString("PieceWord", piece);
        PlayerPrefs.SetString("RelationWord", relation);
        PlayerPrefs.SetString("EmotionWord", emotion);
        StartTimer();
    }

    private void SetNextPerson(string person)
    {
        _indicatingPlayer = true;
        SetText(string.Format(NextPerson, person));
    }

    private void SetTeamWon(string team)
    {
        SetText(string.Format(TeamWon, team));
        _gameActive = false;
    }

    private void WaitForTeam(string team)
    {
        SetText(string.Format(WaitingFor, team));
        StartTimer();
    }

    private void StartTimer()
    {
        _remainingGameTime = secondsPerRound;
        SetTimers(secondsPerRound + "");
    }

    private async void StartPolling()
    {
        while (_gameActive)
        {
            await Task.Delay(2000);
            StartCoroutine(MakeRequest());
        }
    }

    private IEnumerator MakeRequest()
    {
        using var getTeamsRequest = UnityWebRequest.Get(string.Format(url, _gameCode));
        yield return getTeamsRequest.SendWebRequest();
        if (getTeamsRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Couldn't fetch game status: " + getTeamsRequest.error);
        }
        else
        {
            var text = getTeamsRequest.downloadHandler.text;
            var gameStatus = JsonConvert.DeserializeObject<GameStatus>(text);
            if (gameStatus != null)
            {
                if (TeamWinnerAnnounced(gameStatus))
                {
                    SetTeamWon(gameStatus.winningTeam.TeamName);
                    yield break;
                }
                
                if (_gameStatus == null || _indicatingPlayer || !gameStatus.statusId.Equals(_gameStatus.statusId))
                {
                    UpdateState(gameStatus);
                }

                _gameStatus = gameStatus;
            }
            else Debug.LogError("GameStatus was null");
        }
    }

    private void UpdateState(GameStatus status)
    {
        _indicatingPlayer = false;

        if (MimePlayerChanged(status)) SetNextPerson(status.mimePlayer);
        else
        {
            var words = status.currentWords;
            if (!PlayerPrefs.GetString("PieceWord").Equals(words[0])) Portray(words[0], words[1], words[2]);
        }

        if (WaitingForTeam(status)) WaitForTeam(status.currentTeamName);

        CheckPositionChanged(status);
    }

    private void CheckPositionChanged(GameStatus status)
    {
        var teamId = PlayerPrefs.GetString("TEAM_ID");
        var guid = new Guid(teamId);
        var row = status.teamPositions[guid];
        if (Math.Abs(row - _lastSeat.y) < 0.01) return;
        // Developer note, 2 is currently hardcoded to be the center chair.
        // For future expansion this may become variable.
        _lastSeat = new Vector2(2, row);
    }

    private static bool TeamWinnerAnnounced(GameStatus status)
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
        return status.isFinished && status.winningTeam != null;
    }

    private bool MimePlayerChanged(GameStatus status)
    {
        return _gameStatus == null || !status.mimePlayer.Equals(_gameStatus.mimePlayer);
    }

    private static bool WaitingForTeam(GameStatus status)
    {
        var teamId = PlayerPrefs.GetString("TEAM_ID");
        var guid = new Guid(teamId);
        return !status.currentTeamId.Equals(guid);
    }
}