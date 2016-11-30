﻿using UnityEngine;
using System.Collections;

/**
 * Cette classe permet de créer un rayon partant de la caméra en direction de la position du curseur dans l'environnement 3D.
 * L'objet portant se script peut séléctionner des objets et se téléporter à leur position.
 * Trois curseurs sont implémentés : 
 * Un curseur cursorOff lorsqu'aucun objet selectionnable (tag "Target") n'est détecté par le rayon.
 * Un curseur cursorHover lorsqu'un objet séléctionnable est détécté mais non séléctionné
 * Un curseur cursorSelected lorsqu'un objet est selectionné
**/
public class Targetting: MonoBehaviour 
{

	public GameObject beginTarget;

	public GameObject lastTargettedObject;
	public GameObject targettedObject;	// Objet visé, null si aucun objet visé

	public int RAYCASTLENGTH = 10;	// Longueur du rayon issu de la caméra

	public CursorMode cursorMode = CursorMode.Auto;
	public Vector2 hotSpot = new Vector2(16, 16);	// Offset du centre du curseur
	public Texture2D cursorOff, cursorSelected, cursorHover;	// Textures à appliquer aux curseurs

	public float transitionTime = 10.0f;
	public bool transiting = false;

	void Start () 
	{	
		lastTargettedObject = beginTarget;
		targettedObject = beginTarget;

		transitionTime = transitionTime * Time.deltaTime;
		Transition (targettedObject);
		while (!transiting) {
			TransitionAnimation ();
		}

		Cursor.SetCursor (cursorOff, hotSpot, cursorMode);
		Cursor.visible = true;
	}

	void Update () 
	{
		// Le raycast attache un objet séléctionné
		RaycastHit hitInfo;
		Ray ray = GetComponentInChildren<Camera>().ScreenPointToRay(Input.mousePosition);
		Debug.DrawRay (ray.origin, ray.direction * RAYCASTLENGTH, Color.blue);
		bool rayCasted = Physics.Raycast (ray, out hitInfo, RAYCASTLENGTH);

		if (rayCasted) 
		{
			rayCasted = hitInfo.transform.CompareTag("Target");
		}
		// rayCasted est true si un objet possédant le tag draggable est détécté

		if (Input.GetMouseButtonDown (0)) 	// L'utilisateur click
		{
			
			if (rayCasted) 
			{
				Cursor.SetCursor (cursorSelected, hotSpot, cursorMode);

				lastTargettedObject = targettedObject;
				lastTargettedObject.GetComponentInChildren<Renderer> ().enabled = false;
				lastTargettedObject.GetComponentInChildren<Collider> ().isTrigger = false;

				targettedObject = hitInfo.transform.parent.gameObject;

				transiting = true;
				Transition (targettedObject);

				Debug.Log ("Object targetted");
			} 
			else 
			{
				Cursor.SetCursor (cursorOff, hotSpot, cursorMode);
			}
		} 

		else  // L'utilisateur bouge la sourie sans cliquer 
		{
			if (rayCasted) 
			{
				Cursor.SetCursor (cursorHover, hotSpot, cursorMode);
			} 
			else 
			{
				Cursor.SetCursor (cursorOff, hotSpot, cursorMode);
			}
		}
		if (targettedObject.GetComponent<AgentsParameters> ().isMoving) {
			GetComponent<Transform> ().position = targettedObject.GetComponent<AgentsParameters> ().anchor.position;
		}
	}

	void LateUpdate () {
		
		if (transiting) {
			TransitionAnimation ();
		}
	}

	void Transition (GameObject target) {

//		targettedObject.GetComponentInChildren<Rigidbody> ().isKinematic = true;
		targettedObject.GetComponentInChildren<Collider> ().isTrigger = true;

//		GetComponent<Transform> ().position = targettedObject.transform.position;
	
		AgentsParameters parameters = targettedObject.GetComponent<AgentsParameters> ();

//		GetComponent<Rigidbody> ().useGravity = parameters.isFlying ? false : true ;
//		GetComponent<Rigidbody> ().constraints = parameters.isMoving ? RigidbodyConstraints.None : RigidbodyConstraints.FreezePosition;

//		RenderSettings.fogDensity = parameters.visionRange;
//		GetComponentInChildren<Camera> ().fieldOfView = parameters.fov;
//		GetComponentInChildren<ColorCorrectionCurves> ().saturation = parameters.saturation;
//		GetComponentInChildren<MotionBlur> ().blurAmount = parameters.motionBlurStrength;

		RAYCASTLENGTH = parameters.interactionRange;

	}

	void TransitionAnimation () {
		Debug.Log ("Transition");

		AgentsParameters parameters = targettedObject.GetComponent<AgentsParameters> ();

		RenderSettings.fogDensity = Mathf.Lerp (RenderSettings.fogDensity, parameters.visionRange, transitionTime);
		GetComponentInChildren<Camera> ().fieldOfView = Mathf.Lerp (GetComponentInChildren<Camera> ().fieldOfView, parameters.fov, transitionTime);
		GetComponentInChildren<ColorCorrectionCurves> ().saturation = Mathf.Lerp (GetComponentInChildren<ColorCorrectionCurves> ().saturation, parameters.saturation, transitionTime);
		GetComponentInChildren<MotionBlur> ().blurAmount = Mathf.Lerp (GetComponentInChildren<MotionBlur> ().blurAmount, parameters.motionBlurStrength, transitionTime);
//		Debug.Log(Vector3.Lerp(transform.position,targettedObject.transform.position, transitionTime*Time.deltaTime));
		transform.position = Vector3.Lerp (transform.position, targettedObject.GetComponent<AgentsParameters> ().anchor.position, transitionTime);

//		GetComponent<Transform> ().position = Vector3.Lerp(targettedObject.transform.position,lastTargettedObject.transform.position, transitionTime);
		if (GetComponent<Transform> ().position == targettedObject.transform.position) {
			targettedObject.GetComponentInChildren<Renderer> ().enabled = false;
			Debug.Log ("Boo");
			transiting = false;
		}
	}
}