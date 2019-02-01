using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour {

    [Header("Components")]
    [SerializeField] Rigidbody rb;

    //---------------------------------------------------
    [Space]
    [Header ("health variables")]
    [SerializeField] int health;
    [SerializeField] int maxHealth;

    //---------------------------------------------------
    [Space]
    [Header("Movement Variables")]
    [SerializeField] float moveSpeed;

    // for changing to a different move pattern
    [SerializeField] bool shipInRange;
    [SerializeField] float stuck;

    // to keep a safe distance
    [SerializeField] bool keepDistance;
    [SerializeField] float minDis, maxDis;
    [SerializeField] Transform playerPos; // move to the player / normal movement

    // movement pattern
    [SerializeField] float changeMovement; // acts as a timer such that once it reaches 0 the boss will change its move pattern
    public Transform[] movementPositions; // sets the positions that the boss will move to
    [SerializeField] int positionToMove; // sets the current position for the boss to move to, position is relative to array element
    [SerializeField] int positionsMoved; // counts how many times the enemy has used this different move pattern
                                     
    // to return to original move pattern
    [SerializeField] int movedLimit; // sets the limit of how many times the boss can move in this pattern before returning to original movement
    public int minChange, maxChange; // resets the timer on a random range 

    // move pattern when it is low on health
    public Transform nest;

    //---------------------------------------------------------------------------------------------------------------------------

    [Space]
    [Header("Attack Variables")]
    [SerializeField] float atkRate;
    [SerializeField] int spAtkCounter; // when it reaches zero this mob will use a different attack


    private void Start()
    {
        // for accessing components
        rb = GetComponent<Rigidbody>();

        //for setting variables
        moveSpeed = 10;
        movedLimit = 5;
        maxHealth = 100;
        health = maxHealth;
        changeMovement = maxChange;
        playerPos = GameObject.FindGameObjectWithTag("Ship").transform;
    }

    private void Update()
    {
        Movement();
    }

    //-----------------------------------------------------
    // movement related functions
    void Movement()
    {

        changeMovement -= Time.deltaTime;

        if (changeMovement > 0)
        {
            if (shipInRange)
            {
                // normal movement here
                Vector3 direction = playerPos.position - transform.position;

                float distance = Vector3.Distance(playerPos.position, transform.position);
                print(distance);

                if (distance < 5)
                {
                    keepDistance = true;
                }
                if (keepDistance)
                {
                    rb.velocity = direction.normalized * -moveSpeed / 2;
                }

                if (distance > 14)
                {
                    rb.velocity = direction.normalized * moveSpeed;
                    keepDistance = false;
                }
            }
        }

        if (changeMovement <= 0)
        {
            MovePatterns();
        }
    }

    void MovePatterns()
    {
        // input movePatterns here
        Vector3 patternDirection = movementPositions[positionToMove].position - transform.position;
        rb.velocity = patternDirection.normalized * moveSpeed;

        // if low on health move in this pattern
        if (health < 30)
        {
            // move to nest
        }

    }

    //-----------------------------------------------------
    // attack related functions
    void NormalAttack()
    {
        //instantiate something
        spAtkCounter--;
    }

    void SpecialAttack()
    {
        if (spAtkCounter <= 0)
        {
            //instantiateBeam
        }
    }

    //-----------------------------------------------------
    // health related functions
    void HealthManager(int DamageToTake)
    {
        health -= DamageToTake;

        if (health <= 0)
        {
            StartCoroutine(DeathAnimation());
        }
    }

    IEnumerator DeathAnimation()
    {
        yield return new WaitForSeconds(0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ship")
        {
            shipInRange = true;
        }

        if (other.tag == "BossSpots")
        {
            positionToMove = Random.Range(1, movementPositions.Length);
            positionsMoved++;

            if (positionsMoved >= movedLimit)
            {
                changeMovement = Random.Range(minChange, maxChange + 1);
                positionsMoved = 0;
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {

        if (other.tag == "BossSpots")
        {
            stuck += Time.deltaTime;

            if (stuck > 1f) positionToMove = Random.Range(1, movementPositions.Length);
        }
    }
    private void OnTriggerExit(Collider other)
    {

        if (other.tag == "BossSpots")
        {
            stuck = 0f;
        }
    }
}
