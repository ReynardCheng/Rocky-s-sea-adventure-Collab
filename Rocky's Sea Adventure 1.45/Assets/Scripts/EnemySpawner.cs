using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct waveComp
{
	public EnemyController[] enemies;
}

public class EnemySpawner : MonoBehaviour {

	public bool globalSpawner;
	public static int waveValue;
	public bool encountered;
	[SerializeField] float spawnRate;
	public waveComp[] numberOfWaves;
	

	// Use this for initialization
	void Start () {
		waveValue = 2;
		//spawnRate = 2;
	}
	
	// Update is called once per frame
	void Update () {

        if (globalSpawner)
        {
            spawnRate -= Time.deltaTime;

            CurrentWave();
        }
	}

    ///Global Spawning here
	void CurrentWave()
	{
		int currentWaveInt;

		if (spawnRate <= 0)
		{
			Vector3 enemySpawnPosition = new Vector3(transform.position.x, transform.position.y-4, transform.position.z);
			currentWaveInt = Random.Range(0, waveValue);
			print(currentWaveInt);
			foreach (EnemyController e in numberOfWaves[currentWaveInt].enemies)
			{
				GameObject spawnedEnemy = Instantiate(e, enemySpawnPosition, e.transform.rotation).gameObject;
				spawnedEnemy.GetComponent<Rigidbody>().AddForce(Random.Range(-300, 300), Random.Range(-300, 300), Random.Range(-300, 300));
                spawnedEnemy.GetComponent<EnemyController>().spawnTypes = (globalSpawner) ? EnemyController.spawnType.Global : EnemyController.spawnType.Local;
			}

			spawnRate = Random.Range(5, 8);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Ship" && !encountered)
		{
			waveValue++;
			encountered = true;


            //Local Spawning here
            Vector3 enemySpawnPosition = new Vector3(transform.position.x, transform.position.y - 4, transform.position.z);
            int currentWaveInt = Random.Range(0, waveValue);
            print(currentWaveInt);
            foreach (EnemyController e in numberOfWaves[currentWaveInt].enemies)
            {
                GameObject spawnedEnemy = Instantiate(e, enemySpawnPosition, e.transform.rotation).gameObject;
                spawnedEnemy.GetComponent<Rigidbody>().AddForce(Random.Range(-300, 300), Random.Range(-300, 300), Random.Range(-300, 300));
                spawnedEnemy.GetComponent<EnemyController>().spawnTypes = (globalSpawner) ? EnemyController.spawnType.Global : EnemyController.spawnType.Local;
                spawnedEnemy.GetComponent<EnemyController>().chaseShip = true;
            }
        }

        if(other.tag == "Ship")
        {

        }
	}



}
