using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerCheckpoint : MonoBehaviour {

    /// <summary>
    /// temporary script
    /// will prob want to cause enemies to instantiate not setactive
    /// </summary>

    [SerializeField] GameObject[] EnemiesToSpawn;

    private void OnTriggerEnter(Collider other)
    {

        foreach (GameObject enemy in EnemiesToSpawn)
        {
            enemy.SetActive(true);
        }

    }

}
