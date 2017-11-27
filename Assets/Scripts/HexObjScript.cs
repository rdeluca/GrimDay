using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexObjScript : MonoBehaviour {

    GameObject hex;

    //This stores the GameObject’s original color
    Color m_OriginalColor;
    //Get the GameObject’s mesh renderer to access the GameObject’s material and color
    SpriteRenderer sr;
    MapManager handler;
    int x;
    int y;
    MapManager.HexColor e_color;

    private void Awake()
    {
        hex = this.gameObject;
    }

    void Start ()
    {
        sr = hex.GetComponentInChildren<SpriteRenderer>();
        if (sr.color != null)
        {
            //Fetch the original color of the GameObject
            m_OriginalColor = sr.color;
        }
        
        System.String s = hex.name.Substring(3);
        System.String [] nums = s.Split('_');
        x = System.Int32.Parse(nums[0]);
        y = System.Int32.Parse(nums[1]);
    }

    public Vector2 GetCoords()
    {
        return new Vector2(x, y);
    }

    public void SetHandler(MapManager mapMang)
    {
        handler = mapMang;
    }

    private void OnMouseDown()
    {
        handler.HandleOnClick(this);
    }

    private void OnMouseUp()
    {
    }

    void OnMouseEnter()
    {
        handler.HandleOnEnter(this);
    }

    void OnMouseExit()
    {
        //Reset the color of the GameObject back to normal
        handler.HandleOnExit(this);
    }

    internal void MouseOverColor()
    {
        Color m_MouseOverColor = new Color(m_OriginalColor.r + .3f * m_OriginalColor.r,
                             m_OriginalColor.g + .3f * m_OriginalColor.g,
                             m_OriginalColor.b + .3f * m_OriginalColor.b);

        sr.color = m_MouseOverColor;
    }

    internal void MouseExitColor()
    {
        sr.color = m_OriginalColor;
    }

    public MapManager.HexColor GetHexColor()
    {
        return e_color;
    }

    public void SetHexColor(MapManager.HexColor hc)
    {
        e_color = hc;
        m_OriginalColor = GetColorRGB(hc);
        sr = hex.GetComponentInChildren<SpriteRenderer>();
        sr.color = m_OriginalColor;

    }

    private Color GetColorRGB(MapManager.HexColor hc)
    {
        Color color = Color.white;
        switch (hc)
        {
            case MapManager.HexColor.red:
                color = new Color(.5f, 0, 0);
                break;
            case MapManager.HexColor.green:
                color = new Color(0, .5f, 0);
                break;
            case MapManager.HexColor.blue:
                color = new Color(0, 0, .5f);
                break;
        }
        return color;
    }
}