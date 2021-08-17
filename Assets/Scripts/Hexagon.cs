using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hexagon : MonoBehaviour
{
    public int x;
    public int y;
    public Color color;
    public float hexStartX;
    public float hexStartY;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void ShowOutline()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }
    public void HideOutline()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }
  
}
