using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimationManager : MonoBehaviour {

    [System.Serializable]
    public struct BossSounds
    {
        public AudioClip normalAttack;
        public AudioClip specialAttack;
    }

    public BossSounds bossSounds;
    private Animator animator;
    private Boss boss;
    private BoatController boat;
    private AudioSource audioSource;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        boss = GetComponentInParent<Boss>();
        boat = FindObjectOfType<BoatController>();
        audioSource = GetComponent<AudioSource>();
    }

    public void NormalAttack()
    {
        boss.NormalAttack();
        audioSource.PlayOneShot(bossSounds.normalAttack);
    }

    public void PlaySpecialAttackSound()
    {
        audioSource.PlayOneShot(bossSounds.specialAttack);
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
        Destroy(boss.gameObject);
    }
}
