using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    public enum HexColor
    {
        red,
        green,
        blue
    };

    public GameObject[,] hexList;
    public GameObject HexObj;
    List<Vector2> m_AdjacentHexOddRow;
    List<Vector2> m_AdjacentHexEvenRow;

    List<Vector2> m_selectedHex;

    //On Scene load
    void Start()
    {
        hexList = new GameObject[6, 6];
        for (int x = 0; x < hexList.GetLength(0); x++)
        {
            for (int y = 0; y < hexList.GetLength(1); y++)
            {
                GameObject hex = Instantiate(HexObj);
                hex.name = "hex" + x + "_" + y;
                hex.GetComponent<HexObjScript>().SetHandler(this);
                hexList[x, y] = hex;
                SetRandomColor(hex);

                /**
                 *  CHANGE TRANSFORM
                 */

                float width = 3.464f;
                float height = 4.5f;
                float myx = x * width;
                float myy = -y * .5f * height;

                SpriteRenderer[] srList = hex.GetComponentsInChildren<SpriteRenderer>();

                if (y % 2 == 0)
                {
                    hex.transform.position = new Vector3(myx, myy, 1f);

                    foreach (SpriteRenderer sra in srList)
                        sra.sortingOrder += (y) * 10;
                }
                else
                {
                    myx += (.5f * width);

                    hex.transform.position = new Vector3(myx, myy, 1f);
                    foreach (SpriteRenderer sra in srList)
                        sra.sortingOrder += (y) * 10;
                }

                //  Debug.Log(sr.name + " "+myx + ", " + myy);
                //  Vector3 vec = hex.transform.position;
            }
        }
        m_AdjacentHexOddRow = new List<Vector2>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {

                if (x != -1 || y == 0)
                {
                    m_AdjacentHexOddRow.Add(new Vector2(x, y));
                }
            }
        }
        m_AdjacentHexEvenRow = new List<Vector2>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                bool notAdj = (x == -1 && y == -1);
                bool notAdj2 = (x == -1 && y == 1);

                if (x != 1 || y == 0)
                {
                    m_AdjacentHexEvenRow.Add(new Vector2(x, y));
                }
            }
        }


    }

    internal void HandleOnClick(HexObjScript hexObjScript)
    {

        foreach(Vector2 vec in m_selectedHex)
        {
            //Get Hex and script from hex
            GameObject hex = hexList[(int)vec.x, (int)vec.y];
            SetRandomColor(hex);
        }
        m_selectedHex = new List<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject ahex in hexList)
        {
            SpriteRenderer sr = ahex.GetComponentInChildren<SpriteRenderer>();
        }
    }

    private void OnDestroy()
    {
    }

    public void HandleOnEnter(HexObjScript hexScript)
    {
        m_selectedHex = new List<Vector2>
        {
            hexScript.GetCoords()
        };
        hexScript.MouseOverColor();
        SelectAdjacentHexColor(hexScript.GetCoords(), hexScript.GetHexColor());       
    }

    /**
     * Returns all adjacent Hex within bounds
     */
    private List<Vector2> GetAdjacentHex(Vector2 vec2)
    {
        List<Vector2> adjHex = new List<Vector2>();
        List<Vector2> m_AdjacentHex = m_AdjacentHexOddRow;
        int maxX = hexList.GetLength(0);
        int maxY = hexList.GetLength(1);

        if (vec2.y % 2 == 0)
        {
            m_AdjacentHex = m_AdjacentHexEvenRow;
        }

        foreach (Vector2 vec in m_AdjacentHex)
        {
            Vector2 adjHexVec = vec + vec2;
            if (adjHexVec.x < maxX && adjHexVec.x > 0 && adjHexVec.y < maxY && adjHexVec.y > 0)
            {
                adjHex.Add(vec);
            }
        }

        return adjHex;
    }

    /**
     * Returns Adjacent hex with matching "HexColor" 
     */
    private void SelectAdjacentHexColor(Vector2 vec2, HexColor color)
    {
        int maxX = hexList.GetLength(0);
        int maxY = hexList.GetLength(1);

        List<Vector2> m_AdjacentHex = m_AdjacentHexOddRow;

        if(vec2.y%2==0)
        {
            m_AdjacentHex = m_AdjacentHexEvenRow;
        }

        foreach (Vector2 vec in m_AdjacentHex)
        {
            Vector2 adjHexVec = vec + vec2;
            if (!m_selectedHex.Contains(adjHexVec))
            {
                if (adjHexVec.x < maxX && adjHexVec.x >= 0 && adjHexVec.y < maxY && adjHexVec.y >= 0)
                {
                    GameObject hex = hexList[(int)adjHexVec.x, (int)adjHexVec.y];
                    HexObjScript hexScript = hex.GetComponent<HexObjScript>();
                    if (hexScript.GetHexColor() == color)
                    {
                        m_selectedHex.Add(new Vector2(adjHexVec.x,adjHexVec.y));
                        SelectAdjacentHexColor(adjHexVec, color);
                        hexScript.MouseOverColor();
                    }
                }
            }
        }
    }

    public void HandleOnExit(HexObjScript myHexScript)
    {
        foreach(Vector2 vec in m_selectedHex)
        {
            GameObject hex = hexList[(int)vec.x, (int)vec.y];
            HexObjScript hexScript = hex.GetComponent<HexObjScript>();
            hexScript.MouseExitColor();
        }
        m_selectedHex = new List<Vector2>();
    }

    void SetRandomColor(GameObject ahex)
    {
        //Set Hex Colors randomly
        switch (Random.Range(0, 3))
        {
            case 0:
                ahex.GetComponent<HexObjScript>().SetHexColor(HexColor.red);
                break;
            case 1:
                ahex.GetComponent<HexObjScript>().SetHexColor(HexColor.blue);
                break;
            case 2:
                ahex.GetComponent<HexObjScript>().SetHexColor(HexColor.green);
                break;
        }
    }
}