using UnityEngine;
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
	private Transform myTransform;
	public GameObject lastTargettedObject;
	public GameObject targettedObject;	// Objet visé, null si aucun objet visé

	private int RAYCASTLENGTH = 100;	// Longueur du rayon issu de la caméra

	private CursorMode cursorMode = CursorMode.Auto;
	private Vector2 hotSpot = new Vector2(16, 16);	// Offset du centre du curseur
	public Texture2D cursorOff, cursorSelected, cursorHover;	// Textures à appliquer aux curseurs

	public bool selected = false;
	public bool accepted = false;

	public int chargingDist = 10;
	public int chargingCounter = 0;
	public float transitionTime = 0.2f;
	public float chargingTime = 0.05f;

	void Start () 
	{	
		myTransform = transform.parent.parent;
		lastTargettedObject = transform.GetComponent<MenuManager>().startAgent;
		targettedObject = transform.GetComponent<MenuManager>().startAgent;

		Cursor.SetCursor (cursorOff, hotSpot, cursorMode);
		Cursor.visible = true;
	}

	void Update () 
	{
		// Le raycast attache un objet séléctionné
		RaycastHit hitInfo;
		Ray ray = GetComponentInChildren<Camera>().ScreenPointToRay(Input.mousePosition);
		//Ray ray = GetComponentInChildren<Camera>().ScreenPointToRay(transform.forward);
		Debug.DrawRay (ray.origin, ray.direction * RAYCASTLENGTH, Color.blue);
		bool rayCasted = Physics.Raycast (ray, out hitInfo, RAYCASTLENGTH);

		if (rayCasted) 
		{
			rayCasted = hitInfo.transform.CompareTag("Target");
		}
		// rayCasted est true si un objet possédant le tag draggable est détécté

		if (Input.GetMouseButton (0)) { 	// L'utilisateur reste appuyé sur le click

			if (rayCasted) {
				Cursor.SetCursor (cursorHover, hotSpot, cursorMode);

				selected = true;

				targettedObject = hitInfo.transform.parent.gameObject;

				Debug.Log ("Object targetted");

			} 
			else {
				Cursor.SetCursor (cursorOff, hotSpot, cursorMode);
			}
			if (selected && Input.GetKeyDown (KeyCode.Space)) {

				accepted = true;
			}


		}
		else  // L'utilisateur bouge la souris sans cliquer 
		{
			if (rayCasted) 
			{
				Cursor.SetCursor (cursorHover, hotSpot, cursorMode);
			} 
			else 
			{
				Cursor.SetCursor (cursorOff, hotSpot, cursorMode);
			}
			selected = false;
			//targettedObject = null;
			//chargingCounter = 0;
		}

		if (lastTargettedObject) {
			if (lastTargettedObject.GetComponent<AgentsParameters> ().isMoving && chargingCounter == 0) {
				myTransform.position = targettedObject.GetComponent<AgentsParameters> ().anchor.position;
			}
		}
	}

	void LateUpdate () {
		
		if (selected) {
			ChargingTransition ();
		}
		else if (accepted) {
			AcceptedTransition ();
		} 
		else {
			if (transform.position != lastTargettedObject.GetComponent<AgentsParameters> ().anchor.position) {
				myTransform.position = Vector3.Lerp (transform.position, lastTargettedObject.GetComponent<AgentsParameters> ().anchor.position, chargingTime);

				AgentsParameters parameters = lastTargettedObject.GetComponent<AgentsParameters> ();

				GetComponentInChildren<Camera> ().fieldOfView = Mathf.Lerp (GetComponentInChildren<Camera> ().fieldOfView, parameters.fov, chargingTime);
				GetComponentInChildren<ColorCorrectionCurves> ().saturation = Mathf.Lerp (GetComponentInChildren<ColorCorrectionCurves> ().saturation, parameters.saturation, chargingTime);
				GetComponentInChildren<BlurOptimized> ().blurSize = Mathf.Lerp (GetComponentInChildren<BlurOptimized> ().blurSize, parameters.blurSize, chargingTime);
				GetComponentInChildren<DepthOfField> ().focalSize = Mathf.Lerp (GetComponentInChildren<DepthOfField> ().focalSize, parameters.focalSize, chargingTime);

				chargingCounter = 0;
			}
		}
	}

	public void ChargingTransition () {

		if (chargingCounter >= chargingDist)
			return;
		myTransform.position = Vector3.Lerp (transform.position, targettedObject.GetComponent<AgentsParameters> ().anchor.position, chargingTime);

		AgentsParameters parameters = targettedObject.GetComponent<AgentsParameters> ();

		GetComponentInChildren<Camera> ().fieldOfView = Mathf.Lerp (GetComponentInChildren<Camera> ().fieldOfView, parameters.fov, chargingTime);
		GetComponentInChildren<ColorCorrectionCurves> ().saturation = Mathf.Lerp (GetComponentInChildren<ColorCorrectionCurves> ().saturation, parameters.saturation, chargingTime);
		GetComponentInChildren<BlurOptimized> ().blurSize = Mathf.Lerp (GetComponentInChildren<BlurOptimized> ().blurSize, parameters.blurSize, chargingTime);
		GetComponentInChildren<DepthOfField> ().focalSize = Mathf.Lerp (GetComponentInChildren<DepthOfField> ().focalSize, parameters.focalSize, chargingTime);

		chargingCounter ++;

	}

	public void AcceptedTransition () {

		myTransform.position = Vector3.Lerp (myTransform.position, targettedObject.GetComponent<AgentsParameters> ().anchor.position, transitionTime);

		AgentsParameters parameters = targettedObject.GetComponent<AgentsParameters> ();

		GetComponentInChildren<Camera> ().fieldOfView = Mathf.Lerp (GetComponentInChildren<Camera> ().fieldOfView, parameters.fov, transitionTime);
		GetComponentInChildren<ColorCorrectionCurves> ().saturation = Mathf.Lerp (GetComponentInChildren<ColorCorrectionCurves> ().saturation, parameters.saturation, transitionTime);
		GetComponentInChildren<BlurOptimized> ().blurSize = Mathf.Lerp (GetComponentInChildren<BlurOptimized> ().blurSize, parameters.blurSize, transitionTime);
		GetComponentInChildren<DepthOfField> ().focalSize = Mathf.Lerp (GetComponentInChildren<DepthOfField> ().focalSize, parameters.focalSize, transitionTime);

		if (myTransform.position == targettedObject.GetComponent<AgentsParameters> ().anchor.position) {
			chargingCounter = 0;
			accepted = false;
			selected = false;
			lastTargettedObject = targettedObject;
		}
	}

	public void Transition (GameObject target) {

		lastTargettedObject = targettedObject;
		targettedObject = target;

		lastTargettedObject.GetComponentInChildren<Renderer> ().enabled = true;
		lastTargettedObject.GetComponentInChildren<Collider> ().isTrigger = false;
		lastTargettedObject.GetComponentInChildren<Rigidbody> ().isKinematic = true;

		targettedObject.GetComponentInChildren<Renderer> ().enabled = false;
		targettedObject.GetComponentInChildren<Rigidbody> ().isKinematic = true;
		targettedObject.GetComponentInChildren<Collider> ().isTrigger = true;
	
		AgentsParameters parameters = targettedObject.GetComponent<AgentsParameters> ();

		GetComponent<Rigidbody> ().useGravity = parameters.isFlying ? false : true ;
		GetComponent<Rigidbody> ().constraints = parameters.isMoving ? RigidbodyConstraints.FreezeRotation : RigidbodyConstraints.FreezeAll;

	}

	/*void TransitionValidationAnimation () {
		Debug.Log ("TransitionValidation");

		AgentsParameters parameters = targettedObject.GetComponent<AgentsParameters> ();

		RAYCASTLENGTH = parameters.interactionRange;
		//		RenderSettings.fogDensity = Mathf.Lerp (RenderSettings.fogDensity, parameters.visionRange, transitionTime);
		GetComponentInChildren<Camera> ().fieldOfView = Mathf.Lerp (GetComponentInChildren<Camera> ().fieldOfView, parameters.fov, transitionTimeValidating);
		GetComponentInChildren<ColorCorrectionCurves> ().saturation = Mathf.Lerp (GetComponentInChildren<ColorCorrectionCurves> ().saturation, parameters.saturation, transitionTimeValidating);
		GetComponentInChildren<MotionBlur> ().blurAmount = Mathf.Lerp (GetComponentInChildren<MotionBlur> ().blurAmount, parameters.motionBlurStrength, transitionTimeValidating);
		GetComponentInChildren<BlurOptimized> ().blurSize = Mathf.Lerp (GetComponentInChildren<BlurOptimized> ().blurSize, parameters.blurSize, transitionTimeValidating);
		GetComponentInChildren<DepthOfField> ().focalSize = Mathf.Lerp (GetComponentInChildren<DepthOfField> ().focalSize, parameters.focalSize, transitionTimeValidating);

		transform.position = Vector3.Lerp (transform.position, targettedObject.GetComponent<AgentsParameters> ().anchor.position, transitionTimeValidating);
		//		transform.rotation.SetEulerAngles (Vector3.Lerp (transform.rotation.eulerAngles, targettedObject.GetComponent<AgentsParameters> ().anchor.rotation.eulerAngles, transitionTime));

		if (GetComponent<Transform> ().position == targettedObject.transform.position) {
			transiting = false;
			transitionValidate = false;
		}
	}*/

	void TransitionAnimation () {
		Debug.Log ("Transition");

		AgentsParameters parameters = targettedObject.GetComponent<AgentsParameters> ();

		RAYCASTLENGTH = parameters.interactionRange;

		GetComponentInChildren<Camera> ().fieldOfView = Mathf.Lerp (GetComponentInChildren<Camera> ().fieldOfView, parameters.fov, transitionTime);
		GetComponentInChildren<ColorCorrectionCurves> ().saturation = Mathf.Lerp (GetComponentInChildren<ColorCorrectionCurves> ().saturation, parameters.saturation, transitionTime);
		GetComponentInChildren<MotionBlur> ().blurAmount = Mathf.Lerp (GetComponentInChildren<MotionBlur> ().blurAmount, parameters.motionBlurStrength, transitionTime);
		GetComponentInChildren<BlurOptimized> ().blurSize = Mathf.Lerp (GetComponentInChildren<BlurOptimized> ().blurSize, parameters.blurSize, transitionTime);
		GetComponentInChildren<DepthOfField> ().focalSize = Mathf.Lerp (GetComponentInChildren<DepthOfField> ().focalSize, parameters.focalSize, transitionTime);

		transform.position = Vector3.Lerp (transform.position, targettedObject.GetComponent<AgentsParameters> ().anchor.position, transitionTime);

		if (GetComponent<Transform> ().position == targettedObject.transform.position) {
			//transiting = false;
		}
	}
}