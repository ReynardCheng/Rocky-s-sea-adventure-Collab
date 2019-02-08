using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilSlickFire : MonoBehaviour
{

    public float speed = 10;
    public Transform target;  //Target (set by CannonFire script)}
    //public GameObject explosion;

    public GameObject oilSlickPrefab;

    private AudioSource source;
    public AudioClip OilSlickCollide;


    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target) //if enemy target exists
        {
            // Fly towards the target

            GetComponent<Rigidbody>().velocity = MathScript.ShootInArc(target, this.transform, 30f);
            transform.position = Vector3.MoveTowards(transform.position, target.position, 0.5f);

        }
        else
        {
            // Otherwise destroy self
            Destroy(gameObject);
        }

    }
    Vector3 ShootInArc(float angle)
    {
        Vector3 dir = target.position - transform.position;  // get target direction
        float h = dir.y;  // get height difference
        dir.y = 0;  // retain only the horizontal direction
        var dist = dir.magnitude;  // get horizontal distance
        var a = angle * Mathf.Deg2Rad;  // convert angle to radians
        dir.y = dist * Mathf.Tan(a);  // set dir to the elevation angle
        dist += h / Mathf.Tan(a);  // correct for small height differences
                                   // calculate the velocity magnitude
        var vel = Mathf.Sqrt(dist * Physics.gravity.magnitude / Mathf.Sin(2 * a));
        return vel * dir.normalized;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" && other.gameObject == target.gameObject)
        {
            other.GetComponent<EnemyController>().Health(10);
            //Instantiate(explosion, transform.position, Quaternion.identity);
            GameObject oilSlick = Instantiate(oilSlickPrefab, new Vector3(other.transform.position.x, other.transform.position.y - 2, other.transform.position.z), Quaternion.identity);
            AudioSource.PlayClipAtPoint(OilSlickCollide, other.transform.position);
            print("oil slick sound");
            Destroy(oilSlick, 4);
            Destroy(gameObject); //destroy itself
            return;
        }
    }
}