using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsHandler : MonoBehaviour
{
    public Button outsideButton;
    public Button insideButton;

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

    public TextMeshProUGUI liquidLabel;

    [HideInInspector] public string insideLiquidChoice;
    [HideInInspector] public string outsideLiquidChoice;

    //public liquidInside liquidInside;
    public LiquidOutside liquidOutside;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        liquidOutside = FindAnyObjectByType<LiquidOutside>();
        //liquidInside = FindAnyObjectByType<liquidInside>();

        outsideHolder.gameObject.SetActive(false);
        insideHolder.gameObject.SetActive(false);

        liquidLabel.gameObject.SetActive(false);

        runSimButton.interactable = false;
    }

    private void Update()
    {
        if (insideLiquidChoice != null && outsideLiquidChoice != null)
        {
            runSimButton.interactable = true;
        }
        else
        {
            runSimButton.interactable = false;
        }
    }

    public void OutsideButtonClicked()
    {
        outsideHolder.SetActive(true);
        outsideButton.interactable = false;

        insideHolder.SetActive(false);
        insideButton.interactable = true;

        liquidLabel.gameObject.SetActive(true);
        liquidLabel.text = "Outside liquid";
    }

    public void InsideButtonClicked()
    {
        insideHolder.SetActive(true);
        insideButton.interactable = false;

        outsideHolder.SetActive(false);
        outsideButton.interactable = true;

        liquidLabel.gameObject.SetActive(true);
        liquidLabel.text = "Inside liquid";
    }

    public void StarchButtonClicked()
    {
        insideLiquidChoice = "Starch";
        insideLiquidLabel.text = "Inside Liquid: Starch";

        StarchButton.interactable = false;
        MCresolButton.interactable = true;
    }

    public void MCresolButtonClicked()
    {
        insideLiquidChoice = "MCresol";
        insideLiquidLabel.text = "Inside Liquid: M-Cresol";

        MCresolButton.interactable = false;
        StarchButton.interactable = true;
    }

    public void IodineButtonClicked()
    {
        outsideLiquidChoice = "Iodine";
        outsideLiquidLabel.text = "Outside Liquid: Iodine";

        IodineButton.interactable = false;
        SodBiButton.interactable = true;
    }

    public void SodBiButtonClicked()
    {
        outsideLiquidChoice = "SodBi";
        outsideLiquidLabel.text = "Outside Liquid: Sodium Bicarbonate";

        SodBiButton.interactable = false;
        IodineButton.interactable = true;
    }

    public void RunSimButtonClicked()
    {
        transform.Find("Holder").gameObject.SetActive(false);

        //insideButton.transform.parent.gameObject.SetActive(false);
        runSimButton.gameObject.SetActive(false);

        liquidOutside.runSim = true;
    }
}
