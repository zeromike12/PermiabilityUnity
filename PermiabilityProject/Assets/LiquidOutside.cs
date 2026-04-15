using UnityEngine;

public class LiquidOutside : MonoBehaviour
{
    Renderer renderer;// dont touch
    public float liquidSize=1;// size of the chemical. Smaller wins over larger.
    public Color liquidColor = Color.blue;// base color of chemical do not change.
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
        if (runSim)//runs the simulation
        {
            if(liquidSize<0)//todo: set 0 to the variable of the inside bag size, setup similar script to this for the inside bag.
            {
                //todo: change bag color and increase size
            }
            else if (liquidSize>0)//same as before replace with variable of inside bag size
            {
                //todo: change the beaker color and decrease bag size
            }
            else
            {
                //Set dialogue or something else to show that 2 chemicals of the same kind do nothing.
            }
        }
    }

   public void setChemical(float size, Color color)//set chemical data outside of script. Through buttons or etc.
    {
      liquidSize=size;
      liquidColor=color;
    }
   

}
