using UnityEngine;
using System.Collections;

public class objectGenerator : MonoBehaviour {

	public Renderer floorRenderer;

	public GameObject[] prefabs;
	public int maxObjects = 30;

	public int maxHeight = 25;
	public int scaleModifier = 5;
	public Material[] materials;

	public GameObject[] generatedObjects;

	// Use this for initialization
	void Start () {

		generatedObjects = new GameObject [maxObjects];

		for (int i = 0; i < maxObjects; i++) {
			Vector3 position = new Vector3 (Random.Range(floorRenderer.bounds.min.x, floorRenderer.bounds.max.x), Random.Range(0,maxHeight), Random.Range(floorRenderer.bounds.min.z, floorRenderer.bounds.max.z));
			generatedObjects[i] = GameObject.Instantiate (prefabs [Random.Range (0, prefabs.Length)], position, Quaternion.identity, transform) as GameObject;
			generatedObjects[i].GetComponent<MeshRenderer>().material = materials[Random.Range(0, materials.Length)];
			generatedObjects [i].transform.localScale = new Vector3 (Random.Range (0, scaleModifier), Random.Range (0, scaleModifier), Random.Range (0, scaleModifier));
		} 
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
