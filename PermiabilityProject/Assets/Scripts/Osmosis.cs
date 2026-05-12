using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class Osmosis : MonoBehaviour
{
    public class DialsysBag
    {
        public GameObject bagObj;
        public float concentrationPercent;
        public Vector3 initScale;
    }

    public DialsysBag[] bags;
    static float realDuration = 60f;
    public float growthFactor = 0.001f;
    public float simulatedTime; // in mins
    public bool doSim;
    public Button runButton;
    public Button resetButton;


    void Start()
    {
        foreach (var bag in bags)
        {
            if (bag.bagObj != null)
            {
                bag.initScale = bag.bagObj.transform.localScale;
            }
        }

        if (runButton != null) runButton.onClick.AddListener(StartSimulation);
        if (resetButton != null) resetButton.onClick.AddListener(ResetSimulation);
    }

    public void SetTimeInterval20() { simulatedTime = 20f; }
    public void SetTimeInterval40() { simulatedTime = 40f; }
    public void SetTimeInterval60() { simulatedTime = 60f; }

    public void StartSimulation()
    {
        if (!doSim)
        {
            StartCoroutine(RunSimulation());
        }
    }

    private IEnumerator RunSimulation()
    {
        doSim = true;
        float currentTime = 0f;
        Vector3[] targetScales = new Vector3[bags.Length];
        for (int i = 0; i < bags.Length; i++)
        {
            float totalGrowth = bags[i].concentrationPercent * simulatedTime * growthFactor;
            targetScales[i] = bags[i].initScale + new Vector3(totalGrowth, totalGrowth, totalGrowth);
        }

        while (currentTime < realDuration)
        {
            currentTime += Time.deltaTime;

            float time = currentTime / realDuration;
            float easedPressure = 1f - Mathf.Pow(1f - time, 3f);
            for (int i = 0; i < bags.Length; i++)
            {
                if (bags[i].bagObj != null)
                {
                    bags[i].bagObj.transform.localScale = Vector3.Lerp(bags[i].initScale, targetScales[i], easedPressure);
                }
            }

            yield return null;
        }

        for (int i = 0; i < bags.Length; i++)
        {
            if (bags[i].bagObj != null)
            {
                bags[i].bagObj.transform.localScale = targetScales[i];
            }
        }

        doSim = false;

    }

    public void ResetSimulation()
    {
        StopAllCoroutines();
        doSim = false;

        foreach (var bag in bags)
        {
            if (bag.bagObj != null)
            {
                bag.bagObj.transform.localScale = bag.initScale;
            }
        }
    }

}
