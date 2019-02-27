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
    [SerializeField] int checkpointHealth;

    //---------------------------------------------------
    [Space]
    [Header("Movement Variables")]
    [SerializeField] float moveSpeed;
    [SerializeField] float turnSpeed;
    private float timeCount;

    // for changing to a different move pattern
    [SerializeField] bool shipInAttackRange;
    [SerializeField] float stuck;

    // to keep a safe distance
    [SerializeField] bool keepDistance;
    [SerializeField] float minDis, maxDis;
    [SerializeField] Transform playerPos; // move to the player / normal movement

    // movement pattern
    public bool chaseShip;
    //[SerializeField] float moveToShipCounter; // Timer for attacking player. Stops attacking once 0 and instead uses changeMovement timer. 
    [SerializeField] float changeMovement; // acts as a timer such that once it reaches 0 the boss will change its move pattern
    [SerializeField] bool reachedEscapePosition;
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
    BossSpecialAttack beam;
    public bool stopMovement;
    

    private void Start()
    {
        // for accessing components
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        beam = GetComponentInChildren<BossSpecialAttack>();

        //for setting variables
        movedLimit = movementPositions.Count;
        positionsMoved = 0;

        maxHealth = 600;
        health = maxHealth;
        checkpointHealth = maxHealth;

        changeMovement = maxChange;
        playerPos = FindObjectOfType<BoatController>().transform;   //Cheat method brb

		storedPositions = new Transform[movementPositions.Count];
		movementPositions.CopyTo(storedPositions);
        movementPositions.Clear();  //Clear to make boss chase the player first thing

		positionToMove = GetRandomIntFromArray();

        stopMovement = true;
        reachedEscapePosition = false;
        changeMovement = 5;
        //moveToShipCounter = 35;
        spAtkCounter = 5;
	}

    private void Update()
    {
        if (health <= 0)
        {
            animator.Play("Boss_Death");
			Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
			rb.velocity = Vector3.zero;
            return;
        }

        if (!stopMovement)
            Movement();

        if (stopMovement)
            rb.velocity = Vector3.zero;

        //Locks Z rotation
        Vector3 eulerAngles = transform.rotation.eulerAngles;
        eulerAngles = new Vector3(eulerAngles.x, eulerAngles.y, 0);
        transform.rotation = Quaternion.Euler(eulerAngles);
        ////////////
    }

    //-----------------------------------------------------
    // movement related functions
    void Movement()
    {
        if (!chaseShip)
        {
            if(changeMovement <= 0)
            {
                chaseShip = true;
                changeMovement = 20;
                reachedEscapePosition = false;
            }

            if (!reachedEscapePosition)
                MovePatterns();

            if (reachedEscapePosition)
            {
                changeMovement -= Time.deltaTime;

                Vector3 direction = playerPos.position - transform.position;

                Quaternion rotation = Quaternion.LookRotation(direction, transform.TransformDirection(Vector3.up));
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
            }
        }
        else if (chaseShip)
        {
            //Timers for behaviours/////////
            atkRate -= Time.deltaTime;

            Vector3 direction = playerPos.position - transform.position;

            Quaternion rotation = Quaternion.LookRotation(direction, transform.TransformDirection(Vector3.up));
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);

            float distance = Vector3.Distance(playerPos.position, transform.position);

            if (distance < 30)
            {
                rb.velocity = direction.normalized * -moveSpeed / 4;
            }
            else if (distance >= 65)
            {
                rb.velocity = direction.normalized * moveSpeed;
                keepDistance = false;
            }
            else if (distance >= 45) //sweetspot, try to stay in this range by not moving forwards or backwards
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

        Quaternion rotation = Quaternion.LookRotation(patternDirection, transform.TransformDirection(Vector3.up));
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);

        //directionToLook = patternDirection;
        //transform.rotation = Quaternion.LookRotation(directionToLook);

        rb.velocity = patternDirection.normalized * moveSpeed;

        //When boss reaches the desired position,
        if (Vector3.Distance(transform.position, movementPositions[positionToMove].position) <= 2f)
            ReachedDesiredPosition();
    }

    void ReachedDesiredPosition()
    {
        rb.velocity = Vector3.zero;

        reachedEscapePosition = true;
        movementPositions.Remove(actualPositionToMove);

        //positionsMoved++;

        changeMovement = 10;
        //changeMovement = Random.Range(minChange, maxChange + 1);

        //if (positionsMoved >= movedLimit)
        //{
        //    chaseShip = true;
        //    positionsMoved = 0;
        //}

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
                animator.Play("Boss_NormalAttack");
            }
            else if (spAtkCounter <= 0)
            {
                animator.Play("Boss_Entrance");
                stopMovement = true;
            }
        }
    }

    //Called through animation events.
    public void NormalAttack()
    {
        Vector3 direction = Vector3.Normalize(playerPos.position - attackLocation.transform.position);
        //Normal attack here//////
        GameObject projectile = Instantiate(normalAttack, attackLocation.transform.position, normalAttack.transform.rotation);
        projectile.GetComponent<Rigidbody>().velocity = direction * 60;
        projectile.transform.rotation = Quaternion.LookRotation(direction);
        ////////////////////////
        atkRate = 5f;
        spAtkCounter--;
    }
    
    public void SpecialAttack()
    {
        //beam.transform.rotation = Quaternion.LookRotation((playerPos.position - attackLocation.transform.position).normalized);
        stopMovement = false;
        atkRate = 5f;
        spAtkCounter = Random.Range(2, 4);
        //beam.direction = playerPos.position - attackLocation.transform.position;
    }


    //-----------------------------------------------------
    // health related functions
    public void HealthManager(int DamageToTake)
    {
        health -= DamageToTake;

        //How much damage the boss took since the boss's last cycle
        float damageReceivedCheck = checkpointHealth - health;

        if (damageReceivedCheck / maxHealth >= 0.3f)
        {
            checkpointHealth = health;;
            chaseShip = false;
        }
    }


    //IEnumerator DeathAnimation()
    //{
    //    yield return new WaitForSeconds(0f);
    //    Destroy(gameObject);
    //}
    

    private void OnTriggerStay(Collider other)
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
