using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletScript : MonoBehaviour
{
    public enum bulletType { normal, sticky, huge }
    public bulletType bulletState;

    [SerializeField] GameObject shipPos;
   // private ShipScript ship; // Bullet's target: whatever gameObject with ShipScript attached
	public float bulletSpd; // Moving speed of bullet
    public Vector3 moveDirection;
    private Rigidbody rb;
	public Transform target;

	[Header("Damage to give")]
	public int damageToGive;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
		shipPos = GameObject.FindGameObjectWithTag("Ship");
		// ship = GameObject.FindObjectOfType<ShipScript>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(moveDirection * bulletSpd, Space.World);
	//	transform.position = Vector3.MoveTowards(transform.position, target.position, 0);
    }

	void OnTriggerEnter(Collider other)
	{
		//if (other.tag == "Cannon")
		//{
		//	other.gameObject.GetComponentInChildren<CannonController>().damageCannon(damageToGive);
		//	print("HitCannon");
		//	Destroy(gameObject);
		//}

		if (other.tag == "Ship")
		{
			other.gameObject.GetComponent<BoatCombat1>().TakeDamage(5, gameObject);
			Destroy(gameObject);
		}
	}
}