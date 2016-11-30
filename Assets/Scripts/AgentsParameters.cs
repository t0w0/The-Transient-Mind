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
	[Range(1, 179)]
	public int fov = 60;

	[Range(0f, 5f)]
	public float saturation = 1;

	[Range(0.01f, 0.92f)]
	public float motionBlurStrength = 0;

	[Range(0.0f, 10.0f)]
	public float blurSize = 0;

	[Range(0f, 2f)]
	public float focalSize = 0.01f;

	//Interact
	[Range(1, 100)]
	public int interactionRange = 5;

}
