using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCollectResources : MonoBehaviour
{

    public ResourceCount resourceCountScript;
    private AudioSource Audio;

    [Header("Sound")]
    [SerializeField] AudioClip ResourceCollectSound;

	public GameObject woodParticle, metalParticle, SeaParticle; // particles that would spawn when resources are collected respectively

    // Use this for initialization
    void Start()
    {
        Audio = GetComponent<AudioSource>();
        resourceCountScript = FindObjectOfType<ResourceCount>();
    }

    // Update is called once per frame
    void Update()
    {
		if (resourceCountScript == null)
		{
			resourceCountScript = FindObjectOfType<ResourceCount>();
		}
    }

    public void OnTriggerEnter(Collider other)
    {

		if (other.tag == "Sea Essence")
		{
			resourceCountScript.SeaEssenceValue(0, 10);
            Destroy(other.gameObject);

			Instantiate(SeaParticle, other.transform.position, SeaParticle.transform.rotation);
			LevelManager.theLevelManager.PlaySoundEffect(LevelManager.theLevelManager.collectionClip);
            // Audio.clip = ResourceCollectSound;
            // Audio.Play();
        }
		if (other.tag == "Wooden Plank")
		{
			resourceCountScript.WoodenPlankValue(0, 10);
			Destroy(other.gameObject);
			Instantiate(woodParticle, other.transform.position, woodParticle.transform.rotation);
			LevelManager.theLevelManager.PlaySoundEffect(LevelManager.theLevelManager.collectionClip);
            // Audio.clip = ResourceCollectSound;
            // Audio.Play();
        }
		if (other.tag == "Metal Part")
		{
			resourceCountScript.MetalValue(0, 10);
			Destroy(other.gameObject);

			Instantiate(metalParticle, other.transform.position, metalParticle.transform.rotation);
			LevelManager.theLevelManager.PlaySoundEffect(LevelManager.theLevelManager.collectionClip);
            // Audio.clip = ResourceCollectSound;
            // Audio.Play();
        }
			
    }
}
