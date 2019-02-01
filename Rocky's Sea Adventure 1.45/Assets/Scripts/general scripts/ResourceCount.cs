using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceCount : MonoBehaviour
{

    public int seaEssenceCount;
    public int woodCount;
    public int metalCount;

    public Text seaEssenceText;
    public Text woodText;
    public Text metalText;


	//this is for counting the resources


    // Use this for initialization
    void Start()
    {
        seaEssenceCount = 0;
        woodCount = 0;
        metalCount = 0;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            SeaEssenceValue(0,1);
            WoodenPlankValue(0,1);
            MetalValue(0,1);
        }
    }

    // Sea Essence --------------------------------------------------------------------------
    public void SeaEssenceValue(int resourceToDeduct, int resourceToAdd)
	{
		seaEssenceCount -= resourceToDeduct;
		seaEssenceCount += resourceToAdd;

		seaEssenceText.text = seaEssenceCount.ToString();

		if (seaEssenceCount <= 0)
		{
			seaEssenceCount = 0;
		}
	}

	// Wooden Planks --------------------------------------------------------------------------
	public void WoodenPlankValue(int resourceToDeduct, int resourceToAdd)
	{
		woodCount -= resourceToDeduct;
		woodCount += resourceToAdd;

        woodText.text = woodCount.ToString();

		if (woodCount <= 0)
		{
			woodCount = 0;
		}
	}

	// Metal count --------------------------------------------------------------------------
	public void MetalValue(int resourceToDeduct, int resourceToAdd)
	{
		metalCount -= resourceToDeduct;
		metalCount += resourceToAdd;

		metalText.text = metalCount.ToString();

		if (metalCount <= 0)
		{
			metalCount = 0;
		}
	}

	public void ResourceCountBuildCannon1()
    {
        woodCount -= 1;
        metalCount -= 1;
    }

    public void ResourceUpgradeCannonL2()
    {
        woodCount -= 2;
        metalCount -= 2;
    }
}