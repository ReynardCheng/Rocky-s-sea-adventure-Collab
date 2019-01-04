using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClickToUpgrade : MonoBehaviour
{

    /// **************
    /// Attach this to all upgradeable cannons to open up an radial menu for player to choose what to do with the cannon
    /// **************

    public Vector3 menuSpawnPosition;
    public static bool InCannonRange;
    public static bool menuOpen;
    public Camera theCamera;
    private GameObject thePlayer;

    [System.Serializable]
    public class Action
    {
        //What elements each action button will contain
        public Color color;
        public Sprite iconSprite;
        public string actionName;

    }

    /// **************
    /// List in the inspector
    /// What actions can the player take with this object : a list
    /// **************
    public Action[] options;

    void Start()
    {

        //float xPosition = this.transform.position.x;
        //float yPosition = this.transform.position.y;
        //menuSpawnPosition = new Vector3(xPosition, yPosition, 0f);

        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        theCamera = camera.GetComponent<Camera>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {

            InCannonRange = true;
            print("in range");
            thePlayer = other.gameObject;
        }
    }


    void Update()
    {

        if (Input.GetKeyDown("space") && InCannonRange == true && !menuOpen)
        {
            Vector3 menuSpawnPosition = theCamera.transform.position;

            UpgradeMenuSpawner.menu.SpawnMenu(this);
            menuOpen = true;
            print("Pressed space to open");

        }

    }

}
