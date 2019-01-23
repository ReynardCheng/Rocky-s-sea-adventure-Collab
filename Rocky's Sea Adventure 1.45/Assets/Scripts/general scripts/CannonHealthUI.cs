using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CannonHealthUI : MonoBehaviour {

    public Slider healthBar;
    [SerializeField] private CannonController cannon;
    [SerializeField] private float cannonMaxHP;

	// Update is called once per frame
	void Update () {
        if (cannon != null)
        {
            healthBar.value = cannon.health / cannonMaxHP;
        }
        else
            healthBar.value = 0;
	}

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "Cannon" && cannon == null)
        {
            cannon = other.GetComponentInChildren<CannonController>();
            cannonMaxHP = cannon.health;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Cannon")
        {
            cannon = null;
            cannonMaxHP = float.MaxValue;
        }
    }
}
