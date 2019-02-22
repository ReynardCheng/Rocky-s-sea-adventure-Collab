using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {

	public string description;
	public int woodCost, metalCost, essenceCost;
	public GameObject hoverObject, hoverResourcesObject;
	public Text hoverDescriptionText, hoverEssenceText, hoverWoodText, hoverMetalText;
	public bool showCosts;

	public void OnPointerEnter(PointerEventData pointerEventData){
		hoverObject.transform.position = transform.position;
		hoverObject.GetComponent<RectTransform>().anchoredPosition += Vector2.up * (GetComponent<RectTransform>().sizeDelta.y/2 + hoverObject.GetComponent<RectTransform>().sizeDelta.y/2) * hoverObject.transform.localScale.x;
		hoverObject.GetComponent<RectTransform>().anchoredPosition += Vector2.right * (hoverObject.GetComponent<RectTransform>().sizeDelta.x/2 - GetComponent<RectTransform>().sizeDelta.x/2) * hoverObject.transform.localScale.x;
		hoverObject.SetActive(true);
		
		hoverDescriptionText.text = description;

		if (showCosts){
			hoverResourcesObject.SetActive(true);
			hoverEssenceText.text = essenceCost.ToString();
			hoverMetalText.text = metalCost.ToString();
			hoverWoodText.text = woodCost.ToString();
		} else {
			hoverResourcesObject.SetActive(false);
		}
	}

	public void OnPointerExit(PointerEventData pointerEventData){
		hoverObject.SetActive(false);
	}

	public void OnPointerClick(PointerEventData pointerEventData){
		hoverObject.SetActive(false);
	}
}
