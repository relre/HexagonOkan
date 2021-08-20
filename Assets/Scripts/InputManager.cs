using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public List<GameObject> AllSelectableObject;
    public List<GameObject> NearObjects;
    Vector2 firstHexagonTempPosition;
    Vector2 secondHexagonTempPosition;
    Vector2 thirdHexagonTempPosition;

    GameManager gameManager;
    GameObject[] selectedHexagonGameObjects = new GameObject[3];

    bool isRotateOff = false;
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        SelectObject();
        if(isRotateOff)
        {
            Hexagon firstHexagon = selectedHexagonGameObjects[0].gameObject.GetComponent<Hexagon>();
            Hexagon secondHexagon = selectedHexagonGameObjects[1].gameObject.GetComponent<Hexagon>();
            Hexagon thirdHexagon = selectedHexagonGameObjects[2].gameObject.GetComponent<Hexagon>();

            firstHexagon.transform.position = Vector2.MoveTowards(firstHexagon.transform.position, secondHexagonTempPosition, Time.deltaTime * 2f);
            secondHexagon.transform.position = Vector2.MoveTowards(secondHexagon.transform.position, thirdHexagonTempPosition, Time.deltaTime * 2f);
            thirdHexagon.transform.position = Vector2.MoveTowards(thirdHexagon.transform.position, firstHexagonTempPosition, Time.deltaTime * 2f);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isRotateOff = true;
            Rotate();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            isRotateOff = false;
        }
    }
    private void SelectObject()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

            if (hit.collider != null)
            {
                ClearOutlines();
                selectedHexagonGameObjects = new GameObject[3];
                GameObject firstHexagonGameObject = hit.collider.gameObject;
                Hexagon firstHexagon = hit.collider.gameObject.GetComponent<Hexagon>();
                firstHexagon.ShowOutline();
                
                int secondX = firstHexagon.x;
                int secondY = firstHexagon.y;

                if (secondY == gameManager.Y - 1)
                {
                    secondY--;
                }
                else
                {
                    secondY++;
                }

                GameObject secondHexagonGameObject = gameManager.tempHexagons[secondX, secondY];
                Hexagon secondHexagon = secondHexagonGameObject.GetComponent<Hexagon>();
                secondHexagon.ShowOutline();

                int thirdX = firstHexagon.x;
                int thirdY = firstHexagon.y;

                if (thirdX == gameManager.X - 1)
                {
                    thirdX--;
                }
                else
                {
                    thirdX++;
                }
                if (firstHexagon.x % 2 != 0 && firstHexagon.y != gameManager.Y -1)
                {
                    thirdY++;
                }
                else if (firstHexagon.y == gameManager.Y -1 && firstHexagon.x % 2 == 0)
                {
                    thirdY--;
                }

                GameObject thirdHexagonGameObject = gameManager.tempHexagons[thirdX, thirdY];
                Hexagon thirdHexagon = thirdHexagonGameObject.GetComponent<Hexagon>();
                thirdHexagon.ShowOutline();

                selectedHexagonGameObjects[0] = firstHexagonGameObject;
                selectedHexagonGameObjects[1] = secondHexagonGameObject;
                selectedHexagonGameObjects[2] = thirdHexagonGameObject;

                firstHexagonTempPosition = firstHexagon.transform.position;
                secondHexagonTempPosition = secondHexagon.transform.position;
                thirdHexagonTempPosition = thirdHexagon.transform.position;  
            }
        }
    }
    void Rotate()
    {
        Hexagon firstHexagon = selectedHexagonGameObjects[0].gameObject.GetComponent<Hexagon>();
        Hexagon secondHexagon = selectedHexagonGameObjects[1].gameObject.GetComponent<Hexagon>();
        Hexagon thirdHexagon = selectedHexagonGameObjects[2].gameObject.GetComponent<Hexagon>();

        Hexagon firstHexagonTemp = Instantiate(selectedHexagonGameObjects[0].gameObject.GetComponent<Hexagon>());
        firstHexagonTemp.gameObject.SetActive(false);
        Hexagon secondHexagonTemp = Instantiate(selectedHexagonGameObjects[1].gameObject.GetComponent<Hexagon>());
        secondHexagonTemp.gameObject.SetActive(false);
        Hexagon thirdHexagonTemp = Instantiate(selectedHexagonGameObjects[2].gameObject.GetComponent<Hexagon>());
        thirdHexagonTemp.gameObject.SetActive(false);

        firstHexagon.x = secondHexagonTemp.x;
        firstHexagon.y = secondHexagonTemp.y;
        firstHexagon.hexStartX = secondHexagonTemp.hexStartX;
        firstHexagon.hexStartY = secondHexagonTemp.hexStartY;

        gameManager.tempHexagons[firstHexagon.x, firstHexagon.y] = selectedHexagonGameObjects[0];

        secondHexagon.x = thirdHexagonTemp.x;
        secondHexagon.y = thirdHexagonTemp.y;
        secondHexagon.hexStartX = thirdHexagonTemp.hexStartX;
        secondHexagon.hexStartY = thirdHexagonTemp.hexStartY;
        gameManager.tempHexagons[secondHexagon.x, secondHexagon.y] = selectedHexagonGameObjects[1];

        thirdHexagon.x = firstHexagonTemp.x;
        thirdHexagon.y = firstHexagonTemp.y;
        thirdHexagon.hexStartX = firstHexagonTemp.hexStartX;
        thirdHexagon.hexStartY = firstHexagonTemp.hexStartY;
        gameManager.tempHexagons[thirdHexagon.x, thirdHexagon.y] = selectedHexagonGameObjects[2];

        Destroy(firstHexagonTemp);
        Destroy(secondHexagonTemp);
        Destroy(thirdHexagonTemp);
        gameManager.StartCoroutine(gameManager.DestroyController());
    }
    private void ClearOutlines()
    {
        for (int i = 0; i < selectedHexagonGameObjects.Length; i++)
        {
            if (selectedHexagonGameObjects[i] != null)
            {
                selectedHexagonGameObjects[i].gameObject.GetComponent<Hexagon>().HideOutline();
            }
        }
    }
    
}
