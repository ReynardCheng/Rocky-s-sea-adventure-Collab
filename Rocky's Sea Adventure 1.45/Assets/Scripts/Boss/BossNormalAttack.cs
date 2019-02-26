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
        BoatCombat1 ship = other.GetComponentInParent<BoatCombat1>();
        if (ship && canDamage)
        {
            canDamage = false;
            ship.TakeDamage(10, transform.position);
            Instantiate(splashParticles, transform.position, splashParticles.transform.rotation);
            Destroy(gameObject);
        }
    }

}
