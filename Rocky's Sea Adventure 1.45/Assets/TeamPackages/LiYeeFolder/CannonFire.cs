using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonFire : MonoBehaviour {


    /// *********
    /// Shooting
    /// *********
    [Header("Shooting")]
    public Rigidbody projectile; //reference for projectile

    public float atkCD; // Attack cooldown
    public float atkRate; // Attack rate
    private float nextAtk; // Time for next attack
    public Transform Gun; //reference for child of cannon -> the point where bullets fire from
    private Vector3 BulletPos; //position of where bullets fire from in vector3 form

    /// *********
    /// Enemies Detection
    /// *********
    [Header("Enemy Detection")]
    public int speed = 10;
    public Transform TargetingEnemy;
    public List<GameObject> EnemiesInRange;

    [Header("Health")]
    public int CannonHealth = 25;


    // Use this for initialization
    void Start () {
        EnemiesInRange = new List<GameObject>(); //EnemiesInRange = list of enemy targets in the range of the cannon
        //SphereCollider Range = gameObject.GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update () {

        GameObject target = null;
   
        var distance = Mathf.Infinity;
        Vector3 position = transform.position;

        foreach (GameObject enemy in EnemiesInRange) //For each enemy, find the closest one
        {
          
           var difference = (enemy.transform.position - position);
           var curDistance = difference.sqrMagnitude;
           if (curDistance < distance)
            {
                target = enemy; //setting the target to the closest one
                distance = curDistance;

                TargetingEnemy = target.transform;
            }
        }
      
        if (target != null)
        {
            Vector3 direction =  target.transform.position - transform.position; //finding the direction to nearest enemy
         
            float step = speed * Time.deltaTime;

            Vector3 newDir = Vector3.RotateTowards(transform.forward, direction, step, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDir); //rotate the cannon

        
            Shoot();
        }
    }

    void OnTriggerEnter(Collider other) //Add enemy to list of targets when in range of cannon
    {
        if (other.gameObject.tag == "Enemy")
        {
            EnemiesInRange.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other) //Remove enemy from target list when it leaves range
    {
        if (other.gameObject.tag == "Enemy")
        {
            EnemiesInRange.Remove(other.gameObject);
        }
    }

    private void Shoot()
    {
        atkCD = 0.7f; // Attack cooldown time
        if (Time.time <= nextAtk)
        return;

        BulletPos = Gun.transform.position;
        Instantiate(projectile, BulletPos, transform.rotation);
        projectile.GetComponent<BulletFire>().target = TargetingEnemy; //set the target/path for bullets to fly to in a straight line... will want to edit this later on as bullets act like a moving missile.
        nextAtk = Time.time + atkRate;
    }

}
