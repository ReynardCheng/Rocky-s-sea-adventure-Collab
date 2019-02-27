using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpecialAttack : MonoBehaviour {


    public Transform bossMouth;
    public Vector3 direction; //Tis is where the hydro pump is aiming towards.
    public int damageToGive;
    
    public LayerMask layerMask;

    public GameObject waterSprayParticles;

    public BoatCombat1 boat;
    public BoxCollider boxCollider;
    public GameObject beamEnd;
    private Transform storedBeamEndTransform;
    private Boss boss;

    private bool inContactWithShip;
    private float damageTick;

    private void Start()
    {
        boat = FindObjectOfType<BoatCombat1>();
        boss = GetComponentInParent<Boss>();
        boxCollider = GetComponent<BoxCollider>();
        storedBeamEndTransform = beamEnd.transform;

        bossMouth = transform.parent;
    }

    public void FixedUpdate()
    {
        if (inContactWithShip)
        {
            damageTick++;
            if (damageTick % 4 == 0)
            {
                Vector3 directionOfRay = beamEnd.transform.position - bossMouth.position;

                RaycastHit hit;
                Physics.Raycast(bossMouth.position, directionOfRay, out hit, 1000f, layerMask);
                Debug.DrawRay(bossMouth.position, directionOfRay, Color.yellow);

                boat.TakeDamage(damageToGive, hit.point);

                Instantiate(waterSprayParticles, hit.point, waterSprayParticles.transform.rotation);
            }
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.tag == "Ship")
        {
            inContactWithShip = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ship")
        {
            inContactWithShip = false;
        }
    }
}
