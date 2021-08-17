using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public List<GameObject> AllSelectableObject;
    public List<GameObject> NearObjects;
   
    GameManager gameManager;
    Hexagon[] selectedHexagons = new Hexagon[3];

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        ObjectSelecter();
    }

    void Update()
    {
        SelectObject();
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

                selectedHexagons[0] = firstHexagon;
                selectedHexagons[1] = secondHexagon;
                selectedHexagons[2] = thirdHexagon;


            }
            

        }


    }
    private void ClearOutlines()
    {
        for (int i = 0; i < selectedHexagons.Length; i++)
        {
            if (selectedHexagons[i] != null)
            {
                selectedHexagons[i].HideOutline();
            }
        }
    }



    public void ObjectSelecter()
    {
        foreach (var AllObject in FindObjectsOfType(typeof(GameObject)) as GameObject[])
        {
            if (AllObject.name == "Hexagon(Clone)")
            {
                AllSelectableObject.Add(AllObject);
            }
        }
        for (int i = 0; i < AllSelectableObject.Count; i++)
        {
            float x = Mathf.Abs(this.transform.position.x - AllSelectableObject[i].transform.position.x);
            float y = Mathf.Abs(this.transform.position.y - AllSelectableObject[i].transform.position.y);


            if (x > 0)
            {
                NearObjects.Add(AllSelectableObject[i]);
            }

        }
    }
}
