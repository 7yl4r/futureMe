using UnityEngine;
using System.Collections;

public class NearestPointOnMesh_Test : UUnitTestCase
{
	// using manually-created 4-point plane, point is between plane and origin
	[UUnitTest]
	public void nearestPointOnPlane_is_projectedVertex_if_pointBetweenPlaneAndOrigin() {
	  	Debug.Log(" === known plane-projected vertex test for point(0,0,10) === ");

		// test point @ origin
		Vector3 p = new Vector3(0,0,5);
			
		// test mesh is 1x1 plane at z = 1
		GameObject plane = new GameObject("testPlane");
		MeshFilter mf = (MeshFilter)plane.AddComponent(typeof(MeshFilter));
		
		Vector3[] pl = new Vector3[]{new Vector3( 10, 10, 10),
				   new Vector3( 10,-10, 10),
				   new Vector3(-10, 10, 10),
				   new Vector3(-10,-10, 10)};
		
		mf.mesh = CreateMesh(pl);
		
		Vector3 projection = new Vector3( 0, 0, 10);
		Vector3 result = NearestPointOnMesh.getNearestPointOnMesh(p,mf);
		
	  	UUnitAssert.Equals( projection, result );
	}

	// using manually-created 4-point plane, point is not @ origin
	[UUnitTest]
	public void nearestPointOnPlane_is_projectedVertex_if_pointNotAtOrigin() {
	  	Debug.Log(" === known plane-projected vertex test for point(0,0,10) === ");

		// test point @ origin
		Vector3 p = new Vector3(0,0,10);
			
		// test mesh is 1x1 plane at z = 1
		GameObject plane = new GameObject("testPlane");
		MeshFilter mf = (MeshFilter)plane.AddComponent(typeof(MeshFilter));
		
		Vector3[] pl = new Vector3[]{new Vector3( 1, 1, 1),
				   new Vector3( 1,-1, 1),
				   new Vector3(-1, 1, 1),
				   new Vector3(-1,-1, 1)};
		
		mf.mesh = CreateMesh(pl);
		
		
		
		Vector3 projection = new Vector3( 0, 0, 1);
		Vector3 result = NearestPointOnMesh.getNearestPointOnMesh(p,mf);
		
	  	UUnitAssert.Equals( projection, result );
	}

	// using manually-created 4-point plane
	[UUnitTest]
	public void nearestPointOnPlane_is_projectedVertex() {
	  	Debug.Log(" === known plane-projected vertex test === ");

		// test point @ origin
		Vector3 p = new Vector3(0,0,0);
			
		// test mesh is 1x1 plane at z = 1
		GameObject plane = new GameObject("testPlane");
		MeshFilter mf = (MeshFilter)plane.AddComponent(typeof(MeshFilter));
		
		Vector3[] pl = new Vector3[]{new Vector3( 1, 1, 1),
				   new Vector3( 1,-1, 1),
				   new Vector3(-1, 1, 1),
				   new Vector3(-1,-1, 1)};
		
		mf.mesh = CreateMesh(pl);
		
		
		
		Vector3 projection = new Vector3( 0, 0, 1);
		Vector3 result = NearestPointOnMesh.getNearestPointOnMesh(p,mf);
		
	  	UUnitAssert.Equals( projection, result );
	}
	
	//using gameObject.CreatePrimitive(PrimitiveType.Plane);
	[UUnitTest]
	public void nearestPointOnPrimitivePlane_is_projectedVertex() {
	  	Debug.Log(" === known PrimitiveType.Plane-projected vertex test === ");
		
		// test point @ origin
		Vector3 p = new Vector3(1,1,1);
			
		// test mesh is 1x1 plane at z = 1
		//GameObject pl = new GameObject("testPlane");
		GameObject pl = GameObject.CreatePrimitive(PrimitiveType.Plane);
		MeshFilter mf = pl.GetComponent<MeshFilter>();
		
		Vector3 projection = new Vector3( 1, 0, 1);
		Vector3 result = NearestPointOnMesh.getNearestPointOnMesh(p,mf);
		
	  	UUnitAssert.Equals( projection, result );
	}
	
	[UUnitTest]
	public void nearestPointOnScaledCube_is_scaled(){
		// ensures that a point at 1,0,0 moves to a point at .5,0,0 if cube at origin w/ scale .5,.5,.5
		Debug.Log(" === scaled cube test === ");

		
		Vector3 p = new Vector3(1,2,2);
		
		GameObject cu = GameObject.CreatePrimitive(PrimitiveType.Cube);
		cu.transform.localScale = new Vector3(0.5f,2f,2f);
		MeshFilter mf = cu.GetComponent<MeshFilter>();
		
		Vector3 expected = new Vector3(.5f,2f,2f);
		Vector3 result = NearestPointOnMesh.getNearestPointOnMesh(p,mf);
		
		Debug.Log(expected.ToString() + "=?=" + result.ToString());
		
		UUnitAssert.Equals( expected, result);
	
	}
	
	// HELPERS
	Mesh CreateMesh(Vector3[] v)
	{
		Mesh m = new Mesh();
		m.name = "ScriptedMesh";
		m.vertices = v;
		m.uv = new Vector2[] {
			new Vector2 (0, 0),
			new Vector2 (0, 1),
			new Vector2(1, 1),
			new Vector2 (1, 0)
			};
		m.triangles = new int[] { 0, 1, 2, 0, 2, 3};
		m.RecalculateNormals();
		 
		return m;
	}
}