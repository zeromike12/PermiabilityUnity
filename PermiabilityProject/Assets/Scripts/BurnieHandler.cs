using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BurnieHandler : MonoBehaviour {
    public Image burnie;
    public Image speechBubble;
    public TextMeshProUGUI dialogueText;

    public bool isTalking;
    public bool canContinueOnClick = true;
    public int index = 0;
    BurnieDialogue burnieDialogue;

    private Coroutine typingCoroutine;
    private string currentFullText;
    private float currentCharsPerSecond;

    private void Awake() {
        burnieDialogue = GetComponent<BurnieDialogue>();
    }

    private void Update() {
        if (isTalking) {
            burnie.gameObject.SetActive(true);
            speechBubble.gameObject.SetActive(true);

            if (Keyboard.current.spaceKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame) {
                // Skip is spacebar is pressed
                SkipDialogue(burnieDialogue.CanContinue(index));
            }
        }
        else {
            if (dialogueText.text == "") {
                burnie.gameObject.SetActive(false);
                speechBubble.gameObject.SetActive(false);
            }
            else {
                if (Keyboard.current.spaceKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame) {
                    // Proceed to next line when user clicks or presses space
                    if (canContinueOnClick && index < burnieDialogue.Lines.Length) {
                        index++; // Move to the next line of dialogue
                        Talk(index);
                    }
                }

            }
        }
    }

    public void Talk(int indexNumber) {
        if (indexNumber >= burnieDialogue.Lines.Length) return;

        if (dialogueText == null) return;
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);

        float charsPerSecond = 30f;

        currentFullText = burnieDialogue.GetLine(indexNumber) ?? string.Empty;
        currentCharsPerSecond = Mathf.Max(0.1f, charsPerSecond);
        typingCoroutine = StartCoroutine(TypewriterCoroutine(burnieDialogue.CanContinue(indexNumber)));

        isTalking = true;
        canContinueOnClick = false;
    }

    public void StopTalking() {
        if (typingCoroutine != null) {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        isTalking = false;
    }

    private void SkipDialogue(bool doContinue) {
        // If not talking, do nothing
        if (!isTalking) return;

        if (typingCoroutine != null) {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        dialogueText.text = currentFullText ?? string.Empty;
        isTalking = false;

        canContinueOnClick = doContinue;
    }

    private IEnumerator TypewriterCoroutine(bool doContinue) {
        dialogueText.text = string.Empty;
        float delay = 1f / currentCharsPerSecond;

        // Type-write each character in the current line
        for (int i = 0; i < currentFullText.Length; i++) {
            dialogueText.text += currentFullText[i];
            yield return new WaitForSeconds(delay);
        }

        // Current line of dialogue is finished
        typingCoroutine = null;
        isTalking = false;

        canContinueOnClick = doContinue;
    }
}
