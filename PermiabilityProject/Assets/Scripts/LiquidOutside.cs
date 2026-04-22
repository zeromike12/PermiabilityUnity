using UnityEngine;

public class LiquidOutside : MonoBehaviour {
    // Renderers
    Renderer renderer;// dont touch
    Renderer rendererOther;// dont touch

    public float liquidSize = 1;// size of the chemical. Smaller wins over larger.
    public Color liquidColor = Color.blue;// base color of chemical do not change.
    public GameObject insideBag;
    public float insideliquidSize = 1;// size of the chemical. Smaller wins over larger.
    public Color insideliquidColor = Color.blue;// base color of chemical do not change.

    public bool runSim = false; // check to run simulation. This can be changed outside of the script with a start or stop.

    // Materials
    Material mat;
    Material matOther;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        //Beaker
        renderer = GetComponent<Renderer>();
        mat = renderer.material;//material controls for color
        mat.SetColor("_BaseColor", liquidColor);// how to change color
        //Inside Bag
        rendererOther = insideBag.GetComponent<Renderer>();
        matOther = rendererOther.material;//material controls for color
        matOther.SetColor("_BaseColor", insideliquidColor);// how to change color
    }

    // Update is called once per frame
    void Update() {
        if (runSim)//runs the simulation
        {
            if (liquidSize < insideliquidSize)//todo: set 0 to the variable of the inside bag size, setup similar script to this for the inside bag.
            {
                mat.SetColor("_BaseColor", liquidColor);
                matOther.SetColor("_BaseColor", liquidColor);// how to change color
            }
            else if (liquidSize > insideliquidSize)//same as before replace with variable of inside bag size
            {
                mat.SetColor("_BaseColor", insideliquidColor);
                matOther.SetColor("_BaseColor", insideliquidColor);// how to change color
            }
            else {
                //Set dialogue or something else to show that 2 chemicals of the same kind do nothing.
            }
        }
    }

    public void setChemical(float size, Color color)//set chemical data outside of script. Through buttons or etc.
     {
        liquidSize = size;
        liquidColor = color;
    }


}
