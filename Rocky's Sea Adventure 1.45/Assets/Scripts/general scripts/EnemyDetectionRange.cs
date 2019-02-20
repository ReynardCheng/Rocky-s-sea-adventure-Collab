using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetectionRange : MonoBehaviour {

	[SerializeField] List<EnemyController> enemies;

    [SerializeField] private bool enemiesChasing;

    [SerializeField] private float chaseRange;

    private GameObject ship;

	private void Start()
	{
        chaseRange = (GetComponent<BoxCollider>().size.x / 2) + 50;
	}

    private void Update()
    {
        if (enemiesChasing)
        {
            CheckShipInRange();
        }
    }

    void CheckShipInRange()
    {        
        if(Vector3.Distance(ship.transform.position, transform.position) > chaseRange)
        {
            enemiesChasing = false;

            foreach (EnemyController e in enemies)
            {
                e.GetComponent<EnemyController>().chaseShip = false;
                e.GetComponent<EnemyController>().returningHome = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Enemy")
		{
            if (other.GetComponent<EnemyController>().spawnTypes == EnemyController.spawnType.Local)
			    enemies.Add(other.gameObject.GetComponent<EnemyController>());
		}

		if (other.tag == "Ship")
		{
            print("ASIodojd");
			foreach (EnemyController e in enemies)
			{
				//e.GetComponent<EnemyController>().chaseShip = true;
                e.GetComponent<EnemyController>().spawnTypes = EnemyController.spawnType.Global;
            }
            ship = other.gameObject;
            enemiesChasing = true;
		}
	}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.tag == "Ship")
    //    {
    //        foreach (EnemyController e in enemies)
    //        {
    //            e.GetComponent<EnemyController>().chaseShip = false;
    //            e.GetComponent<Rigidbody>().velocity = Vector3.zero;
    //        }
    //    }
    //}
}
