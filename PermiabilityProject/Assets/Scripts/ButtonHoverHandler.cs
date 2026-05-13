using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    GameManager gameManager;

    private void Start() {
        gameManager = FindAnyObjectByType<GameManager>();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        gameManager.isHoveringButton = true;
    }

    public void OnPointerExit(PointerEventData eventData) {
        gameManager.isHoveringButton = false;
    }
}