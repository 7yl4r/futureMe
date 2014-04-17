/*
Defines methods for linearly interpolating between given point and the nearest point on a given mesh
*/

using UnityEngine;
using System.Collections;

public class lerpNearestOnMesh {
	
	public static Vector3 lerpMove(Vector3 from,MeshFilter to, float t){
    // returns movement vector for "from" point to be t% of the way towards nearest point on "to"
        Vector3 lerpPoint = lerpNearest(from,to,t);
        Vector3 move = lerpPoint-from;
        Debug.Log("movement vector"+move.ToString());
        return move;
    }

    public static Vector3 lerpNearest(Vector3 from, MeshFilter to, float t){
    //Linearly interpolates between given point and nearest point on mesh by fraction t
        Mesh mesh = to.mesh;
		VertTriList vt = new VertTriList(mesh);
		Vector3 objSpacePt = from;// to.transform.InverseTransformPoint(from);
		Vector3[] verts = mesh.vertices;
		KDTree kd = KDTree.MakeFromPoints(verts);
        Vector3 meshPt = NearestPointOnMesh.getNearestPointOnMesh(objSpacePt, verts, kd, mesh.triangles, vt);
		Vector3 closest = meshPt; //= to.transform.TransformPoint(meshPt);
        
        Debug.Log("selected:"+from.ToString()+" nearest:"+closest.ToString());
		Vector3 towards = Vector3.Lerp(from,closest,t);
		Debug.Log("lerp point"+towards.ToString());
		return towards;
    }
	
}