using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    BurnieHandler burnieHandler;

    public int dialogueIndex = 0;
    public bool isHoveringButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        burnieHandler = FindAnyObjectByType<BurnieHandler>();

        if (burnieHandler != null) {
            burnieHandler.Talk(0);
        }
        else {
            Debug.LogError("[GameManager] BurnieHandler returned null.");
        }

        Button[] buttonsInScene = FindObjectsByType<Button>();
        foreach (Button button in buttonsInScene) {
            if (button.gameObject.GetComponent<ButtonHoverHandler>() == null) {
                // Doesn't have it, add it
                button.gameObject.AddComponent<ButtonHoverHandler>();
            }
        }
    }

    // Update is called once per frame
    void Update() {

    }
}