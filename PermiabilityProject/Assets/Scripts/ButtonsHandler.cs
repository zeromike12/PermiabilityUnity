using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsHandler : MonoBehaviour
{
    public Button outsideButton;
    public Button insideButton;
    public Button runSimButton;

    public TextMeshProUGUI choiceLabel;

    [HideInInspector] public string choice;

    private liquidInside liquidInside;
    private LiquidOutside liquidOutside;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        liquidOutside = FindAnyObjectByType<LiquidOutside>();
        liquidInside = FindAnyObjectByType<liquidInside>();

        runSimButton.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OutsideButtonClicked() {
        choice = "OUTSIDE";
        outsideButton.interactable = false;
        insideButton.interactable = true;

        runSimButton.interactable = true;

        choiceLabel.text = "Chosen: Outside";
    }

    public void InsideButtonClicked() {
        choice = "INSIDE";
        insideButton.interactable = false;
        outsideButton.interactable = true;

        runSimButton.interactable = true;

        choiceLabel.text = "Chosen: Inside";
    }

    public void RunSimButtonClicked() {
        insideButton.transform.parent.gameObject.SetActive(false);
        runSimButton.gameObject.SetActive(false);
        choiceLabel.gameObject.SetActive(false);
        choiceLabel.transform.parent.transform.Find("TopLabel").gameObject.SetActive(false);

        if (choice == "INSIDE") {
            liquidInside.runSim = true;
        }
        else if (choice == "OUTSIDE") {
            liquidOutside.runSim = true;
        }
    }
}
