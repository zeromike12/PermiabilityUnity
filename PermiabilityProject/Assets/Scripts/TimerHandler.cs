using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TimerHandler : MonoBehaviour {
    private float seconds;
    public TextMeshProUGUI timerLabel;

    [HideInInspector] public float fastForward = 1f; // 1 by default

    public LiquidOutside liquidOutside;
    public liquidInside liquidInside;

    public Button runSimButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {

    }

    private string FormatTime() {
        TimeSpan time = TimeSpan.FromSeconds(seconds);
        string formattedTime = time.ToString(@"h\:mm\:ss\.ff");

        return formattedTime;
    }

    // Update is called once per frame
    void Update() {
        if (Keyboard.current.rightCtrlKey.isPressed) {
            fastForward = 10f;
        }
        else {
            fastForward = 1f;
        }

        if (liquidOutside.runSim) {
            timerLabel.gameObject.SetActive(true);
            seconds += (Time.deltaTime * 60) * fastForward; // 1 hour simulation time over 1 real-time minute

            timerLabel.text = FormatTime();

            if (seconds >= 3600f) {
                runSimButton.gameObject.SetActive(true);
            }
            else runSimButton.gameObject.SetActive(false);
        }
        else {
            timerLabel.gameObject.SetActive(false);
            seconds = 0f;
        }
    }
}
