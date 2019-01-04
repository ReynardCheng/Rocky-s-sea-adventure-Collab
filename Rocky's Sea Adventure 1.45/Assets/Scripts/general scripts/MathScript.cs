using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathScript : MonoBehaviour
{


	public static Vector3 ShootInArc(Transform target,Transform myPosition, float angle)
	{
		Vector3 dir = target.position - myPosition.position;  // get target direction
		float h = dir.y;  // get height difference
		dir.y = 0;  // retain only the horizontal direction
		var dist = dir.magnitude;  // get horizontal distance
		var a = angle * Mathf.Deg2Rad;  // convert angle to radians
		dir.y = dist * Mathf.Tan(a);  // set dir to the elevation angle
		dist += h / Mathf.Tan(a);  // correct for small height differences
								   // calculate the velocity magnitude
		var vel = Mathf.Sqrt(dist * Physics.gravity.magnitude / Mathf.Sin(2 * a));
		return vel * dir.normalized;
	}
}
