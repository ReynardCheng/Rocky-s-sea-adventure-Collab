using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum cannonTypes { normal, aoe, oilSlick, defence };

public class BuildCannonManager : MonoBehaviour
{
    public static bool menuSpawned;
    
    SpawnMenu theMenu;
    private ResourceCount resourceCount;
    private CharacterMovement characterMovement;

    [Header("reference to cannon")]
    public GameObject normalCannon;
    public GameObject aoeCannon;
    public GameObject oilSlickCannon;
    public GameObject defenceCannon;
    private GameObject selectedCannon;

    [Header("reference to menus")]
    public GameObject normalMenu;
    public GameObject upgradeMenu;
    public GameObject dismantleMenu;
    public GameObject buildProgressSlider;

    [SerializeField] GameObject menu;

    private bool inRangeToBuild;
    private GameObject cannonSlot;

    //[SerializeField]Button[] buttons;
    // Use this for initialization
    void Start()
    {
        characterMovement = GetComponent<CharacterMovement>();
        resourceCount = FindObjectOfType<ResourceCount>();
        menuSpawned = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !menuSpawned && inRangeToBuild)
        {
            OpenBuildMenu();
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
                    }
                    if (b.gameObject.name.Contains("Cancel"))
                    {
                        b.GetComponentInChildren<Button>().onClick.AddListener(CancelBuild);
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
                        b.GetComponentInChildren<Button>().onClick.AddListener(CancelBuild);
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
                        b.GetComponentInChildren<Button>().onClick.AddListener(CancelBuild);
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

    public void CancelBuild()
    {
        Destroy(menu);
        menuSpawned = false;
		selectedCannon = null;
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

        Destroy(canvas);
        Destroy(selectedCannon);

        GameObject cannon = Instantiate(cannonToBuild, menu.transform.parent.position, Quaternion.identity);

        cannon.transform.parent = menu.transform.parent;
        cannon.transform.rotation = menu.transform.parent.transform.rotation;
        menuSpawned = false;
        Destroy(menu.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CannonSlot")
        {
            cannonSlot = other.gameObject;
            inRangeToBuild = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "CannonSlot")
        {
            cannonSlot = null;
            inRangeToBuild = false;
        }
    }
}
