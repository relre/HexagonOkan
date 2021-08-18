using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject hexObjectPrefab;
    public GameObject bombObjectPrefab;

    static int canvasHeight = 8;
    static int canvasWidth = 9;
    static float hexGapX = 0.88f;
    static float hexGapY = 1f;

    float initialhexStartY = canvasWidth / 2 * -hexGapY;
    public Color[] hexagonColors;

    public int X = canvasWidth;
    public int Y = canvasHeight;

    public GameObject loginPanel;
    public Button startButton;
    public TextMeshProUGUI scoreText;
    public int score;

    Hexagon firstHexagon;
    Hexagon secondHexagon;
    Hexagon thirdHexagon;


    public GameObject[,] tempHexagons = new GameObject[canvasWidth, canvasHeight];

    void Start()
    {
        Invoke("StartGameDelay", 2.5f);
        GenerateHexagon();
        StartCoroutine(DestroyController());

    }
    void Update()
    {
        scoreText.text = score.ToString();
    }
    void GenerateHexagon()
    {
        float hexStartX = (canvasHeight / 2 * -hexGapX) - hexGapX;
        float hexStartY;

        for (int canvasXPosition = 0; canvasXPosition < canvasWidth; canvasXPosition++)
        {
            hexStartX += hexGapX;
            hexStartY = canvasXPosition % 2 == 0 ? initialhexStartY : initialhexStartY + 0.5f;

            for (int canvasYPosition = 0; canvasYPosition < canvasHeight; canvasYPosition++)
            {
                CreateHexagon(hexStartX, hexStartY, canvasXPosition, canvasYPosition);

                hexStartY += hexGapY;
            }
        }
    }
    bool ScanForExplode()
    {
        for (int x = 0; x < canvasWidth; x++)
        {
            for (int y = 0; y < canvasHeight; y++)
            {
                if (x != canvasWidth -1 && y != canvasHeight - 1)
                {
                    GameObject firstHexagonGameObject = tempHexagons[x, y];
                    firstHexagon = firstHexagonGameObject.GetComponent<Hexagon>();

                    int secondX = firstHexagon.x;
                    int secondY = firstHexagon.y;
                    secondY++;

                    GameObject secondHexagonGameObject = tempHexagons[secondX, secondY];
                    secondHexagon = secondHexagonGameObject.GetComponent<Hexagon>();

                    int thirdX = firstHexagon.x;
                    int thirdY = firstHexagon.y;

                    thirdX++;

                    if (firstHexagon.x % 2 != 0)
                    {
                        thirdY++;
                    }

                    GameObject thirdHexagonGameObject = tempHexagons[thirdX, thirdY];
                    thirdHexagon = thirdHexagonGameObject.GetComponent<Hexagon>();
                    if (firstHexagon.color == secondHexagon.color && firstHexagon.color == thirdHexagon.color)
                    {
                       return true;
                    }
                }
                if (x != 0 && y != 0)
                {
                    GameObject firstHexagonGameObject = tempHexagons[x, y];
                    firstHexagon = firstHexagonGameObject.GetComponent<Hexagon>();

                    int secondX = firstHexagon.x;
                    int secondY = firstHexagon.y;
                    secondY--;

                    GameObject secondHexagonGameObject = tempHexagons[secondX, secondY];
                    secondHexagon = secondHexagonGameObject.GetComponent<Hexagon>();

                    int thirdX = firstHexagon.x;
                    int thirdY = firstHexagon.y;
                    thirdX--;

                    if (firstHexagon.x % 2 == 0)
                    {
                        thirdY--;
                    }

                    GameObject thirdHexagonGameObject = tempHexagons[thirdX, thirdY];
                    thirdHexagon = thirdHexagonGameObject.GetComponent<Hexagon>();
                    if (firstHexagon.color == secondHexagon.color && firstHexagon.color == thirdHexagon.color)
                    {
                        return true;
                    }
                }
            }
        }
        return false;   
    }
    IEnumerator DestroyAndRegenerateHexagon()
    {
        Destroy(firstHexagon.gameObject);
        score += 5;
        Destroy(secondHexagon.gameObject);
        score += 5;
        Destroy(thirdHexagon.gameObject);
        score += 5;

        yield return new WaitForSeconds(0.2f);

        CreateHexagon(firstHexagon.hexStartX, firstHexagon.hexStartY, firstHexagon.x, firstHexagon.y);
        CreateHexagon(secondHexagon.hexStartX, secondHexagon.hexStartY, secondHexagon.x, secondHexagon.y);
        CreateHexagon(thirdHexagon.hexStartX, thirdHexagon.hexStartY, thirdHexagon.x, thirdHexagon.y);
    }
    IEnumerator DestroyAndRegenerateBomb()
    {
        Destroy(firstHexagon.gameObject);
        score += 5;
        Destroy(secondHexagon.gameObject);
        score += 5;
        Destroy(thirdHexagon.gameObject);
        score += 5;

        yield return new WaitForSeconds(0.2f);

        CreateBomb(firstHexagon.hexStartX, firstHexagon.hexStartY, firstHexagon.x, firstHexagon.y);
        CreateHexagon(secondHexagon.hexStartX, secondHexagon.hexStartY, secondHexagon.x, secondHexagon.y);
        CreateHexagon(thirdHexagon.hexStartX, thirdHexagon.hexStartY, thirdHexagon.x, thirdHexagon.y);
    }
    public IEnumerator DestroyController()
    {
        yield return new WaitForSeconds(1f);
        bool searchExplode = true;
        bool bombInGameNull;
        if (GameObject.FindGameObjectWithTag("HexagonBomb") == null)
        {
            bombInGameNull = true;
        }
        else
        {
            bombInGameNull = false;
        }
        while (searchExplode)
        {
            searchExplode = ScanForExplode();
            if (searchExplode)
            {
                Debug.Log(bombInGameNull);
                if (score < 1000 && bombInGameNull)
                {
                     StartCoroutine(DestroyAndRegenerateHexagon());
                }
                else if(score > 1000 && bombInGameNull)
                {
                    StopCoroutine(DestroyAndRegenerateHexagon());
                    StartCoroutine(DestroyAndRegenerateBomb());
                }
                else if (score > 1000 && !bombInGameNull)
                {
                    StopCoroutine(DestroyAndRegenerateBomb());
                    StartCoroutine(DestroyAndRegenerateHexagon());
                }
            }
            yield return new WaitForSeconds(0.2f);
        }
    }
    void CreateHexagon(float hexStartX, float hexStartY, int canvasXPosition, int canvasYPosition)
    {
        Vector2 hexStart = new Vector2(hexStartX, hexStartY);
        Color color = hexagonColors[Random.Range(0, hexagonColors.Length)];
        hexObjectPrefab.GetComponent<SpriteRenderer>().color = color;

        GameObject hexSpawnLocation = Instantiate(hexObjectPrefab, hexStart, Quaternion.identity);
        tempHexagons[canvasXPosition, canvasYPosition] = hexSpawnLocation;

        hexSpawnLocation.transform.SetParent(GameObject.FindGameObjectWithTag("Hexagon").transform, false);
        Hexagon hexagon = hexSpawnLocation.GetComponent<Hexagon>();
        hexagon.x = canvasXPosition;
        hexagon.y = canvasYPosition;
        hexagon.color = color;
        hexagon.hexStartX = hexStartX;
        hexagon.hexStartY = hexStartY;
    }
    void CreateBomb(float hexStartX, float hexStartY, int canvasXPosition, int canvasYPosition)
    {
        Vector2 hexStart = new Vector2(hexStartX, hexStartY);
        Color color = hexagonColors[Random.Range(0, hexagonColors.Length)];
        bombObjectPrefab.GetComponent<SpriteRenderer>().color = color;

        GameObject hexSpawnLocation = Instantiate(bombObjectPrefab, hexStart, Quaternion.identity);
        tempHexagons[canvasXPosition, canvasYPosition] = hexSpawnLocation;

        hexSpawnLocation.transform.SetParent(GameObject.FindGameObjectWithTag("Hexagon").transform, false);
        Hexagon hexagon = hexSpawnLocation.GetComponent<Hexagon>();
        hexagon.x = canvasXPosition;
        hexagon.y = canvasYPosition;
        hexagon.color = color;
        hexagon.hexStartX = hexStartX;
        hexagon.hexStartY = hexStartY;
    }

    public void StartGame()
    {
        loginPanel.gameObject.SetActive(false);
        score = 0;
    }
    void StartGameDelay()
    {
        startButton.interactable = true;
    }
}
