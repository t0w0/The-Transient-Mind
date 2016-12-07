using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {

	public GameObject titleCan;
	public GameObject introCamera;
	public GameObject playerCamera;
	public float transitionTime = 0.025f;
	public bool title = false;
	public bool transitingToBegin = false;
	public bool start = false;

	// Use this for initialization
	void Start () {
		playerCamera.transform.gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Space) && !start) {

			if (!title) {
				title = true;
				titleCan.GetComponent<Animation> ().Play ();
			} else if (!transitingToBegin) {
				introCamera.GetComponent<Animation> ().Stop ();
				transitingToBegin = true;
			}
		}
		if (transitingToBegin) {
		
			TransitionToBeginning ();

		}


	}

	public void TransitionToBeginning () {
	
		introCamera.transform.position = Vector3.Lerp (introCamera.transform.position, playerCamera.transform.position, transitionTime);
		titleCan.GetComponent<CanvasGroup>().alpha = Mathf.Lerp (titleCan.GetComponent<CanvasGroup>().alpha, 0, transitionTime);
		if (introCamera.transform.position == playerCamera.transform.position) {
			playerCamera.transform.parent.GetComponent<Targetting> ().enabled = true;
			playerCamera.transform.gameObject.SetActive (true);
			introCamera.transform.gameObject.SetActive (false);
			transitingToBegin = false;
			start = true;
		}
	
	}
}
