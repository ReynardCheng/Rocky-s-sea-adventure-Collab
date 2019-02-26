using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour {

    [Header("Components")]
    [SerializeField] Rigidbody rb;
    private Animator animator;
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
    [SerializeField] bool chaseShip;
    [SerializeField] float moveToShipCounter; // Timer for attacking player. Stops attacking once 0 and instead uses changeMovement timer. 
    [SerializeField] float changeMovement; // acts as a timer such that once it reaches 0 the boss will change its move pattern
    public List<Transform> movementPositions = new List<Transform>(); // sets the positions that the boss will move to
	private Transform[] storedPositions;
	public Transform actualPositionToMove;
	[SerializeField] int positionToMove; // sets the current position for the boss to move to, position is relative to array element
    [SerializeField] int positionsMoved; // counts how many times the enemy has used this different move pattern
	
    // to return to original move pattern
    [SerializeField] int movedLimit; // sets the limit of how many times the boss can move in this pattern before returning to original movement
    public int minChange, maxChange; // resets the timer on a random range 

    // move pattern when it is low on health
    public Transform nest;

    private Vector3 directionToLook;

    //---------------------------------------------------------------------------------------------------------------------------

    [Space]
    [Header("Attack Variables")]
    bool canAttack;
    [SerializeField] float atkRate;
    [SerializeField] int spAtkCounter; // when it reaches zero this mob will use a different attack
    [SerializeField] GameObject normalAttack;
    [SerializeField] GameObject specialAttack;
    [SerializeField] GameObject attackLocation;
    public bool stopMovement;
    

    private void Start()
    {
        // for accessing components
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();

        //for setting variables
        movedLimit = movementPositions.Count;
        positionsMoved = 0;

        maxHealth = 100;
        health = maxHealth;
        changeMovement = maxChange;
        playerPos = FindObjectOfType<BoatController>().transform;   //Cheat method brb

		storedPositions = new Transform[movementPositions.Count];
		movementPositions.CopyTo(storedPositions);

		positionToMove = GetRandomIntFromArray();

        changeMovement = 5;
        moveToShipCounter = 35;
        spAtkCounter = 5;
	}

    private void Update()
    {
        if(!stopMovement)
            Movement();

        if (stopMovement)
            rb.velocity = Vector3.zero;

        //if (shipInAttackRange)
        //{
        //    Attack();
        //}
    }

    //-----------------------------------------------------
    // movement related functions
    void Movement()
    {
        changeMovement -= Time.deltaTime;

        if (!chaseShip)
        {
            if (changeMovement >= 0)
            {
                transform.rotation = Quaternion.LookRotation(playerPos.position - transform.position);
            }

            if (changeMovement < 0)
            {
                MovePatterns();
            }
        }
        else if (chaseShip)
        {
            //Timers for behaviours/////////
            atkRate -= Time.deltaTime;
            moveToShipCounter -= Time.deltaTime;

            if (moveToShipCounter <= 0)
            {
                movementPositions.Clear();
                foreach (Transform t in storedPositions)
                {
                    movementPositions.Add(t);
                }

                chaseShip = false;
                moveToShipCounter = 35;
            }
            //////////////////////////////
            
            Vector3 direction = playerPos.position - transform.position;
            transform.rotation = Quaternion.LookRotation(playerPos.position - transform.position);

            float distance = Vector3.Distance(playerPos.position, transform.position);

            if (distance < 50)
            {
                rb.velocity = direction.normalized * -moveSpeed / 2;
            }
            else if (distance >= 80)
            {
                rb.velocity = direction.normalized * moveSpeed;
                keepDistance = false;
            }
            else if (distance >= 65) //sweetspot, try to stay in this range by not moving forwards or backwards
            {
                rb.velocity = Vector3.zero;
            }

            if (shipInAttackRange)
            {
                Attack();
            }
        }

    }

    void MovePatterns()
    {
        // input movePatterns here
        Vector3 patternDirection = movementPositions[positionToMove].position - transform.position;

        directionToLook = patternDirection;
        transform.rotation = Quaternion.LookRotation(directionToLook);

        rb.velocity = patternDirection.normalized * moveSpeed;

        //When boss reaches the desired position,
        if (Vector3.Distance(transform.position, movementPositions[positionToMove].position) <= 2f)
            ReachedDesiredPosition();
    }

    void ReachedDesiredPosition()
    {

        rb.velocity = Vector3.zero;

        movementPositions.Remove(actualPositionToMove);

        positionsMoved++;

        changeMovement = Random.Range(minChange, maxChange + 1);

        if (positionsMoved >= movedLimit)
        {
            chaseShip = true;
            positionsMoved = 0;
        }

        positionToMove = GetRandomIntFromArray();
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
                Vector3 direction = Vector3.Normalize(playerPos.position - attackLocation.transform.position);
                //Normal attack here//////
                GameObject projectile = Instantiate(normalAttack, attackLocation.transform.position, normalAttack.transform.rotation);
                projectile.GetComponent<Rigidbody>().velocity = direction * 60;
                projectile.transform.rotation = Quaternion.LookRotation(direction);
                ////////////////////////
                atkRate = 2f;
                spAtkCounter--;
            }
            else if (spAtkCounter <= 0)
            {
                atkRate = 15f;
                SpecialAttack();
            }
        }
    }


    void SpecialAttack()
    {
        BossSpecialAttack beam = Instantiate(specialAttack, attackLocation.transform.position, specialAttack.transform.rotation).GetComponent<BossSpecialAttack>();
        beam.bossMouth = attackLocation.transform;
        beam.transform.parent = attackLocation.transform;
        beam.transform.rotation = Quaternion.LookRotation((playerPos.position - attackLocation.transform.position).normalized);

        spAtkCounter = 5;
        //beam.direction = playerPos.position - attackLocation.transform.position;
    }


    //-----------------------------------------------------
    // health related functions
    public void HealthManager(int DamageToTake)
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
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ship")
        {
            shipInAttackRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ship")
        {
            shipInAttackRange = false;
        }
    }
}
