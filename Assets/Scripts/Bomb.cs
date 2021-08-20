using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public int bombNumber;
    public TextMeshPro bombText;
    void Start()
    {
        bombNumber = 5;
    }

    void Update()
    {
        bombText.GetComponent<TextMeshPro>().text = bombNumber.ToString();
    }
}
