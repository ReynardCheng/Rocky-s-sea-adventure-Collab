using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RadialButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image circle;
    public Image icon;
    public string actionName;
    public RadialMenu menu;

    Color defaultColor;

    public void OnPointerEnter(PointerEventData eventData)
    {
        menu.selected = this; //when clicked, set target as selected
        defaultColor = circle.color;
        circle.color = Color.white; //colour of button when selected
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        menu.selected = null;
        circle.color = Color.white;
    }
}