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
        layerMask = LayerMask.NameToLayer("ShipFront");
        storedBeamEndTransform = beamEnd.transform;

        StartCoroutine(BeamCoroutine());
    }

    public void FixedUpdate()
    {
        if (inContactWithShip)
        {
            damageTick++;
            if (damageTick % 6 == 0)
            {
                Vector3 directionOfRay = bossMouth.position - beamEnd.transform.position;
                RaycastHit hit;
                Physics.Raycast(bossMouth.position, directionOfRay, out hit, 1000f, layerMask);

                beamEnd.transform.position = hit.point;

                boat.TakeDamage(damageToGive, hit.point);
                Instantiate(waterSprayParticles, hit.point, waterSprayParticles.transform.rotation);
            }

            beamEnd.transform.position = storedBeamEndTransform.position;
        }
    }

    public IEnumerator BeamCoroutine()
    {
        boss.stopMovement = true;
        yield return new WaitForSeconds(0.3f);  //estimated 0.3seconds before the beam reaches the edge.
        boxCollider.enabled = true;
        yield return new WaitForSeconds(3);
        boss.stopMovement = false;
        boxCollider.enabled = false;
        Destroy(gameObject);
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
