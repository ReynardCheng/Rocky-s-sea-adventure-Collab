using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBeam : MonoBehaviour {


    public Transform bossMouth; //This is where the 1s position of the LineRenderer is.
    public Vector3 direction; //Tis is where the hydro pump is aiming towards.
    public int damageToGive;

    private LineRenderer beam;
    private LayerMask layerMask;

    public GameObject waterSprayParticles;

    public Vector3 shipLocation;

    public bool attacking;
    private bool attackStarted;

    private void Start()
    {
        beam = GetComponent<LineRenderer>();
        layerMask.value = LayerMask.NameToLayer("Ship");
        StartCoroutine(BeamCoroutine());
    }

    public void Update()
    {
        if (attacking)
        {
            if (!attackStarted)
            {
                beam.SetPosition(0, bossMouth.position);
                beam.SetPosition(1, shipLocation);
            }
            RaycastHit hit;
            Debug.DrawRay(bossMouth.position, direction * 1000f, Color.yellow);
            if (Physics.Raycast(bossMouth.position, direction, out hit, 1000f))
            {
                BoatCombat1 ship = hit.collider.gameObject.GetComponentInParent<BoatCombat1>();
                if (ship)
                {
                    Instantiate(waterSprayParticles, hit.point, waterSprayParticles.transform.rotation);
                    ship.TakeDamage(1, hit.point);
                }
            }
        }
    }

    public IEnumerator BeamCoroutine()
    {
        attacking = true;
        yield return new WaitForSeconds(3);
        attacking = false;
        Destroy(gameObject);
    }
}
