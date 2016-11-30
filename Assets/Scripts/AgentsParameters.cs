using UnityEngine;
using System.Collections;

public class AgentsParameters : MonoBehaviour {

	public Transform anchor;

	//Physics
	public bool isFlying = true;
	public bool isMoving = false;

	public float scaleFactor = 1;

	//Movement


	//Vision
	public float fov = 60;
	public float visionRange = 0.01f;
	public float blurStrength = 0;
	public float saturation = 1;
	public float fishEyeStrength = 0;
	public float motionBlurStrength = 0;

	//Interact
	public int interactionRange = 5;

}
