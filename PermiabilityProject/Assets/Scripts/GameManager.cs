using UnityEngine;

public class GameManager : MonoBehaviour
{
    BurnieHandler burnieHandler;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        burnieHandler = FindAnyObjectByType<BurnieHandler>();

        burnieHandler.Talk("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
