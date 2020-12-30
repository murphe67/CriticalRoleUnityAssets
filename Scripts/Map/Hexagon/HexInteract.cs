using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using CriticalRole.UI;

public interface IHexInteract
{ 
    IHexagon hex { get; set; }
    UI_Input MyUI_Input { get; set; }
    void MakeSelectable();

    void MakeUnselectable();

    void Highlight();

    void Unhighlight();
}

public class HexInteract : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IHexInteract
{
    public UI_Input MyUI_Input { get; set; }
    public IHexagon hex { get; set; }
    public bool clickable = false;

    private void Awake()
    {
        MyMaterial = GetComponent<MeshRenderer>().material;
    }

    public Material MyMaterial;

    public void MakeSelectable()
    {
        MyMaterial.SetColor("_Color", RedTransparent);
        clickable = true;
    }

    public void MakeUnselectable()
    {
        MyMaterial.SetColor("_Color", WhiteTransparent);
        clickable = false;
        
    }

    public void Highlight()
    {
        MyMaterial.SetColor("_Color", BlueTransparent);
    }

    public void Unhighlight()
    {
        MyMaterial.SetColor("_Color", RedTransparent);
    }

    public Color RedTransparent
    {
        get
        {
            return new Color(1, 0, 0, AlphaValue);
        }
    }

    public Color BlueTransparent
    {
        get
        {
            return new Color(0, 0, 1, AlphaValue);
        }
    }

    public Color WhiteTransparent
    {
        get
        {
            return new Color(1, 1, 1, 0.1f);
        }
    }


    public readonly float AlphaValue = 0.3f;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left && clickable)
        {
            MyUI_Input.IHexagonClicked(hex);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(clickable)
        {
            MyUI_Input.IHexagonHovered(hex);
        }
    }





}
