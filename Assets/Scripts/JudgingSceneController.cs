using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JudgingSceneController : MonoBehaviour
{

    private readonly List<string> _clicked = new List<string>();

    [SerializeField] private TMP_Text buttonA;
    [SerializeField] private TMP_Text buttonB;
    [SerializeField] private TMP_Text buttonC;

    private void Start()
    {
        SetValues(new []{
            "Test", "Kaas", "Kees"
        });
}

    public void OnClicked(Button button)
    {
        if (!_clicked.Contains(button.name))
        {
            button.image.color = new Color(0.9294118F, 0.145098F, 0.3254902F);
            _clicked.Add(button.name);
        }
        else
        {
            button.image.color = new Color(0.7058824F, 0.5960785F, 0.6235294F);
            _clicked.Remove(button.name);
        }

        print(button.name);
    }

    public void SetValues(string[] words)
    {
        buttonA.text = words[0];
        buttonB.text = words[1];
        buttonC.text = words[2];
    }

    public void OnSubmit()
    {
        var amount = _clicked.Count;
        // ...
    }
}
