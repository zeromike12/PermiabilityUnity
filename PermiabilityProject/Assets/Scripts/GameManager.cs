using UnityEngine;

public class GameManager : MonoBehaviour {
    BurnieHandler burnieHandler;

    public int dialogueIndex = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        burnieHandler = FindAnyObjectByType<BurnieHandler>();

        if (burnieHandler != null) {
            burnieHandler.Talk(0);
        }
        else {
            Debug.LogError("[GameManager] BurnieHandler returned null.");
        }

    }

    // Update is called once per frame
    void Update() {

    }
}