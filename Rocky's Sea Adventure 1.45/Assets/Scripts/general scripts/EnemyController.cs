using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public Transform ship; // The ship; enemy's target
    public float moveSpd = 2.0f; // Moving speed of enemy
    private Vector3 moveDirection;

    [Header("Detection Range")]
    public float aggroRange; // Enemy moves towards ship when within this range
    public float atkRange; // Enemy attacks ship when within this range

    private bool canMove;
    private Rigidbody rb;


    // Attack variables
    [Header("Attacking Variables")]
    public float atkCD; // Attack cooldown
    public float atkRate; // Attack rate
    private float nextAtk; // Time for next attack

    // Bullet properties
    public GameObject EnemyBullet; // Enemy's attacking bullet
    private Vector3 EnemyBulletPos; // Position of enemy bullet


    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Aggro detection range
        if (Vector3.Distance(ship.position, transform.position) <= aggroRange)
        {
            canMove = true;
            moveDirection = ship.transform.position - transform.position;
            moveDirection = moveDirection.normalized;
            transform.Translate(moveDirection * moveSpd, Space.World);
        }

        // Attack detection range
        if (Vector3.Distance(ship.position, transform.position) <= atkRange)
        {
            canMove = false;
            BasicAttack();
            print(gameObject.transform.position);
        }

        // Stops enemy movement
        if (canMove == false)
        {
            moveSpd = 0.0f;
        }
    }


    public void BasicAttack()
    {
        atkCD = 0.7f; // Attack cooldown time
        if (Time.time <= nextAtk)
            return;

        EnemyBulletPos = transform.position;
        Instantiate(EnemyBullet, EnemyBulletPos, Quaternion.identity);
        nextAtk = Time.time + atkRate;
    }
}