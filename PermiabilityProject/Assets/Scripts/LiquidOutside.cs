using UnityEngine;

public class LiquidOutside : MonoBehaviour {
    TimerHandler timerHandler;

    Renderer rendererComponent;
    Renderer rendererOther;

    // Chemical Data
    public string outsideLiquidName = "";
    public float liquidSize = 1;
    public Color liquidColor = Color.blue;

    public GameObject insideBag;
    public string insideLiquidName = "";
    public float insideliquidSize = 1;
    public Color insideliquidColor = Color.blue;

    public bool runSim = false;

    public float diffusionSpeed = 0.5f;
    private float currentDiffusion = 0f;

    Material mat;
    Material matOther;

    void Start() {
        timerHandler = FindAnyObjectByType<TimerHandler>();

        // Beaker
        rendererComponent = GetComponent<Renderer>();
        mat = rendererComponent.material;

        // Inside Bag
        rendererOther = insideBag.GetComponent<Renderer>();
        matOther = rendererOther.material;

        UpdateStartingColors();
    }

    void Update() {
        if (runSim) {
            // Increase the diffusion progress over time
            currentDiffusion += (Time.deltaTime * diffusionSpeed) * timerHandler.fastForward;
            currentDiffusion = Mathf.Clamp01(currentDiffusion);

            insideBag.transform.localScale += new Vector3(0.00001f, 0f, 0.00001f) * timerHandler.fastForward;

            if (liquidSize < insideliquidSize) {
                // The outside liquid is smaller, so it diffuses INTO the inside bag.
                Color reactionColor = GetReactionColor(outsideLiquidName, insideLiquidName);
                matOther.SetColor("_TargetColor", reactionColor);
                matOther.SetFloat("_DiffusionAmount", currentDiffusion);
            }
            else if (liquidSize > insideliquidSize) {
                // The inside liquid is smaller, so it diffuses INTO the outside beaker.
                Color reactionColor = GetReactionColor(outsideLiquidName, insideLiquidName);
                mat.SetColor("_TargetColor", reactionColor);
                mat.SetFloat("_DiffusionAmount", currentDiffusion);
            }
            else {
                Debug.Log("Molecules are the same size. No diffusion across the membrane.");
            }
        }
        else {
            // Reset state when simulation is not running
            mat.SetColor("_BaseColor", liquidColor);
            matOther.SetColor("_BaseColor", insideliquidColor);
            mat.SetFloat("_DiffusionAmount", -1f);
            matOther.SetFloat("_DiffusionAmount", -1f);
        }
    }

    // Helper method to determine the actual chemical reaction color
    private Color GetReactionColor(string outsideName, string insideName) {
        // Iodine + Starch complex turns Dark Blue / Black
        if ((outsideName == "Iodine" && insideName == "Starch") || (outsideName == "Starch" && insideName == "Iodine")) {
            return new Color(0.05f, 0.05f, 0.2f); // Dark Indigo/Black
        }

        // Sodium Bicarbonate (Base) + M-Cresol Purple turns Purple
        if ((outsideName == "SodBi" && insideName == "MCresol") || (outsideName == "MCresol" && insideName == "SodBi")) {
            return new Color(0.5f, 0f, 0.5f); // Deep Purple
        }

        // If no specific chemical reaction, just blend the two colors naturally
        return Color.Lerp(liquidColor, insideliquidColor, 0.5f);
    }

    // Called by the UI buttons to visually refresh the beaker and bag before the sim starts
    public void UpdateStartingColors() {
        if (mat != null && matOther != null) {
            mat.SetColor("_BaseColor", liquidColor);
            mat.SetColor("_TargetColor", liquidColor);
            mat.SetFloat("_DiffusionAmount", 0f);

            matOther.SetColor("_BaseColor", insideliquidColor);
            matOther.SetColor("_TargetColor", insideliquidColor);
            matOther.SetFloat("_DiffusionAmount", 0f);

            currentDiffusion = 0f;
        }
    }

    // Kept your original method just in case other scripts depend on it
    public void setChemical(float size, Color color) {
        liquidSize = size;
        liquidColor = color;
        UpdateStartingColors();
    }
}