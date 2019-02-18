using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCollectResources : MonoBehaviour
{

    public ResourceCount resourceCountScript;
    private AudioSource Audio;

    [Header("Sound")]
    [SerializeField] AudioClip ResourceCollectSound;

    // Use this for initialization
    void Start()
    {
        print(this);
        Audio = GetComponent<AudioSource>();
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
			resourceCountScript.SeaEssenceValue(0, 10);
            Destroy(other.gameObject);

            LevelManager.theLevelManager.PlaySoundEffect(LevelManager.theLevelManager.collectionClip);
            // Audio.clip = ResourceCollectSound;
            // Audio.Play();
        }
		if (other.tag == "Wooden Plank")
		{
			resourceCountScript.WoodenPlankValue(0, 10);
			Destroy(other.gameObject);

            LevelManager.theLevelManager.PlaySoundEffect(LevelManager.theLevelManager.collectionClip);
            // Audio.clip = ResourceCollectSound;
            // Audio.Play();
        }
		if (other.tag == "Metal Part")
		{
			resourceCountScript.MetalValue(0, 10);
			Destroy(other.gameObject);

            LevelManager.theLevelManager.PlaySoundEffect(LevelManager.theLevelManager.collectionClip);
            // Audio.clip = ResourceCollectSound;
            // Audio.Play();
        }
			
    }
}
