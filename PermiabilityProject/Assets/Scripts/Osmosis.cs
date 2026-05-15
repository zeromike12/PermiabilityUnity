using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mono.Cecil.Cil;
using Unity.VisualScripting;

public class OsmosisSimulation : MonoBehaviour {
    BurnieHandler burnieHandler;

    [System.Serializable]
    public class DialysisBag {
        [Tooltip("Drag the bag GameObject here")]
        public GameObject bagObject;

        [Tooltip("Concentration of Karo syrup (0, 25, 50, 75)")]
        public float concentrationPercent;

        [Tooltip("Drag the UI Text object that will display this bag's weight")]
        public TMP_Text weightText;

        [HideInInspector]
        public Vector3 initialScale;
    }

    [Header("Lab Setup")]
    public DialysisBag[] bags;

    [Tooltip("The starting weight of every bag in grams")]
    public float startingWeightGrams = 15.0f;

    [Header("Science Math")]
    [Tooltip("Grams gained per 1% syrup per minute. 0.005 is realistic (75% bag gains ~22g in 60 mins)")]
    public float weightGainFactor = 0.005f;

    [Tooltip("Exaggerates the visual size of the bag so students can actually see it swelling, without breaking the text weight.")]
    public float visualSwellMultiplier = 2.0f;

    [Header("Simulation Settings")]
    public float realLifeDuration = 60f;

    [SerializeField] private float simulatedMinutes = 20f;
    public bool isSimulating = false;

    [Header("UI Buttons")]
    public Button runButton;
    public Button resetButton;

    [Header("Interval Buttons")]
    public Button simButton_20;
    public Button simButton_40;
    public Button simButton_60;

    public Button confirmPredictionButton;
    public GameObject creditScreen;
    public GameObject buttonHolder;

    void Start() {
        burnieHandler = FindAnyObjectByType<BurnieHandler>();
        runButton.interactable = false;
        confirmPredictionButton.gameObject.SetActive(false);

        simButton_20.interactable = false;
        simButton_40.interactable = false;
        simButton_60.interactable = false;

        foreach (var bag in bags) {
            if (bag.bagObject != null) {
                bag.initialScale = bag.bagObject.transform.localScale;
            }
        }

        UpdateAllWeightUI(startingWeightGrams);

        //if (runButton != null) runButton.onClick.AddListener(StartSimulation);
        //if (resetButton != null) resetButton.onClick.AddListener(ResetSimulation);
    }

    private void Update() {
        if (isSimulating) {
            burnieHandler.StopTalking();
            burnieHandler.burnie.gameObject.SetActive(false);
            burnieHandler.speechBubble.gameObject.SetActive(false);
        }
        else {
            burnieHandler.burnie.gameObject.SetActive(true);
            burnieHandler.speechBubble.gameObject.SetActive(true);
        }

        switch (burnieHandler.index) {
            case 3:
                simButton_20.interactable = true;
                break;
            case 5:
                if (!burnieHandler.isTalking) {
                    confirmPredictionButton.gameObject.SetActive(true);
                }
                break;
            case 8:
                if (!burnieHandler.isTalking) {
                    confirmPredictionButton.gameObject.SetActive(true);
                }
                break;
            case 11:
                if (!burnieHandler.isTalking) {
                    confirmPredictionButton.gameObject.SetActive(true);
                }
                break;
            default:
                break;
        }
    }

    public void SetTimeInterval20() {
        simulatedMinutes = 20f;

        simButton_20.interactable = false;
        simButton_40.interactable = true;
        simButton_60.interactable = true;

        if (burnieHandler.index == 3) {
            burnieHandler.Talk(4);
            burnieHandler.index = 4;
        }
    }
    public void SetTimeInterval40() {
        simulatedMinutes = 40f;

        simButton_20.interactable = true;
        simButton_40.interactable = false;
        simButton_60.interactable = true;

        if (burnieHandler.index == 7) {
            burnieHandler.Talk(8);
            burnieHandler.index = 8;
        }
    }
    public void SetTimeInterval60() {
        simulatedMinutes = 60f;

        simButton_20.interactable = true;
        simButton_40.interactable = true;
        simButton_60.interactable = false;

        if (burnieHandler.index == 10) {
            burnieHandler.Talk(11);
            burnieHandler.index = 11;
        }
    }

    public void StartSimulation() {
        if (burnieHandler.index == 6 && simulatedMinutes == 20) {
            if (!isSimulating) StartCoroutine(RunOsmosisRoutine());
        }
        else if (burnieHandler.index == 9 && simulatedMinutes == 40) {
            if (!isSimulating) StartCoroutine(RunOsmosisRoutine());
        }
        else if (burnieHandler.index == 12 && simulatedMinutes == 60) {
            if (!isSimulating) StartCoroutine(RunOsmosisRoutine());
        }
    }

    private IEnumerator RunOsmosisRoutine() {
        isSimulating = true;
        resetButton.interactable = false;
        buttonHolder.gameObject.SetActive(false);
        creditScreen.gameObject.SetActive(false);

        float elapsedTime = 0f;

        runButton.gameObject.SetActive(false);
        resetButton.gameObject.SetActive(true);

        // Arrays to hold our final targets
        Vector3[] targetScales = new Vector3[bags.Length];
        float[] targetWeights = new float[bags.Length];

        for (int i = 0; i < bags.Length; i++) {
            float totalWeightGain = bags[i].concentrationPercent * simulatedMinutes * weightGainFactor;
            targetWeights[i] = startingWeightGrams + totalWeightGain;
            float volumeRatio = targetWeights[i] / startingWeightGrams;
            float trueScaleRatio = Mathf.Pow(volumeRatio, 1f / 3f);
            float visualScaleRatio = 1f + ((trueScaleRatio - 1f) * visualSwellMultiplier);

            targetScales[i] = bags[i].initialScale * visualScaleRatio;
        }

        while (elapsedTime < realLifeDuration) {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / realLifeDuration;
            float easedT = 1f - Mathf.Pow(1f - t, 3f);

            for (int i = 0; i < bags.Length; i++) {
                if (bags[i].bagObject != null) {
                    // Larp visual scale
                    bags[i].bagObject.transform.localScale = Vector3.Lerp(bags[i].initialScale, targetScales[i], easedT);

                    // Larp UI weight text independently 
                    float currentWeight = Mathf.Lerp(startingWeightGrams, targetWeights[i], easedT);
                    if (bags[i].weightText != null) {
                        bags[i].weightText.text = currentWeight.ToString("F1") + " g";
                    }
                }
            }

            yield return null;
        }

        // Final snap to ensure exact numbers
        for (int i = 0; i < bags.Length; i++) {
            if (bags[i].bagObject != null) {
                bags[i].bagObject.transform.localScale = targetScales[i];
                if (bags[i].weightText != null) {
                    bags[i].weightText.text = targetWeights[i].ToString("F1") + " g";
                }
            }
        }

        isSimulating = false;
        resetButton.interactable = true;

        burnieHandler.Talk(14);
    }

    public void ResetSimulation() {
        StopAllCoroutines();
        isSimulating = false;

        runButton.interactable = false;
        resetButton.gameObject.SetActive(false);

        runButton.gameObject.SetActive(true);
        buttonHolder.gameObject.SetActive(true);
        creditScreen.gameObject.SetActive(true);

        simButton_20.interactable = false;
        simButton_40.interactable = false;
        simButton_60.interactable = false;

        foreach (var bag in bags) {
            if (bag.bagObject != null) {
                bag.bagObject.transform.localScale = bag.initialScale;
            }
        }

        UpdateAllWeightUI(startingWeightGrams);

        if (burnieHandler.index == 6) {
            burnieHandler.Talk(7);
            burnieHandler.index = 7;

            simButton_40.interactable = true;
        }
        else if (burnieHandler.index == 9) {
            burnieHandler.Talk(10);
            burnieHandler.index = 10;

            simButton_60.interactable = true;
        }
        else if (burnieHandler.index == 12) {
            burnieHandler.Talk(13);
            burnieHandler.index = 13;
        }
    }

    public void ConfirmPredictionButtonClicked() {
        confirmPredictionButton.gameObject.SetActive(false);

        if (burnieHandler.index == 5) {
            burnieHandler.Talk(6);
            burnieHandler.index = 6;

            runButton.interactable = true;
        }
        else if (burnieHandler.index == 8) {
            burnieHandler.Talk(9);
            burnieHandler.index = 9;

            runButton.interactable = true;
        }
        else if (burnieHandler.index == 11) {
            burnieHandler.Talk(12);
            burnieHandler.index = 12;

            runButton.interactable = true;
        }
    }

    private void UpdateAllWeightUI(float weightToDisplay) {
        foreach (var bag in bags) {
            if (bag.weightText != null) {
                bag.weightText.text = weightToDisplay.ToString("F1") + " g";
            }
        }
    }
}