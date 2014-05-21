/*
Defines methods for linearly interpolating between given point and the nearest point on a given mesh
*/

using UnityEngine;
using System.Collections;

public class lerpNearestOnMesh_Test : UUnitTestCase	{
	[UUnitTest]
	public void lerpHalfwayToPlane(){
		Debug.Log(" === lerp 1/2 way to plane test === ");
		
		// test point
		Vector3 p = new Vector3(0,10,0);
			
		// test mesh is plane at z = 0
		GameObject pl = GameObject.CreatePrimitive(PrimitiveType.Plane);
		MeshFilter mf = pl.GetComponent<MeshFilter>();
		
		Vector3 expected = new Vector3( 0, 5, 0);
		Vector3 result = lerpNearestOnMesh.lerpNearest(p,mf,0.5f);
		
	  	UUnitAssert.Equals( expected, result );
	}
	
	[UUnitTest]
	public void moveHalfwayToPlane(){
		Debug.Log(" === move 1/2 way to plane test === ");
		
		// test point
		Vector3 p = new Vector3(0,10,0);
			
		// test mesh is plane at z = 0
		GameObject pl = GameObject.CreatePrimitive(PrimitiveType.Plane);
		MeshFilter mf = pl.GetComponent<MeshFilter>();
		
		Vector3 expected = new Vector3( 0, -5, 0);
		Vector3 result = lerpNearestOnMesh.lerpMove(p,mf,0.5f);
		
	  	UUnitAssert.Equals( expected, result );
	}
	
	
	
}