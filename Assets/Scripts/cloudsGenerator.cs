using UnityEngine;
using System.Collections;

public class cloudsGenerator : MonoBehaviour {

	public Renderer floorRenderer;

	public GameObject[] prefabs;
	public int maxObjects = 30;

	public int minHeight = 10;
	public int maxHeight = 25;
	public int scaleModifier = 1;

	public Material[] materials;

	public GameObject[] generatedObjects;

	// Use this for initialization
	void Start () {

		generatedObjects = new GameObject [maxObjects];

		for (int i = 0; i < maxObjects; i++) {
			Vector3 position = new Vector3 (Random.Range(floorRenderer.bounds.min.x, floorRenderer.bounds.max.x), Random.Range(0,maxHeight), Random.Range(floorRenderer.bounds.min.z, floorRenderer.bounds.max.z));
			generatedObjects[i] = GameObject.Instantiate (prefabs [Random.Range (0, prefabs.Length)], position, Quaternion.identity, transform) as GameObject;

			float scaler = Random.Range (0, scaleModifier);
			generatedObjects [i].transform.localScale = new Vector3 (scaler, scaler, scaler);
			generatedObjects [i].transform.rotation = Quaternion.Euler ( new Vector3 (0,Random.Range(0, 360), 0));

			int mat = Random.Range (0, materials.Length);

			Renderer[] colors = generatedObjects[i].GetComponentsInChildren<Renderer> ();
			for (int j = 0 ; j < colors.Length ; j ++) {

				colors[j].material = materials[mat];
			}
		} 

	}
}
