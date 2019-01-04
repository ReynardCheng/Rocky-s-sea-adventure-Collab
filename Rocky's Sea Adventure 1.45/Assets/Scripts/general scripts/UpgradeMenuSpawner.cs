using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeMenuSpawner : MonoBehaviour {

    public static UpgradeMenuSpawner menu;
    public RadialMenu menuPrefab;

    void Awake()
    {
        menu = this;
    }

    public void SpawnMenu(ClickToUpgrade obj)
    {
        //Instantiate radial menu prefab
        RadialMenu newMenu = Instantiate(menuPrefab) as RadialMenu;
        newMenu.transform.SetParent(transform, false);
        newMenu.transform.position = Input.mousePosition; //spawn the menu where the mouse is
        newMenu.SpawnButtons(obj); //spawn buttons
    }
}
