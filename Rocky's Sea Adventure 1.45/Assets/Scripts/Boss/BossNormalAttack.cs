using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossNormalAttack : MonoBehaviour {

    [SerializeField] GameObject splashParticles;
    private bool canDamage = true;

    private void Start()
    {
        Destroy(gameObject, 3);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ship" && canDamage)
        {
            canDamage = false;   //This bool prevents bullets from damaging the ship twice.

            other.gameObject.GetComponentInParent<BoatCombat1>().TakeDamage(25, gameObject);
            Instantiate(splashParticles, transform.position, splashParticles.transform.rotation);

            Destroy(gameObject);
        }
        //BoatCombat1 ship = other.GetComponentInParent<BoatCombat1>();
        //if (ship && canDamage)
        //{
        //    canDamage = false;
        //    ship.TakeDamage(10, transform.position);
        //    Instantiate(splashParticles, transform.position, splashParticles.transform.rotation);
        //    Destroy(gameObject);
        //}
    }

}
