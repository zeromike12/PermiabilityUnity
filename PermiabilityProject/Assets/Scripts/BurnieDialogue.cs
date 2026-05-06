using System;
using UnityEngine;

public class BurnieDialogue : MonoBehaviour {

    // Keep the DialogueLine nested under BurnieDialogue if only using it here
    // Move it above BurnieDialogue if needed in other script(s)
    [Serializable]
    public class DialogueLine {
        public string text; // The text displayed in the dialogue speech bubble
        public bool canContinue = true; // Whether or not the user can click to proceed to the next line of dialogue
    }

    public DialogueLine[] Lines = new DialogueLine[] {
        new DialogueLine { text = "Hey there, I'm Burnie the Bunsen Burner!\n\n(Click anywhere to continue)", canContinue = true },
        new DialogueLine { text = "This is the second line of dialogue, pretty cool right?\n\n(Click anywhere to continue)", canContinue = true },
        new DialogueLine { text = "Don't skip this", canContinue = false },
        new DialogueLine { text = "Last here we go weeeeeeee", canContinue = true },
    };

    public string GetLine(int index) {
        return Lines[index].text;
    }

    public bool CanContinue(int index) {
        //if (index > 0 || index >= Lines.Length) return false;
        return Lines[index].canContinue;
    }
}
