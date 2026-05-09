using UnityEngine;
using UnityEngine.UI;

public class LiquidOutside : MonoBehaviour {
    Renderer rendererComponent;
    Renderer rendererOther;

    public float liquidSize = 1;
    public Color liquidColor = Color.blue;

    public GameObject insideBag;
    public float insideliquidSize = 1;
    public Color insideliquidColor = Color.blue;

    public bool runSim = false;

    public float diffusionSpeed = 0.5f; // Not affected by in-script changes, only inspector. In inspector set to 0.01 to reach 100% diffusion in ~1 min RL time.
    [SerializeField] private float currentDiffusion = 0f;

    Material mat;
    Material matOther;

    void Start() {
        // Beaker
        rendererComponent = GetComponent<Renderer>();
        mat = rendererComponent.material;

        // Ensure the shader properties are initialized
        mat.SetColor("_BaseColor", liquidColor);
        mat.SetColor("_TargetColor", liquidColor);
        mat.SetFloat("_DiffusionAmount", 0f);

        // Inside Bag
        rendererOther = insideBag.GetComponent<Renderer>();
        matOther = rendererOther.material;

        matOther.SetColor("_BaseColor", insideliquidColor);
        matOther.SetColor("_TargetColor", insideliquidColor);
        matOther.SetFloat("_DiffusionAmount", 0f);
    }


    void Update() {

        if (runSim) {
            // Increase the diffusion progress over time
            currentDiffusion += Time.deltaTime * diffusionSpeed;
            currentDiffusion = Mathf.Clamp01(currentDiffusion);
            insideBag.transform.localScale += new Vector3(0.0001f, 0.001f, 0.0001f);

            if (liquidSize < insideliquidSize) {
                // The outside liquid is smaller, so it diffuses INTO the inside bag.
                // The inside bag's target color becomes the outside liquid's color.
                matOther.SetColor("_TargetColor", liquidColor);
                matOther.SetFloat("_DiffusionAmount", currentDiffusion);
            }
            else if (liquidSize > insideliquidSize) {
                // The inside liquid is smaller, so it diffuses INTO the outside beaker.
                // The outside beaker's target color becomes the inside bag's color.
                mat.SetColor("_TargetColor", insideliquidColor);
                mat.SetFloat("_DiffusionAmount", currentDiffusion);
            }
            else {
                Debug.Log("Molecules are the same size. No diffusion across the membrane.");
            }
        }
        else {
            mat.SetColor("_BaseColor", liquidColor);
            matOther.SetColor("_BaseColor", insideliquidColor);
            mat.SetFloat("_DiffusionAmount", -1f);
            matOther.SetFloat("_DiffusionAmount", -1f);
        }
    }

    public void setChemical(float size, Color color) {
        liquidSize = size;
        liquidColor = color;
        // Reset simulation visuals if a new chemical is set
        if (mat != null) {
            mat.SetColor("_BaseColor", liquidColor);
            mat.SetFloat("_DiffusionAmount", 0f);
            currentDiffusion = 0f;
        }
    }
}