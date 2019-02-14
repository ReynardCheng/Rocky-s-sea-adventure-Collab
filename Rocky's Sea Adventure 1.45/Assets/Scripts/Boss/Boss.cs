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
    [SerializeField] bool shipInAttackRange;
    [SerializeField] float stuck;

    // to keep a safe distance
    [SerializeField] bool keepDistance;
    [SerializeField] float minDis, maxDis;
    [SerializeField] Transform playerPos; // move to the player / normal movement

    // movement pattern
    [SerializeField] float changeMovement; // acts as a timer such that once it reaches 0 the boss will change its move pattern
    public List<Transform> movementPositions = new List<Transform>(); // sets the positions that the boss will move to
	private Transform[] storedPositions;
	public Transform actualPositionToMove;
	[SerializeField] int positionToMove; // sets the current position for the boss to move to, position is relative to array element
    [SerializeField] int positionsMoved; // counts how many times the enemy has used this different move pattern
	public Transform bossMesh;
	
    // to return to original move pattern
    [SerializeField] int movedLimit; // sets the limit of how many times the boss can move in this pattern before returning to original movement
    public int minChange, maxChange; // resets the timer on a random range 

    // move pattern when it is low on health
    public Transform nest;

    //---------------------------------------------------------------------------------------------------------------------------

    [Space]
    [Header("Attack Variables")]
    bool canAttack;
    [SerializeField] float atkRate;
    [SerializeField] int spAtkCounter; // when it reaches zero this mob will use a different attack
    [SerializeField] GameObject hydroPump;
    [SerializeField] GameObject attackLocation;
    

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

		storedPositions = new Transform[movementPositions.Count];
		movementPositions.CopyTo(storedPositions);

		positionToMove = GetRandomIntFromArray();
	}

    private void Update()
    {
        Movement();

        if (shipInAttackRange)
        {
            Attack();
        }
    }

    //-----------------------------------------------------
    // movement related functions
    void Movement()
    {
        changeMovement -= Time.deltaTime;

        if (changeMovement > 0)
        {
            if (shipInAttackRange)
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
				
				bossMesh.transform.rotation = Quaternion.LookRotation(direction);
			}
        }

        if (changeMovement <= 0)
        {
			MovePatterns();
        }

        atkRate -= Time.deltaTime;
    }

    void MovePatterns()
    {
        // input movePatterns here
        Vector3 patternDirection = movementPositions[positionToMove].position - transform.position;

		rb.velocity = patternDirection.normalized * moveSpeed * 5;

        // if low on health move in this pattern
        if (health < 30)
        {
            // move to nest
        }
    }

	private Vector3 GetPositionFromArray()
	{
		if (movementPositions.Count <= 0)
		{
			foreach (Transform t in storedPositions)
			{
				movementPositions.Add(t);
			}
		}

		int random = Random.Range(0, movementPositions.Count);
		Vector3 position = movementPositions[random].position;
		movementPositions.RemoveAt(random);

		return position;
	}

	private int GetRandomIntFromArray()
	{
		if (movementPositions.Count <= 0)
		{
			foreach (Transform t in storedPositions)
			{
				movementPositions.Add(t);
			}
			movementPositions.Remove(actualPositionToMove);
		}

		int random = Random.Range(0, movementPositions.Count);
		actualPositionToMove = movementPositions[random];

		return random;
	}

	//-----------------------------------------------------
	// attack related functions
	void Attack()
    {
        if (atkRate <= 0)
        {
            if (spAtkCounter > 0)
            {
                //Normal attack here//////
                BossBeam beam = Instantiate(hydroPump).GetComponent<BossBeam>();
                beam.bossMouth = attackLocation.transform;
                beam.shipLocation = playerPos.position;
                beam.direction = playerPos.position - attackLocation.transform.position;
                ////////////////////////
                atkRate = 7f;
                spAtkCounter--;
            }
            else if (spAtkCounter <= 0)
                SpecialAttack();
        }

    }


    void SpecialAttack()
    {

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
            shipInAttackRange = true;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "BossSpots")
        {
            movementPositions.Remove(actualPositionToMove);

            positionToMove = GetRandomIntFromArray();
            positionsMoved++;

            if (positionsMoved >= movedLimit)
            {
                changeMovement = Random.Range(minChange, maxChange + 1);
                positionsMoved = 0;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "BossSpots")
        {
            stuck = 0f;
        }

        if (other.tag == "Ship")
        {
            shipInAttackRange = false;
        }
    }
}
