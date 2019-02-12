using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum cannonTypes { normal, aoe, oilSlick, defence };

public class BuildCannonManager : MonoBehaviour
{
    public static bool menuSpawned;
    
    SpawnMenu theMenu;
    public ResourceCount resourceCount;
    public BoatController boatController;
    public BoatCombat1 boatCombat;
    private CharacterMovement characterMovement;
    private CameraSwitch cameraSwitch;
    private UnityStandardAssets.Characters.FirstPerson.FirstPersonController fpsController;

    [Header("References to Cannons and Ship")]
    public GameObject normalCannon;
    public GameObject aoeCannon;
    public GameObject oilSlickCannon;
    public GameObject defenceCannon;
    private GameObject selectedCannon;
    public GameObject shipPartsToHide;

    [Header("reference to menus")]
    public GameObject normalMenu;
    public GameObject upgradeMenu;
    public GameObject dismantleMenu;
    public GameObject buildProgressSlider;
    public GameObject mastMenu;
    public GameObject mastInteractMenu;
    public GameObject cannonInteractMenuPrefab;
    private GameObject cannonInteractMenu;
    public Slider boostSlider;

    [SerializeField] GameObject menu;

    public Transform buildLeftView;
    public Transform buildRightView;
    public Transform buildMastView;

    private bool inRangeToBuild;
    private bool inShipMastRange;
    private GameObject cannonSlot;

    //[SerializeField]Button[] buttons;
    // Use this for initialization
    void Awake()
    {
        characterMovement = GetComponent<CharacterMovement>();
        boatController = FindObjectOfType<BoatController>();
        boatCombat = FindObjectOfType<BoatCombat1>();
		resourceCount = FindObjectOfType<ResourceCount>();
		cameraSwitch = FindObjectOfType<CameraSwitch>();
        fpsController = GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
        menuSpawned = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !cameraSwitch.switching)
        {

            if (!menuSpawned && inRangeToBuild)
            {
				cameraSwitch.switching = true;
				OpenBuildMenu();
                LockCameraMovement();
            }
            else if (!menuSpawned && inShipMastRange)
            {
				cameraSwitch.switching = true;
				OpenMastMenu();
                LockCameraMovement();
            }
        }
    }

    void OpenBuildMenu()
    {
        if (cannonSlot != null)
        {
            Transform spawnLocation = cannonSlot.transform;

            //If a cannon hasnt been built inside the cannonslot,
            //start building a normal cannon
            if (cannonSlot.GetComponentInChildren<CannonController>() == null)
            {
                //Spawning, setting position and parent of menu
                menu = Instantiate(normalMenu, cannonSlot.transform, true);
                menu.transform.position = new Vector3(spawnLocation.position.x, spawnLocation.position.y + 2, spawnLocation.position.z);
                menu.transform.SetParent(cannonSlot.transform);
                menuSpawned = true;

                //Looping through all the buttons and giving them functionality
                Button[] buttons;
                buttons = menu.GetComponentsInChildren<Button>();

                foreach (Button b in buttons)
                {
                    if (b.gameObject.name.Contains("buildNormal"))
                    {
                        b.GetComponentInChildren<Button>().onClick.AddListener(BuildNormalCannon);
                        print("good luck");
                    }
                    if (b.gameObject.name.Contains("Cancel"))
                    {
                        b.GetComponentInChildren<Button>().onClick.AddListener(Cancel);
                    }
                    if (b.gameObject.name.Contains("Dismantle")) 
                    {
                        b.GetComponentInChildren<Button>().onClick.AddListener(DismantleCannon);
                    }
                }
            }
            //But if there is already a normal cannon previously built,
            else if (cannonSlot.GetComponentInChildren<CannonController>().cannonType == cannonTypes.normal)
            {
                menu = Instantiate(upgradeMenu, cannonSlot.transform, true);
                menu.transform.position = new Vector3(spawnLocation.position.x, spawnLocation.position.y + 3, spawnLocation.position.z);
                menu.transform.SetParent(cannonSlot.transform);
                menuSpawned = true;

                selectedCannon = cannonSlot.GetComponentInChildren<CannonController>().gameObject.transform.parent.gameObject;

                //Looping through all the buttons and giving them functionality
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
                    if (b.gameObject.name.Contains("Cancel"))
                    {
                        b.GetComponentInChildren<Button>().onClick.AddListener(Cancel);
                    }
                    if (b.gameObject.name.Contains("Dismantle"))
                    {
                        b.GetComponentInChildren<Button>().onClick.AddListener(DismantleCannon);
                    }
                }
            }
            //Else it has to be an oil slick, aoe, or defence cannon
            else
            {
                menu = Instantiate(dismantleMenu, cannonSlot.transform, true);
                menu.transform.position = new Vector3(spawnLocation.position.x, spawnLocation.position.y + 3, spawnLocation.position.z);
                menu.transform.SetParent(cannonSlot.transform);
                menuSpawned = true;

                selectedCannon = cannonSlot.GetComponentInChildren<CannonController>().gameObject.transform.parent.gameObject;

                //Looping through all the buttons and giving them functionality
                Button[] buttons;
                buttons = menu.GetComponentsInChildren<Button>();

                foreach (Button b in buttons)
                {
                    if (b.gameObject.name.Contains("Cancel"))
                    {
                        b.GetComponentInChildren<Button>().onClick.AddListener(Cancel);
                    }
                    if (b.gameObject.name.Contains("Dismantle"))
                    {
                        b.GetComponentInChildren<Button>().onClick.AddListener(DismantleCannon);
                    }
                }
            }
        }
    }

    public void BuildNormalCannon()
    {
        if (resourceCount.woodCount >= 1 && resourceCount.metalCount >= 1)
        {
            resourceCount.WoodenPlankValue(1, 0);
            resourceCount.MetalValue(1, 0);
            StartCoroutine(BuildTime(7, normalCannon));
        }
    }

    public void upgradeAoe()
    {
        if (resourceCount.woodCount >= 1 && resourceCount.metalCount >= 3)
        {
            resourceCount.WoodenPlankValue(1, 0);
            resourceCount.MetalValue(3, 0);
            print("Aoe");
            StartCoroutine(BuildTime(15, aoeCannon));
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
            StartCoroutine(BuildTime(15, oilSlickCannon));
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
            StartCoroutine(BuildTime(15, defenceCannon));
            //GameObject cannon = Instantiate(defenceCannon, menu.transform.parent.position, Quaternion.identity);
            //cannon.transform.rotation = menu.transform.parent.transform.rotation;
            //cannon.transform.parent = menu.transform.parent.parent;
            //menuSpawned = false;
            //Destroy(menu.transform.parent.gameObject);
        }
    }

    public void DismantleCannon()
    {
        CannonController cannon = menu.transform.parent.GetComponentInChildren<CannonController>();
        if (cannon == null)
            return;

        int cannonHealth = cannon.health;

        switch (cannon.cannonType)
        {
            case cannonTypes.normal:
                if (cannonHealth <= 15)
                {
                    resourceCount.WoodenPlankValue(0, 0);   //YO ARTISTS/GAME DESIGNERS -  Format is [TypeOfResource](resourcesToDeduct, resourcesToAdd);
                    resourceCount.MetalValue(0, 0);         //Say you wanna give 5 wood planks. Do resourceCount.WoodenPlankValue(0, 5);
                }
                else
                {
                    resourceCount.WoodenPlankValue(0, 0);
                    resourceCount.MetalValue(0, 0);
                }
                break;

            case cannonTypes.aoe:
                if (cannonHealth <= 15)
                {
                    resourceCount.WoodenPlankValue(0, 0);
                    resourceCount.MetalValue(0, 1);
                }
                else
                {
                    resourceCount.WoodenPlankValue(0, 0);
                    resourceCount.MetalValue(0, 2);
                }
                break;

            case cannonTypes.oilSlick:
                if (cannonHealth <= 15)
                {
                    resourceCount.WoodenPlankValue(0, 1);
                    resourceCount.MetalValue(0, 0);
                }
                else
                {
                    resourceCount.WoodenPlankValue(0, 2);
                    resourceCount.MetalValue(0, 0);
                }
                break;

            case cannonTypes.defence:
                if (cannonHealth <= 15)
                {
                    resourceCount.WoodenPlankValue(0, 0);
                    resourceCount.MetalValue(0, 0);
                }
                else
                {
                    resourceCount.WoodenPlankValue(0, 1);
                    resourceCount.MetalValue(0, 1);
                }
                break;
        }
        shipPartsToHide.SetActive(true);
        cameraSwitch.locked = false;
        fpsController.controllingShip = false;

        Destroy(selectedCannon);
        Destroy(menu.gameObject);
        menuSpawned = false;
    }
       
    public IEnumerator BuildTime(int timesToSpam, GameObject cannonToBuild)
    {
        float timesSpammed = 0;
        
        GameObject canvas = Instantiate(buildProgressSlider, menu.transform.parent.position, Quaternion.identity);
        Slider slider = canvas.GetComponentInChildren<Slider>();

        canvas.transform.SetParent(menu.transform.parent);
        canvas.transform.rotation = Quaternion.Euler(0, -90, 0);

        menu.SetActive(false);

        //Start Spamming - Start Building Sequence
        while (timesSpammed < timesToSpam)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                timesSpammed++;
                slider.value = timesSpammed / timesToSpam;
            }
            yield return null;
        }
        //Finished Building Sequence

        shipPartsToHide.SetActive(true);
        cameraSwitch.locked = false;
        fpsController.controllingShip = false;

		Destroy(canvas);
        Destroy(selectedCannon);

        GameObject cannon = Instantiate(cannonToBuild, menu.transform.parent.position, Quaternion.identity);

        cannon.transform.parent = menu.transform.parent;
        cannon.transform.rotation = menu.transform.parent.transform.rotation;
        menuSpawned = false;
        Destroy(menu.gameObject);
    }

    void OpenMastMenu()
    {
        Transform spawnLocation = cannonSlot.transform;

       // boostSlider.gameObject.SetActive(true);

        menu = Instantiate(mastMenu, cannonSlot.transform, true);
        menu.transform.position = new Vector3(spawnLocation.position.x, spawnLocation.position.y + 2, spawnLocation.position.z - 0.1f);
        menu.transform.SetParent(cannonSlot.transform);
        menuSpawned = true;

        Button[] buttons;
        buttons = menu.GetComponentsInChildren<Button>();

        foreach (Button b in buttons)
        {
            if (b.gameObject.name.Contains("Repair"))
            {
                b.GetComponentInChildren<Button>().onClick.AddListener(RepairShip);
            }
            if (b.gameObject.name.Contains("Refill"))
            {
                b.GetComponentInChildren<Button>().onClick.AddListener(RefillBoost);
            }
            if (b.gameObject.name.Contains("Cancel"))
            {
                b.GetComponentInChildren<Button>().onClick.AddListener(Cancel);
            }
        }
    }

    void RepairShip()
    {
        if (resourceCount.seaEssenceCount >= 1 && boatCombat.shipHealth < boatCombat.shipMaxHP)
        {
            resourceCount.SeaEssenceValue(1, 0);
            boatCombat.DamageShip(-15);
            if (boatCombat.shipHealth > boatCombat.shipMaxHP)
            {
                boatCombat.shipHealth = (int)boatCombat.shipMaxHP;
                boatCombat.DamageShip(0);   //This extra call needed to update UI
            }
        }
    }

    void RefillBoost()
    {
        if (resourceCount.seaEssenceCount >= 1 && boatController.boost < 100f)
        {
            resourceCount.SeaEssenceValue(1, 0);
            boatController.boost += 15f;
            if (boatController.boost > 100f)
                boatController.boost = 100f;
        }
    }

    void LockCameraMovement()
    {
        cameraSwitch.locked = true;
        fpsController.controllingShip = true;

        if (inRangeToBuild)
        {
            if (characterMovement.transform.localPosition.x < 0)
                StartCoroutine(cameraSwitch.SwitchView(buildLeftView));
            else if (characterMovement.transform.localPosition.x > 0)
                StartCoroutine(cameraSwitch.SwitchView(buildRightView));
            shipPartsToHide.SetActive(false);
        }
        else
            StartCoroutine(cameraSwitch.SwitchView(buildMastView));
    }

    public void Cancel()
    {
		if (!cameraSwitch.switching)
		{
			shipPartsToHide.SetActive(true);
			cameraSwitch.locked = false;
			fpsController.controllingShip = false;

			// boostSlider.gameObject.SetActive(false);


			menuSpawned = false;
			selectedCannon = null;
			Destroy(menu);
		}
	}

    public void OpenMastInteractionMenu()
    {
        mastInteractMenu.SetActive(true);
    }

    public void CloseMastInteractionMenu()
    {
        mastInteractMenu.SetActive(false);
    }

    public void OpenCannonInteractionMenu(GameObject cannonSlot)
    {
        cannonInteractMenu = Instantiate(cannonInteractMenuPrefab, cannonSlot.transform);
        cannonInteractMenu.GetComponent<RectTransform>().anchoredPosition = new Vector2(cannonInteractMenu.GetComponent<RectTransform>().anchoredPosition.x, 0.18f);
    }

    public void CloseCannonInteractionMenu(GameObject cannonSlot)
    {
        Destroy(cannonInteractMenu);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CannonSlot")
        {
            cannonSlot = other.gameObject;
            inRangeToBuild = true;

            OpenCannonInteractionMenu(cannonSlot);
        }

        if (other.tag == "MastInteraction")
        {
            cannonSlot = other.gameObject;

            OpenMastInteractionMenu();
            inShipMastRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "CannonSlot")
        {
            cannonSlot = null;
            inRangeToBuild = false;

            CloseCannonInteractionMenu(cannonSlot);
        }

        if (other.tag == "MastInteraction")
        {
            cannonSlot = null;
            inShipMastRange = false;

            CloseMastInteractionMenu();
        }
    }
}
