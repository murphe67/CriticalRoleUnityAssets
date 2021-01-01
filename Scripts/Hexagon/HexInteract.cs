using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using CriticalRole.UI;

public interface IHexInteract
{ 
    IHexagon hex { get; set; }
    I_UIManager MyUIManager { get; set; }
    void MakeSelectable();

    void MakeUnselectable();

    void ChangeColor(Color color);
}

public class HexInteract : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IHexInteract
{
    public I_UIManager MyUIManager { get; set; }
    public IHexagon hex { get; set; }
    public bool clickable = false;

    private void Awake()
    {
        MyMaterial = GetComponent<MeshRenderer>().material;
    }

    public Material MyMaterial;

    public void MakeSelectable()
    {
        clickable = true;
    }

    public void MakeUnselectable()
    {
        clickable = false;
        MyMaterial.SetColor("_Color", DefaultHexColor);
    }

    public void ChangeColor(Color color)
    {
        Color transColor = color;
        transColor.a = AlphaValue;
        MyMaterial.SetColor("_Color", transColor);
    }

    public Color DefaultHexColor
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
            MyUIManager.IHexagonClicked(hex);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(clickable)
        {
            MyUIManager.IHexagonHovered(hex);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (clickable)
        {
            MyUIManager.IHexagonUnhovered(hex);
        }
    }





}
