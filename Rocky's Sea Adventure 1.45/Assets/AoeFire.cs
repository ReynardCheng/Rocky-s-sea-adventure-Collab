using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoeFire : MonoBehaviour {

    [Header("Explode")]
    public float explodeDelay = 3f;
    public float countdown;
    public bool hasExploded = false;
    public float explodeRadius = 50f;
    public float explodeForce = 10f;
    public float damage = 30f;

    [Header("Speed and misc")]
    public float speed = 10;
    public Transform target;  //Target (set by CannonFire script)}
    public GameObject explosion; //explosion effect

	[Header ("Sound Related")]
	public AudioClip vfxToPlay;

    private void Start()
    {
        Destroy(gameObject, 5);
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

    void Explode()
    {
        //show effect
        Instantiate(explosion, transform.position, transform.rotation);



        Collider[] collidersToDamage = Physics.OverlapSphere(transform.position, explodeRadius);
        foreach (Collider nearbyObject in collidersToDamage)
        {
            /// damage
            EnemyController EnemyHealth = nearbyObject.GetComponent<EnemyController>();
            if (EnemyHealth != null)
            {
                EnemyHealth.Health(damage);
                print("boom");
            }
        }

        //get nearby objects
        //Collider[] colliders = Physics.OverlapSphere(transform.position, explodeRadius);
        //foreach (Collider nearbyObject in colliders)
        //{

        //    /// add force
        //    Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
        //    if (rb != null)
        //    {
        //        rb.AddExplosionForce(explodeForce, transform.position, explodeRadius);
        //    }
        //}
        //Remove projectile/bomb
        Destroy(gameObject);
    }

	void SoundSpawned()
	{
		GameObject spawnedSound = new GameObject();
		spawnedSound.transform.position = CharacterMovement.characterPos.position;
		spawnedSound.AddComponent<AudioSource>();
		spawnedSound.GetComponent<AudioSource>().clip = vfxToPlay;
		spawnedSound.GetComponent<AudioSource>().Play();
		Destroy(spawnedSound, spawnedSound.GetComponent<AudioSource>().clip.length);
	}

	void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            //Instantiate(explosion, transform.position, Quaternion.identity);
            //Destroy(gameObject); //destroy itself
            Explode();
			SoundSpawned();
            Destroy(gameObject);
            return;
        }

        //Boss boss = other.GetComponent<Boss>();
        //if (boss)
        //{
        //    boss.HealthManager(10);
        //    Destroy(gameObject);
        //}
    }

    private void OnCollisionEnter(Collision collision)
    {
        print(collision.gameObject);
        Boss boss = collision.gameObject.GetComponentInParent<Boss>();
        if (boss)
        {
            boss.HealthManager(10);
            Destroy(gameObject);
        }
    }




}
