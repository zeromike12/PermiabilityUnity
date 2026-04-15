using UnityEngine;

public class LiquidOutside : MonoBehaviour
{
    Renderer renderer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        renderer = GetComponent<Renderer>();
        Material mat = renderer.material;
        mat.SetColor("_BaseColor", Color.red);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
