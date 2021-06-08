using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
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
    private TMP_Text _text;
    
    // Reference to last seat in 2D space (0,0 .. 4,4)
    private Vector2 _lastSeat;

    [SerializeField]
    private int chairShiftSpeed = 5;

    private void Start()
    {
        _text = FindObjectOfType<TMP_Text>();
        _xAngle = 0;
        _yAngle = 0;
        transform.rotation = Quaternion.Euler(_yAngle, _xAngle, 0);
        SetText("Beeld uit:\nTrompet speler");
        SetChair(new Vector2(3, 2));
    }

    private void Update()
    {
        // Development only, changes the active row
        if (Input.GetKey(KeyCode.Alpha1)) SetChair(new Vector2(_lastSeat.x, 0));
        if (Input.GetKey(KeyCode.Alpha2)) SetChair(new Vector2(_lastSeat.x, 1));
        if (Input.GetKey(KeyCode.Alpha3)) SetChair(new Vector2(_lastSeat.x, 2));
        if (Input.GetKey(KeyCode.Alpha4)) SetChair(new Vector2(_lastSeat.x, 3));
        if (Input.GetKey(KeyCode.Alpha5)) SetChair(new Vector2(_lastSeat.x, 4));
        
        // Development only, changes the active seat
        if (Input.GetKey(KeyCode.A)) SetChair(new Vector2(0, _lastSeat.y));
        if (Input.GetKey(KeyCode.B)) SetChair(new Vector2(1, _lastSeat.y));
        if (Input.GetKey(KeyCode.C)) SetChair(new Vector2(2, _lastSeat.y));
        if (Input.GetKey(KeyCode.D)) SetChair(new Vector2(3, _lastSeat.y));
        if (Input.GetKey(KeyCode.E)) SetChair(new Vector2(4, _lastSeat.y));
        
        if (Input.touchCount <= 0) return;

        if (Input.GetTouch(0).phase == TouchPhase.Began)
        {
            _firstPoint = Input.GetTouch(0).position;
            _xAngleTemp = _xAngle;
            _yAngleTemp = _yAngle;
            return;
        }

        if (Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            _secondPoint = Input.GetTouch(0).position;
            _xAngle = _xAngleTemp + (_secondPoint.x - _firstPoint.x) * 180 / Screen.width;
            _yAngle = _yAngleTemp + (_secondPoint.y - _firstPoint.y) * 90 / Screen.height;
            this.transform.rotation = Quaternion.Euler(_yAngle, _xAngle, 0.0f);
        }
    }

    public void SetChair(Vector2 position)
    {
        var chair = _chairPositions[(int) position.x];
        var row = _rowPositions[(int) position.y];
        var seat = new Vector3(chair, row.x, row.y);

        var delta = _lastSeat.y - position.y;
        if (delta < 0) delta = -delta;
        
        var speed = (chairShiftSpeed - delta) * Time.deltaTime;
        _lastSeat = position;
        transform.position = Vector3.Lerp(transform.position, seat, speed);
    }

    public void SetText(string text)
    {
        _text.text = text;
    }
    
}