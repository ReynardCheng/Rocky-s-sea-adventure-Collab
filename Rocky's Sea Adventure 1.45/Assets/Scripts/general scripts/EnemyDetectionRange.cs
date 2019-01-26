using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetectionRange : MonoBehaviour {

	[SerializeField] List<EnemyController> enemies;

	private void Start()
	{
		//enemies = new List<EnemyController>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Enemy")
		{
			enemies.Add(other.gameObject.GetComponent<EnemyController>());
		}

		if (other.tag == "Ship")
		{
			foreach (EnemyController e in enemies)
			{
				e.GetComponent<EnemyController>().shipInRange = true;
			
			}
		}
	}
}
