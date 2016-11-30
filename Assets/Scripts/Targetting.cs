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

	public GameObject beginTarget;
	private GameObject lastTargettedObject;

	public GameObject targettedObject;	// Objet visé, null si aucun objet visé

	private int RAYCASTLENGTH = 10;	// Longueur du rayon issu de la caméra

	private CursorMode cursorMode = CursorMode.Auto;
	private Vector2 hotSpot = new Vector2(16, 16);	// Offset du centre du curseur
	public Texture2D cursorOff, cursorSelected, cursorHover;	// Textures à appliquer aux curseurs

	public float transitionTime = 10.0f;
	private bool transiting = false;

	void Start () 
	{	
		lastTargettedObject = beginTarget.gameObject;
		targettedObject = beginTarget.gameObject;

		transitionTime = Time.deltaTime / transitionTime;

		Transition (targettedObject);

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

		if (Input.GetMouseButtonDown (0)) 	// L'utilisateur relache un objet visé
		{
			
			if (rayCasted) 
			{
				Cursor.SetCursor (cursorHover, hotSpot, cursorMode);

				Transition (hitInfo.transform.parent.gameObject);

				Debug.Log ("Object targetted");
			} 
			else 
			{
				Cursor.SetCursor (cursorOff, hotSpot, cursorMode);
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
		}

		if (targettedObject.GetComponent<AgentsParameters> ().isMoving && !transiting) {
			GetComponent<Transform> ().position = targettedObject.GetComponent<AgentsParameters> ().anchor.position;
//			GetComponent<Transform> ().rotation = targettedObject.GetComponent<AgentsParameters> ().anchor.rotation;
			Debug.Log ("Boo");
		}
	}

	void LateUpdate () {
		
		if (transiting) {
			TransitionAnimation ();
		}
	}

	void Transition (GameObject target) {

		lastTargettedObject = targettedObject;
		targettedObject = target;

		lastTargettedObject.GetComponentInChildren<Renderer> ().enabled = true;
		lastTargettedObject.GetComponentInChildren<Collider> ().isTrigger = false;
		lastTargettedObject.GetComponentInChildren<Rigidbody> ().isKinematic = true;

		transiting = true;

		targettedObject.GetComponentInChildren<Renderer> ().enabled = false;
		targettedObject.GetComponentInChildren<Rigidbody> ().isKinematic = true;
		targettedObject.GetComponentInChildren<Collider> ().isTrigger = true;
	
		AgentsParameters parameters = targettedObject.GetComponent<AgentsParameters> ();

		GetComponent<Rigidbody> ().useGravity = parameters.isFlying ? false : true ;
		GetComponent<Rigidbody> ().constraints = parameters.isMoving ? RigidbodyConstraints.FreezeRotation : RigidbodyConstraints.FreezeAll;

	}

	void TransitionAnimation () {
		Debug.Log ("Transition");

		AgentsParameters parameters = targettedObject.GetComponent<AgentsParameters> ();

		RAYCASTLENGTH = parameters.interactionRange;
//		RenderSettings.fogDensity = Mathf.Lerp (RenderSettings.fogDensity, parameters.visionRange, transitionTime);
		GetComponentInChildren<Camera> ().fieldOfView = Mathf.Lerp (GetComponentInChildren<Camera> ().fieldOfView, parameters.fov, transitionTime);
		GetComponentInChildren<ColorCorrectionCurves> ().saturation = Mathf.Lerp (GetComponentInChildren<ColorCorrectionCurves> ().saturation, parameters.saturation, transitionTime);
		GetComponentInChildren<MotionBlur> ().blurAmount = Mathf.Lerp (GetComponentInChildren<MotionBlur> ().blurAmount, parameters.motionBlurStrength, transitionTime);
		GetComponentInChildren<BlurOptimized> ().blurSize = Mathf.Lerp (GetComponentInChildren<BlurOptimized> ().blurSize, parameters.blurSize, transitionTime);
		GetComponentInChildren<DepthOfField> ().focalSize = Mathf.Lerp (GetComponentInChildren<DepthOfField> ().focalSize, parameters.focalSize, transitionTime);

		transform.position = Vector3.Lerp (transform.position, targettedObject.GetComponent<AgentsParameters> ().anchor.position, transitionTime);
//		transform.rotation.SetEulerAngles (Vector3.Lerp (transform.rotation.eulerAngles, targettedObject.GetComponent<AgentsParameters> ().anchor.rotation.eulerAngles, transitionTime));

		if (GetComponent<Transform> ().position == targettedObject.transform.position) {
			transiting = false;
		}
	}
}