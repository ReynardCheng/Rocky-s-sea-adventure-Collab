using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnterTrigger : MonoBehaviour {

    private Boss boss;
    private LevelManager levelManager;

	// for boss animation
	public Camera cutSceneCamera;
	public string animToPlay;
	CharacterMovement theCharacter;
	public GameObject rockBlock;

    private void Start()
    {
        boss = FindObjectOfType<Boss>();
		theCharacter = FindObjectOfType<CharacterMovement>();
        levelManager = FindObjectOfType<LevelManager>();
    }

	public void PlayBossAnimation()
	{
		boss.GetComponentInChildren<Animator>().Play(animToPlay);
		
	}
	public void BossMove()
	{
		Animator bossAnim = boss.GetComponentInChildren<Animator>();
		boss.stopMovement = false;
		boss.chaseShip = true;
		//cutSceneCamera.enabled = false;
	}
	public void OffThisCam()
	{
		theCharacter.canMove = true;
		cutSceneCamera.gameObject.SetActive(false);
		gameObject.GetComponentInParent<BossEnterTrigger>().gameObject.SetActive(false);
	}

    private void OnTriggerEnter(Collider other)
    {
		rockBlock.SetActive(true);
		EnemySpawner.totalSpawnedEnemies = 0;
		levelManager.InitializeBossFight();
		cutSceneCamera.gameObject.SetActive(true);
		theCharacter.canMove = false;

		//BoatController boat = other.GetComponent<BoatController>();
		//if (boat)
		//{
		//    levelManager.InitializeBossFight();
		//    boss.stopMovement = false;
		//    boss.chaseShip = true;
		//}
	}

}
