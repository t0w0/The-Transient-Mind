using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {

	private GameObject camera;
	public GameObject titleCan;
	public Animator titleAnimator;

	public GameObject startAgent;

	public float transitionTime = 0.025f;
	public bool start = false;

	// Use this for initialization
	void Start () {

		camera = Camera.main.gameObject;
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

		if (titleAnimator.GetBool ("SpacePress")) {
			camera.GetComponent<Animation> ().Stop ();
			camera.transform.position = Vector3.Lerp (camera.transform.position, startAgent.transform.position, transitionTime);

			if (camera.transform.position == startAgent.transform.position) {
				camera.transform.parent.GetComponent<Targetting> ().enabled = true;
				start = true;
			}
		}
	}
}
