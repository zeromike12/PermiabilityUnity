using UnityEngine;
using UnityEngine.UI;

public class CreditsHandler : MonoBehaviour {
    public GameObject overlay;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        overlay.SetActive(false);
    }

    public void InfoButtonClicked() {
        overlay.SetActive(!overlay.activeSelf);
    }
}
