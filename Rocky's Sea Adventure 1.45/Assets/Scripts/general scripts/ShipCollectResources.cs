using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCollectResources : MonoBehaviour
{

    public ResourceCount resourceCountScript;

    // Use this for initialization
    void Start()
    {
        resourceCountScript = FindObjectOfType<ResourceCount>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter(Collider other)
    {

		if (other.tag == "Sea Essence")
		{
			resourceCountScript.SeaEssenceValue(0, 1);
			Destroy(other.gameObject);
		}
		if (other.tag == "Wooden Plank")
		{
			resourceCountScript.WoodenPlankValue(0, 1);
			Destroy(other.gameObject);
		}
		if (other.tag == "Metal Part")
		{
			resourceCountScript.MetalValue(0, 1);
			Destroy(other.gameObject);
		}
			
    }
}
