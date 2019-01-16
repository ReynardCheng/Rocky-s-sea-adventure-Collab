using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum cannonTypes { normal, aoe, oilSlick,defence };

public class RaycastToWorld : MonoBehaviour {
	public static bool menuSpawned;

	public Camera cam;
	SpawnMenu theMenu;
    public LayerMask cannonMask;

	[Header("reference to cannon")]
	public GameObject normalCannon;
	public GameObject aoeCannon;
	public GameObject oiSlickCannon;
	public GameObject defenceCannon;

	[Header("reference to menus")]
	public GameObject normalMenu;
	public GameObject upgradeMenu;

	[SerializeField] GameObject menu;
	
	//[SerializeField]Button[] buttons;
	// Use this for initialization
	void Start () {

		menuSpawned = false;
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonDown(0) && !menuSpawned)
		{
			RaycastToWorldPos();
		}

	}

	void RaycastToWorldPos()
	{

		Ray ray = cam.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;


		if (Physics.Raycast(ray, out hit, Mathf.Infinity, cannonMask))
		{
			if (hit.collider != null)
			{
				if (hit.collider.gameObject.layer == 11 && hit.collider.transform.childCount < 1)
				{
					Transform hitPos = hit.collider.transform;
					menuSpawned = true;
					menu = Instantiate(normalMenu, hit.transform, true);
					menu.transform.position = new Vector3(hitPos.position.x, hitPos.position.y + 1, hitPos.position.z);
					menu.GetComponentInChildren<Button>().onClick.AddListener(BuildNormalCannon);
					menu.transform.SetParent(hit.collider.transform);
				}

				if (hit.collider.gameObject.layer == 12)
				{
					if (hit.collider.GetComponentInChildren<CannonController>().cannonType == cannonTypes.normal)
					{
						Transform hitPos = hit.collider.transform;
					

						menuSpawned = true;
						menu = Instantiate(upgradeMenu, hit.transform, true);
						menu.transform.position = new Vector3(hitPos.position.x, hitPos.position.y + 2, hitPos.position.z);
						menu.transform.SetParent(hit.collider.transform);
						Button[] buttons;
						buttons = menu.GetComponentsInChildren<Button>();

						foreach (Button b in buttons)
						{
							if (b.gameObject.name.Contains("upgradeAoe"))
							{
								b.GetComponentInChildren<Button>().onClick.AddListener(upgradeAoe);
							}
							if (b.gameObject.name.Contains("upgradeOilSlick"))
							{
								b.GetComponentInChildren<Button>().onClick.AddListener(upgradeOilSlick);
							}
							if (b.gameObject.name.Contains("upgradeDefence"))
							{
								b.GetComponentInChildren<Button>().onClick.AddListener(upgradeDefence);
							}
						}
					}
				}
			}
		}
	}

	public void BuildNormalCannon()
	{
		GameObject cannon = Instantiate(normalCannon, menu.transform.parent.position, Quaternion.identity);

		cannon.transform.parent = menu.transform.parent;
		cannon.transform.rotation = menu.transform.parent.transform.rotation;
		menuSpawned = false;
		Destroy(menu.gameObject);
	}

	public void upgradeAoe()
	{
		print("Aoe");
		GameObject cannon = Instantiate(aoeCannon, menu.transform.parent.position, Quaternion.identity);
		cannon.transform.rotation = menu.transform.parent.transform.rotation;
		cannon.transform.parent = menu.transform.parent.parent;
		menuSpawned = false;
		Destroy(menu.transform.parent.gameObject);
	
	}

	public void upgradeOilSlick()
	{
		print("Slick");
		GameObject cannon = Instantiate(oiSlickCannon, menu.transform.parent.position, Quaternion.identity);
		cannon.transform.rotation = menu.transform.parent.transform.rotation;
		cannon.transform.parent = menu.transform.parent.parent;
		menuSpawned = false;
		Destroy(menu.transform.parent.gameObject);
	}
	public void upgradeDefence()
	{
		print("Defence");
		GameObject cannon = Instantiate(defenceCannon, menu.transform.parent.position, Quaternion.identity);
		cannon.transform.rotation = menu.transform.parent.transform.rotation;
		cannon.transform.parent = menu.transform.parent.parent;
		menuSpawned = false;
		Destroy(menu.transform.parent.gameObject);
	}

	
}
