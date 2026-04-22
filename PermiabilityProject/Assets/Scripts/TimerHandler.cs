using System;
using TMPro;
using UnityEngine;

public class TimerHandler : MonoBehaviour
{
    private float seconds;
    public TextMeshProUGUI timerLabel;

    public LiquidOutside liquidOutside;
    public liquidInside liquidInside;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    private string FormatTime() {
        TimeSpan time = TimeSpan.FromSeconds(seconds);
        string formattedTime = time.ToString(@"h\:mm\:ss\.ff");

        return formattedTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (liquidOutside.runSim) {
            seconds += (Time.deltaTime * 60); // 1 hour simulation time over 1 real-time minute

            timerLabel.text = FormatTime();
        }
    }
}
