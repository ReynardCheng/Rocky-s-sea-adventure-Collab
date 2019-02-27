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
	[SerializeField] float spawnRate;
	public waveComp[] numberOfWaves;
	int currentWaveInt;
	[SerializeField] int i; // for the spawning of the enemies
	public float spawnTimer; // time between each spawning

	public int maxEnemies = 12;
	public static int totalSpawnedEnemies;
	public static int currentEnemies;
	List<GameObject> enemies;

	// distance from ship to spawn enemies
	public float distToStartSpawn;
	[SerializeField] float distanceFromBoat;
	BoatController theBoat;

	// Use this for initialization
	void Start () {
		enemies = new List<GameObject>();
		theBoat = FindObjectOfType<BoatController>();
		InvokeRepeating("DistanceFromBoat",0, 1);
	}
	
	// Update is called once per frame
	void Update () {

        if (globalSpawner)
        {
			if (distanceFromBoat <= distToStartSpawn)
			{
				spawnRate -= Time.deltaTime;

				for (int i = 0; i < enemies.Count; i++)
				{
					if (enemies[i] == null) enemies.Remove(enemies[i]);
				}
				if (totalSpawnedEnemies < maxEnemies)
					CurrentWave();
			}
        }
		
	}

	void DistanceFromBoat()
	{
		distanceFromBoat = Vector3.Distance(theBoat.transform.position, this.transform.position);
		print(distanceFromBoat);
	}

    ///Global Spawning here
	void CurrentWave()
	{
		if (spawnRate <= 0)
		{
			currentWaveInt = Random.Range(0, numberOfWaves.Length);
			ProcedualSpawning();
		}
	}

	void ProcedualSpawning()
	{
		spawnTimer -= Time.deltaTime;
		if (spawnTimer <= 0)
		{
			Vector3 enemySpawnPosition = new Vector3(transform.position.x, transform.position.y - 4, transform.position.z);
			GameObject spawnedEnemy = Instantiate(numberOfWaves[currentWaveInt].enemies[i], enemySpawnPosition, numberOfWaves[currentWaveInt].enemies[i].transform.rotation).gameObject;
			enemies.Add(spawnedEnemy);
			totalSpawnedEnemies++;
			i++;
			spawnTimer = 3;
		}
		if (i >= numberOfWaves[currentWaveInt].enemies.Length)
		{
			i = 0;
			spawnRate = Random.Range(15, 30);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Ship")
		{
	
            ////Local Spawning here
            //Vector3 enemySpawnPosition = new Vector3(transform.position.x, transform.position.y - 4, transform.position.z);
            //int currentWaveInt = Random.Range(0, waveValue);
            //print(currentWaveInt);
            //foreach (EnemyController e in numberOfWaves[currentWaveInt].enemies)
            //{
            //    GameObject spawnedEnemy = Instantiate(e, enemySpawnPosition, e.transform.rotation).gameObject;
            //    spawnedEnemy.GetComponent<Rigidbody>().AddForce(Random.Range(-300, 300), Random.Range(-300, 300), Random.Range(-300, 300));
            //    spawnedEnemy.GetComponent<EnemyController>().spawnTypes = (globalSpawner) ? EnemyController.spawnType.Global : EnemyController.spawnType.Local;
            //    spawnedEnemy.GetComponent<EnemyController>().chaseShip = true;
            //}
        }

        if(other.tag == "Ship")
        {

        }
	}

	// -----------------------------------
	// for spawning, placed here so that it would not be so messy at the top

	//Vector3 enemySpawnPosition = new Vector3(transform.position.x, transform.position.y-4, transform.position.z);

	//print(currentWaveInt);


	//foreach (EnemyController e in numberOfWaves[currentWaveInt].enemies)
	//{
	//	GameObject spawnedEnemy = Instantiate(e, enemySpawnPosition, e.transform.rotation).gameObject;
	//	enemies.Add(spawnedEnemy);
	//	//spawnedEnemy.transform.Translate(new Vector3(Random.Range(-300, 300), Random.Range(-300, 300), Random.Range(-300, 300)));
	//	//spawnedEnemy.GetComponent<Rigidbody>().AddForce(Random.Range(-300, 300), Random.Range(-300, 300), Random.Range(-300, 300));
	//             spawnedEnemy.GetComponent<EnemyController>().spawnTypes = (globalSpawner) ? EnemyController.spawnType.Global : EnemyController.spawnType.Local;
	//	totalSpawnedEnemies++;
	//	print(totalSpawnedEnemies);
	//}

	//spawnRate = Random.Range(15, 30);

}
