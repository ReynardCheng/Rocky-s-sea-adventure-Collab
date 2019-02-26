using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnterTrigger : MonoBehaviour {

    private Boss boss;
    private LevelManager levelManager;

    private void Start()
    {
        boss = FindObjectOfType<Boss>();
        levelManager = FindObjectOfType<LevelManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        BoatController boat = other.GetComponent<BoatController>();
        if (boat)
        {
            levelManager.InitializeBossFight();
            boss.stopMovement = false;
            boss.chaseShip = true;
        }
    }

}
