using UnityEngine;

public class GameManager : MonoBehaviour
{
    BurnieHandler burnieHandler;
    public BurnieDialogue burnieDialogue;

    public int dialogueIndex = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        burnieHandler = FindAnyObjectByType<BurnieHandler>();

        burnieHandler.Talk(burnieDialogue.Lines[0]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}