using UnityEngine;
using System.Collections;

public class ControlProbe : MonoBehaviour {

	public GameObject plane;
	public GameObject camera;
	public Direction directionFacing;
	
	// Update is called once per frame
	void Update () {
		
		if (directionFacing == Direction.X) {
			float offset = (plane.transform.position.x - camera.transform.position.x);

			transform.position.Set (plane.transform.position.x + offset, camera.transform.position.y, camera.transform.position.z);

			/*transform.position.x = plane.transform.position.x + offset;
			transform.position.y = camera.transform.position.y;
			transform.position.z = camera.transform.position.z;*/
		}
		else if (directionFacing == Direction.Y) {
			float offset = (plane.transform.position.y - camera.transform.position.y);

			transform.position = new Vector3 (camera.transform.position.x, plane.transform.position.y + offset, camera.transform.position.z);

			/*transform.position.x = camera.transform.position.x;
			transform.position.y = plane.transform.position.y + offset;
			transform.position.z = camera.transform.position.z;*/
		}
		else if (directionFacing == Direction.Z) {
			float offset = (plane.transform.position.z - camera.transform.position.z);

			transform.position = new Vector3 (camera.transform.position.x, camera.transform.position.y, plane.transform.position.z + offset);
		}
	}

	public enum Direction {
		X, Y, Z
	}
}
