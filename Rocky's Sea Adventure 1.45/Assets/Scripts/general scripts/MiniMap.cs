using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
	public Transform player;

	private bool revertFogState = false;

	[Header("Moving The Camera")]
	[SerializeField] CharacterMovement theCharacter;
	[SerializeField] float scrollSpeed;
	[SerializeField] float zoomAmount;
	public Transform miniMapCam;
	public float borderThickness;

	[Header("Raycast")]
	public Camera raycastCam;
	public LayerMask hitMask;

	private void Start()
	{
	
		theCharacter = FindObjectOfType<CharacterMovement>();
		borderThickness = 10f;
	}

	private void LateUpdate()
	{
		if (!theCharacter.mapOpened && !theCharacter.crRunning)
		{
			Vector3 newPosition = player.position;
			newPosition.y = transform.position.y;
			transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * 5);
			raycastCam.fieldOfView = 60f;
			zoomAmount = raycastCam.fieldOfView;
		}
		//transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);
	}

	// Update is called once per frame
	void Update()
	{
		if (theCharacter.mapOpened && !theCharacter.crRunning)
		{
			MoveMiniMap();
			handleZoom();
		}

		if (Input.GetMouseButtonDown(0) && theCharacter.mapOpened && !theCharacter.crRunning) ShootRayCast();
	}

	void MoveMiniMap()
	{
		if (Input.mousePosition.y >= Screen.height - borderThickness)
		{
			transform.Translate(Vector3.forward * scrollSpeed * Time.deltaTime, Space.World);
		}
		if (Input.mousePosition.y <= borderThickness)
		{
			transform.Translate(Vector3.back * scrollSpeed * Time.deltaTime, Space.World);
		}
		if (Input.mousePosition.x >= Screen.width -borderThickness)
		{
			transform.Translate(Vector3.right * scrollSpeed * Time.deltaTime, Space.World);
		}
		if (Input.mousePosition.x <= borderThickness)
		{
			transform.Translate(Vector3.left * scrollSpeed * Time.deltaTime, Space.World);
		}
	}

	void handleZoom()
	{

		if (zoomAmount > 20)
			if (Input.GetAxisRaw("Mouse ScrollWheel") > 0) zoomAmount -= 8;

		if (zoomAmount < 110)
			if (Input.GetAxisRaw("Mouse ScrollWheel") < 0) zoomAmount += 8;

		raycastCam.fieldOfView = Mathf.Lerp(raycastCam.fieldOfView, zoomAmount, Time.deltaTime * 3);
	}

	void ShootRayCast()
	{
		Ray ray = raycastCam.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;


		if (Physics.Raycast(ray, out hit, Mathf.Infinity, hitMask))
		{
			if (hit.collider != null)
			{
				print(hit.collider.gameObject.name);
			}
		}
	}

	private void OnPreRender()
	{
		revertFogState = RenderSettings.fog;
		RenderSettings.fog = false;
	}

	private void OnPostRender()
	{
		RenderSettings.fog = revertFogState;
	}

}
