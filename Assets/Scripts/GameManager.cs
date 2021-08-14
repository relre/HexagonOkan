using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject hexObjectPrefab;
    Vector2 hexStart;
    [SerializeField]
    public static int canvasHeight = 8;
    static int canvasWidth = 9;
    static float hexGapX = 0.88f;
    static float hexGapY = 1f;
    float hexStartX = (canvasHeight / 2 * -hexGapX) - hexGapX;
    float hexStartY = 0f;
    float initialhexStartY = canvasWidth / 2 * -hexGapY;
    public Color[] hexagonColors;

    void Start()
    {
        

        for (int canvasXPosition = 0; canvasXPosition < canvasWidth; canvasXPosition++)
        {
            hexStartX += hexGapX;
            hexStartY = canvasXPosition % 2 == 0 ? initialhexStartY : initialhexStartY + 0.5f; 

            for (int canvasYPosition = 0; canvasYPosition < canvasHeight; canvasYPosition++)
            {
                hexStart = new Vector2(hexStartX, hexStartY);
                hexObjectPrefab.GetComponent<SpriteRenderer>().color = hexagonColors[Random.Range(0, 4)];
                Instantiate(hexObjectPrefab, hexStart, Quaternion.identity);
                hexStartY += hexGapY;
            }
        }
        
    }

    void Update()
    {


    }
}
