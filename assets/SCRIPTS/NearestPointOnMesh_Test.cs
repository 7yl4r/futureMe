using UnityEngine;
using System.Collections;

public class NearestPointOnMesh_Test : UUnitTestCase
{
	[UUnitTest]
	public void nearestPointOnPlane_is_projectedVertex() {
	  	Debug.Log("C# test runs");
		
		// test point @ origin
		Vector3 p = new Vector3(0,0,0);
		
		// test mesh is 1x1 plane at z = 1
		MeshFilter mf = new MeshFilter();
		Mesh mesh = new Mesh ();
		mf.mesh = mesh;
		Vector3[] pl = new Vector3[]{new Vector3( 1, 1, 1),
		                   new Vector3( 1,-1, 1),
						   new Vector3(-1, 1, 1),
						   new Vector3(-1,-1, 1)};
		mesh.vertices = pl;
		mesh.uv = new Vector2[]{new Vector2(1,1),
		                        new Vector2(1,-1),
								new Vector2(-1,1),
								new Vector2(-1,-1)};
		mesh.triangles = new int[]{0,1,2,3,2,1};
		
		
		Vector3 projection = new Vector3( 0, 0, 1);
	
	  	UUnitAssert.Equals( projection, NearestPointOnMesh.getNearestPointOnMesh(p,mf) );
	}
}

