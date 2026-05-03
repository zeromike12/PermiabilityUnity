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

    private Coroutine typingCoroutine;
    private string currentFullText;
    private float currentCharsPerSecond;

    private void Update() {
        if (isTalking) {
            burnie.gameObject.SetActive(true);
            speechBubble.gameObject.SetActive(true);

            if (Keyboard.current.spaceKey.wasPressedThisFrame) {
                // Skip is spacebar is pressed
                SkipDialogue();
            }
        }
        else {
            if (dialogueText.text == "") {
                burnie.gameObject.SetActive(false);
                speechBubble.gameObject.SetActive(false);
            }
        }
    }

    private void OnMouseDown() {
        if (isTalking) SkipDialogue();
    }

    public void Talk(string fullText) {
        if (dialogueText == null) return;
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);

        float charsPerSecond = 30f;

        currentFullText = fullText ?? string.Empty;
        currentCharsPerSecond = Mathf.Max(0.1f, charsPerSecond);
        typingCoroutine = StartCoroutine(TypewriterCoroutine());

        isTalking = true;
    }

    public void StopTalking() {
        if (typingCoroutine != null) {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        isTalking = false;
    }

    private void SkipDialogue() {
        // If not talking, do nothing
        if (!isTalking) return;

        if (typingCoroutine != null) {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        dialogueText.text = currentFullText ?? string.Empty;
        isTalking = false;
    }

    private IEnumerator TypewriterCoroutine() {
        dialogueText.text = string.Empty;
        float delay = 1f / currentCharsPerSecond;

        for (int i = 0; i < currentFullText.Length; i++) {
            dialogueText.text += currentFullText[i];
            yield return new WaitForSeconds(delay);
        }

        typingCoroutine = null;
        isTalking = false;
    }
}
