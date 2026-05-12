using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OsmosisSimulation : MonoBehaviour
{
    [System.Serializable]
    public class DialysisBag
    {
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

    private float simulatedMinutes = 20f;
    private bool isSimulating = false;

    [Header("UI Buttons")]
    public Button runButton;
    public Button resetButton;

    void Start()
    {
        foreach (var bag in bags)
        {
            if (bag.bagObject != null)
            {
                bag.initialScale = bag.bagObject.transform.localScale;
            }
        }

        UpdateAllWeightUI(startingWeightGrams);

        if (runButton != null) runButton.onClick.AddListener(StartSimulation);
        if (resetButton != null) resetButton.onClick.AddListener(ResetSimulation);
    }

    public void SetTimeInterval20() { simulatedMinutes = 20f; }
    public void SetTimeInterval40() { simulatedMinutes = 40f; }
    public void SetTimeInterval60() { simulatedMinutes = 60f; }

    public void StartSimulation()
    {
        if (!isSimulating)
        {
            StartCoroutine(RunOsmosisRoutine());
        }
    }

    private IEnumerator RunOsmosisRoutine()
    {
        isSimulating = true;
        float elapsedTime = 0f;

        // Arrays to hold our final targets
        Vector3[] targetScales = new Vector3[bags.Length];
        float[] targetWeights = new float[bags.Length];

        for (int i = 0; i < bags.Length; i++)
        {
            // 1. Calculate the REALISTIC target weight first
            float totalWeightGain = bags[i].concentrationPercent * simulatedMinutes * weightGainFactor;
            targetWeights[i] = startingWeightGrams + totalWeightGain;

            // 2. Calculate the true scientific volume ratio
            float volumeRatio = targetWeights[i] / startingWeightGrams;

            // 3. True scale is the cube root of the volume ratio
            float trueScaleRatio = Mathf.Pow(volumeRatio, 1f / 3f);

            // 4. Apply our visual exaggeration (so players can actually see it)
            float visualScaleRatio = 1f + ((trueScaleRatio - 1f) * visualSwellMultiplier);

            targetScales[i] = bags[i].initialScale * visualScaleRatio;
        }

        while (elapsedTime < realLifeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / realLifeDuration;
            float easedT = 1f - Mathf.Pow(1f - t, 3f);

            for (int i = 0; i < bags.Length; i++)
            {
                if (bags[i].bagObject != null)
                {
                    // Lerp visual scale
                    bags[i].bagObject.transform.localScale = Vector3.Lerp(bags[i].initialScale, targetScales[i], easedT);

                    // Lerp UI weight text independently 
                    float currentWeight = Mathf.Lerp(startingWeightGrams, targetWeights[i], easedT);
                    if (bags[i].weightText != null)
                    {
                        bags[i].weightText.text = currentWeight.ToString("F1") + " g";
                    }
                }
            }

            yield return null;
        }

        // Final snap to ensure exact numbers
        for (int i = 0; i < bags.Length; i++)
        {
            if (bags[i].bagObject != null)
            {
                bags[i].bagObject.transform.localScale = targetScales[i];
                if (bags[i].weightText != null)
                {
                    bags[i].weightText.text = targetWeights[i].ToString("F1") + " g";
                }
            }
        }

        isSimulating = false;
    }

    public void ResetSimulation()
    {
        StopAllCoroutines();
        isSimulating = false;

        foreach (var bag in bags)
        {
            if (bag.bagObject != null)
            {
                bag.bagObject.transform.localScale = bag.initialScale;
            }
        }

        UpdateAllWeightUI(startingWeightGrams);
    }

    private void UpdateAllWeightUI(float weightToDisplay)
    {
        foreach (var bag in bags)
        {
            if (bag.weightText != null)
            {
                bag.weightText.text = weightToDisplay.ToString("F1") + " g";
            }
        }
    }
}