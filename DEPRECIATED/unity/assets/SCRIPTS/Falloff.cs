/*
Defines filter falloff functions using distance from center and radius of filter.
Enum type can be used to select filter type and run corresponding function.
*/

using UnityEngine;
using System.Collections;

public class Falloff {
	public enum type { Gauss, Linear, Needle };
    
	public static float Linear(float distance , float inRadius) {
		return Mathf.Clamp01(1.0f - distance / inRadius);
	}

	public static float Gauss(float distance ,float inRadius) {
		return Mathf.Clamp01 (Mathf.Pow (360.0f, -Mathf.Pow (distance / inRadius, 2.5f) - 0.01f));
	}

	public static float Needle(float dist, float inRadius){
		return -(dist*dist) / (inRadius * inRadius) + 1.0f;
	}
}
