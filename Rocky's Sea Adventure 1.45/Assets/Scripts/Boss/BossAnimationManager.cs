using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimationManager : MonoBehaviour {

    private Animator animator;
    private Boss boss;
    private BoatController boat;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        boss = GetComponentInParent<Boss>();
        boat = FindObjectOfType<BoatController>();
    }

    public void NormalAttack()
    {
        boss.NormalAttack();
    }

    public void SpecialAttack()
    {
        boss.SpecialAttack();
    }

    public void ResetToIdling()
    {
        animator.Play("Boss_Idle");
    }
    
    public void BossDeath()
    {
        boss.stopMovement = true;
        boat.reachedEnd = true;
    }
}
