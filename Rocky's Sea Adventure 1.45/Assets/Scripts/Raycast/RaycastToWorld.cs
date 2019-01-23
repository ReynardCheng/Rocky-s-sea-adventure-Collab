using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum cannonTypes { normal, aoe, oilSlick,defence };

public class RaycastToWorld : MonoBehaviour {
	public static bool menuSpawned;

	public Camera cam;
	SpawnMenu theMenu;
    private ResourceCount resourceCount;
    public LayerMask cannonMask;

	[Header("reference to cannon")]
	public GameObject normalCannon;
	public GameObject aoeCannon;
	public GameObject oiSlickCannon;
	public GameObject defenceCannon;
    private GameObject selectedCannon;

	[Header("reference to menus")]
	public GameObject normalMenu;
	public GameObject upgradeMenu;
    public GameObject buildProgressSlider;

	[SerializeField] GameObject menu;
	
	//[SerializeField]Button[] buttons;
	// Use this for initialization
	void Start () {
        resourceCount = FindObjectOfType<ResourceCount>();
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
				if (hit.collider.gameObject.layer == 11 && hit.collider.GetComponent<CannonController>() == null)
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
                        selectedCannon = hit.collider.gameObject;
						menu.transform.position = new Vector3(hitPos.position.x, hitPos.position.y + 2, hitPos.position.z);
						menu.transform.SetParent(hit.collider.transform.parent);
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
        if (resourceCount.woodCount >= 1 && resourceCount.metalCount >=1)
        {
            resourceCount.WoodenPlankValue(1, 0);
            resourceCount.MetalValue(1, 0);
            StartCoroutine(BuildTime(4, normalCannon));
        }
	}

	public void upgradeAoe()
	{
        if (resourceCount.woodCount >= 1 && resourceCount.metalCount >= 3)
        {
            resourceCount.WoodenPlankValue(1, 0);
            resourceCount.MetalValue(3, 0);
            print("Aoe");
            Destroy(selectedCannon);
            StartCoroutine(BuildTime(7, aoeCannon));
            //GameObject cannon = Instantiate(aoeCannon, menu.transform.parent.position, Quaternion.identity);
            //cannon.transform.rotation = menu.transform.parent.transform.rotation;
            //cannon.transform.parent = menu.transform.parent.parent;
            //menuSpawned = false;
            //Destroy(menu.transform.parent.gameObject);
        }

    }

	public void upgradeOilSlick()
	{
        if (resourceCount.woodCount >= 3 && resourceCount.metalCount >= 1)
        {
            resourceCount.WoodenPlankValue(3, 0);
            resourceCount.MetalValue(1, 0);
            print("Slick");
            Destroy(selectedCannon);
            StartCoroutine(BuildTime(7, oiSlickCannon));
            //GameObject cannon = Instantiate(oiSlickCannon, menu.transform.parent.position, Quaternion.identity);
            //cannon.transform.rotation = menu.transform.parent.transform.rotation;
            //cannon.transform.parent = menu.transform.parent.parent;
            //menuSpawned = false;
            //Destroy(menu.transform.parent.gameObject);
        }
	}

	public void upgradeDefence()
	{
        if (resourceCount.woodCount >= 2 && resourceCount.metalCount >= 2)
        {
            resourceCount.WoodenPlankValue(2, 0);
            resourceCount.MetalValue(2, 0);
            print("Defence");
            Destroy(selectedCannon);
            StartCoroutine(BuildTime(7, defenceCannon));
            //GameObject cannon = Instantiate(defenceCannon, menu.transform.parent.position, Quaternion.identity);
            //cannon.transform.rotation = menu.transform.parent.transform.rotation;
            //cannon.transform.parent = menu.transform.parent.parent;
            //menuSpawned = false;
            //Destroy(menu.transform.parent.gameObject);
        }
	}

    public IEnumerator BuildTime(float buildTime, GameObject cannonToBuild)
    {
        float timeFinishedBuilding = Time.time + buildTime;
        GameObject canvas = Instantiate(buildProgressSlider, menu.transform.parent.position, Quaternion.identity);
        Slider slider = canvas.GetComponentInChildren<Slider>();

        canvas.transform.SetParent(menu.transform.parent);
        canvas.transform.rotation = Quaternion.Euler(0, -90, 0);

        menu.SetActive(false);

        while (Time.time < timeFinishedBuilding)
        {
            slider.value = 1 - ((timeFinishedBuilding - Time.time) / buildTime);
            yield return null;
        }

        Destroy(canvas);
        GameObject cannon = Instantiate(cannonToBuild, menu.transform.parent.position, Quaternion.identity);

        cannon.transform.parent = menu.transform.parent;
        cannon.transform.rotation = menu.transform.parent.transform.rotation;
        menuSpawned = false;
        Destroy(menu.gameObject);
    }
}
