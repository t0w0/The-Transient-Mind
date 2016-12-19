using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {

	private Transform myTransform;
	private GameObject camera;
	public GameObject titleCan;
	public Animator titleAnimator;

	public GameObject startAgent;

	public float transitionTime = 1f;
	public bool start = false;

	// Use this for initialization
	void Start () {

		camera = Camera.main.gameObject;
		myTransform = camera.transform.parent.parent;
		titleAnimator = titleCan.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.anyKeyDown) {
		
			titleAnimator.GetComponent<Animator> ().SetBool ("KeyPress", true);
		
		}

		if (Input.GetKeyDown (KeyCode.Space) && titleAnimator.GetBool ("KeyPress")) {
		
			titleAnimator.GetComponent<Animator> ().SetBool ("SpacePress", true);
		
		}

		if (titleAnimator.GetBool ("SpacePress") && !start) {
			myTransform.GetComponent<Animation> ().Stop ();
			myTransform.GetComponent<Animation> ().enabled = false;
			myTransform.position = Vector3.MoveTowards (camera.transform.parent.position, startAgent.transform.position, transitionTime);
			myTransform.rotation = Quaternion.Euler ( Vector3.MoveTowards (camera.transform.parent.rotation.eulerAngles, startAgent.transform.rotation.eulerAngles, transitionTime));

			if (camera.transform.position == startAgent.transform.position) {
				//myTransform.GetComponent<Targetting> ().enabled = true;
				start = true;
			}
		}
	}
}
