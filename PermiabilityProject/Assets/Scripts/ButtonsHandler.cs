using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsHandler : MonoBehaviour {
    BurnieHandler burnieHandler;
    BurnieDialogue burnieDialogue;

    public Button outsideButton;
    public Button insideButton;

    public GameObject liquidLabelsHolder;
    public TextMeshProUGUI outsideLiquidLabel;
    public TextMeshProUGUI insideLiquidLabel;

    [Header("Outside liquids")]
    public GameObject outsideHolder;
    public Button IodineButton;
    public Button SodBiButton;

    [Header("Inside liquids")]
    public GameObject insideHolder;
    public Button StarchButton;
    public Button MCresolButton;

    public Button runSimButton;
    public Button confirmPredictionButton;

    public TextMeshProUGUI liquidLabel;
    public GameObject creditScreen;

    [HideInInspector] public string insideLiquidChoice = "";
    [HideInInspector] public string outsideLiquidChoice = "";

    public LiquidOutside liquidOutside;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        liquidOutside = FindAnyObjectByType<LiquidOutside>();
        burnieHandler = FindAnyObjectByType<BurnieHandler>();
        burnieDialogue = FindAnyObjectByType<BurnieDialogue>();

        IodineButton.interactable = false;
        SodBiButton.interactable = false;

        outsideHolder.gameObject.SetActive(false);
        insideHolder.gameObject.SetActive(false);

        liquidLabel.gameObject.SetActive(false);

        runSimButton.interactable = false;
    }

    private void Update() {
        // If either button isn't filled with /something/, prevent the Run Sim button from being clicked at all.
        if (insideLiquidChoice == "" || outsideLiquidChoice == "") runSimButton.interactable = false;

        switch (burnieHandler.index) {
            case 5:
                // Making the first prediction
                confirmPredictionButton.gameObject.SetActive(true);
                break;
            case 6:
                // Both liquids should be selected, check if they're correct
                if (insideLiquidChoice == "Starch" && outsideLiquidChoice == "SodBi") {
                    runSimButton.interactable = true;
                }
                else {
                    runSimButton.interactable = false;
                }
                break;
            case 10:
                // Both liquids should be selected, check if they're correct
                if (insideLiquidChoice == "MCresol" && outsideLiquidChoice == "Iodine") {
                    runSimButton.interactable = true;
                }
                else {
                    runSimButton.interactable = false;
                }
                break;
            default:
                // Every other possibility
                break;
        }
    }

    public void OutsideButtonClicked() {
        outsideHolder.SetActive(true);
        outsideButton.interactable = false;

        insideHolder.SetActive(false);
        insideButton.interactable = true;

        liquidLabel.gameObject.SetActive(true);
        liquidLabel.text = "Outside liquid";
    }

    public void InsideButtonClicked() {
        insideHolder.SetActive(true);
        insideButton.interactable = false;

        outsideHolder.SetActive(false);
        outsideButton.interactable = true;

        liquidLabel.gameObject.SetActive(true);
        liquidLabel.text = "Inside liquid";
    }

    public void StarchButtonClicked() {
        insideLiquidChoice = "Starch";
        insideLiquidLabel.text = "Inside Liquid: Starch";
        liquidOutside.insideliquidSize = 1;
        StarchButton.interactable = false;
        MCresolButton.interactable = true;

        if (burnieHandler.index == 3) {
            // First, click the "Inside" button and choose "Starch".
            burnieHandler.Talk(4);
            burnieHandler.index = 4;

            IodineButton.interactable = true;
            SodBiButton.interactable = true;
        }
    }

    public void MCresolButtonClicked() {
        insideLiquidChoice = "MCresol";
        insideLiquidLabel.text = "Inside Liquid: M-Cresol";
        liquidOutside.insideliquidSize = 2;
        MCresolButton.interactable = false;
        StarchButton.interactable = true;

        if (burnieHandler.index == 8) {
            // Now, let's take a look at the other options. Go ahead and select "M-Cresol" from the "Inside" liquids screen.
            IodineButton.interactable = true;
            SodBiButton.interactable = true;

            burnieHandler.Talk(9);
            burnieHandler.index = 9;
        }
    }

    public void IodineButtonClicked() {
        outsideLiquidChoice = "Iodine";
        outsideLiquidLabel.text = "Outside Liquid: Iodine";
        liquidOutside.liquidSize = 3;
        IodineButton.interactable = false;
        SodBiButton.interactable = true;

        if (burnieHandler.index == 9) {
            // Next, select "Iodine" from the "Outside" liquids screen.

            burnieHandler.Talk(10);
            burnieHandler.index = 10;
        }
    }

    public void SodBiButtonClicked() {
        outsideLiquidChoice = "SodBi";
        outsideLiquidLabel.text = "Outside Liquid: Sodium Bicarbonate";
        liquidOutside.liquidSize = 4;
        SodBiButton.interactable = false;
        IodineButton.interactable = true;

        if (burnieHandler.index == 4) {
            // Next, click the "Outside" button and choose "Sodium Bicarbonate".
            burnieHandler.Talk(5);
            burnieHandler.index = 5;
        }
    }

    public void ConfirmPredictionButtonClicked() {
        if (burnieHandler.index == 5) {
            // Clicked on the first prediction
            burnieHandler.Talk(6);
            burnieHandler.index = 6;

            confirmPredictionButton.gameObject.SetActive(false);
        }
        else if (burnieHandler.index == 10) {
            // Clicked on the second prediction
            burnieHandler.Talk(11);
            burnieHandler.index = 11;

            confirmPredictionButton.gameObject.SetActive(false);
        }
    }

    public void RunSimButtonClicked() {
        if (!liquidOutside.runSim) {
            // Run sim

            // Deactivate all main UI elements
            transform.Find("Holder").gameObject.SetActive(false);

            outsideHolder.gameObject.SetActive(false);
            insideHolder.gameObject.SetActive(false);

            outsideLiquidLabel.gameObject.SetActive(false);
            insideLiquidLabel.gameObject.SetActive(false);

            liquidLabel.gameObject.SetActive(false);

            runSimButton.gameObject.SetActive(false);
            runSimButton.GetComponentInChildren<TextMeshProUGUI>().text = "Stop Sim";

            burnieHandler.burnie.gameObject.SetActive(false);
            burnieHandler.speechBubble.gameObject.SetActive(false);

            FindAnyObjectByType<CreditsHandler>().gameObject.SetActive(false);

            // Lastly, actually run the sim
            liquidOutside.runSim = true;
        }
        else {
            // Stop sim

            // Reactivate appropriate main UI elements
            transform.Find("Holder").gameObject.SetActive(true);

            outsideLiquidLabel.gameObject.SetActive(true);
            outsideLiquidLabel.text = "Outside liquid: (Not selected)";
            insideLiquidLabel.gameObject.SetActive(true);
            insideLiquidLabel.text = "Inside liquid: (Not selected)";

            IodineButton.interactable = false;
            SodBiButton.interactable = false;

            creditScreen.gameObject.SetActive(true);

            // Lastly, actually run the sim
            liquidOutside.runSim = false;
            runSimButton.GetComponentInChildren<TextMeshProUGUI>().text = "Run Sim";

            if (burnieHandler.index == 6) {
                // After first sim
                burnieHandler.Talk(7);
                burnieHandler.index = 7;
            }
        }
    }
}
