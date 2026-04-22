using UnityEngine;

public class liquidInside : MonoBehaviour
{
    Renderer renderer;// dont touch
    public float liquidSize = 1;// size of the chemical. Smaller wins over larger.
    public Color liquidColor = Color.lightBlue;// base color of chemical do not change.
    public bool runSim = false; // check to run simulation. This can be changed outside of the script with a start or stop.
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        renderer = GetComponent<Renderer>();
        Material mat = renderer.material;//material controls for color
        mat.SetColor("_BaseColor", liquidColor);// how to change color
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setChemical(float size, Color color)//set chemical data outside of script. Through buttons or etc.
    {
        liquidSize = size;
        liquidColor = color;
    }
}
