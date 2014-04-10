/*
Defines filter falloff functions using distance from center and radius of filter.
Enum type can be used to select filter type and run corresponding function.
*/

using UnityEngine;
using System.Collections;

public class NearestPointOnMesh {

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
		Vector3 objSpacePt = to.transform.InverseTransformPoint(from);
		Vector3[] verts = mesh.vertices;
		KDTree kd = KDTree.MakeFromPoints(verts);
        Vector3 meshPt = getNearestPointOnMesh(objSpacePt, verts, kd, mesh.triangles, vt);
		Vector3 closest = to.transform.TransformPoint(meshPt);
        
        Debug.Log("selected:"+from.ToString()+" nearest:"+closest.ToString());
		Vector3 towards = Vector3.Lerp(from,closest,t);
		Debug.Log("lerp point"+towards.ToString());
		return towards;
    }

	public static Vector3 getTowardsNearestPoint(Vector3 startPoint, MeshFilter mf){
    /*
		BaryCentricDistance closestPointCalculator = new BaryCentricDistance(meshGoal);
		BaryCentricDistance.Result result = closestPointCalculator.GetClosestTriangleAndPoint(startPoint);
        Vector3 closest = result.closestPoint;
    */ 
        Mesh mesh = mf.mesh;
		VertTriList vt = new VertTriList(mesh);
		Vector3 objSpacePt = mf.transform.InverseTransformPoint(startPoint);
		Vector3[] verts = mesh.vertices;
		KDTree kd = KDTree.MakeFromPoints(verts);
        Vector3 meshPt = getNearestPointOnMesh(objSpacePt, verts, kd, mesh.triangles, vt);
		Vector3 closest = mf.transform.TransformPoint(meshPt);
        
        Debug.Log("selected:"+startPoint.ToString()+" nearest:"+closest.ToString());
		Vector3 towards = closest - startPoint;
		Debug.Log("movement dir vector"+towards.normalized.ToString());
		return towards.normalized;
	}
	
	public static Vector3 getNearestPointOnMesh(Vector3 pt, Vector3[] verts, KDTree vertProx, int[] tri, VertTriList vt) {
	//	First, find the nearest vertex (the nearest point must be on one of the triangles
	//	that uses this vertex if the mesh is convex).
		int nearest = vertProx.FindNearest(pt);
		
	//	Get the list of triangles in which the nearest vert "participates".
		int[] nearTris = vt[nearest];
		
		Vector3 nearestPt = Vector3.zero;
		float nearestSqDist = 100000000f;
		
		for (int i = 0; i < nearTris.Length; i++) {
			int triOff = nearTris[i] * 3;
			Vector3 a = verts[tri[triOff]];
			Vector3 b = verts[tri[triOff + 1]];
			Vector3 c = verts[tri[triOff + 2]];
			
			Vector3 possNearestPt = NearestPointOnTri(pt, a, b, c);
			float possNearestSqDist = (pt - possNearestPt).sqrMagnitude;
			
			if (possNearestSqDist < nearestSqDist) {
				nearestPt = possNearestPt;
				nearestSqDist = possNearestSqDist;
			}
		}
		
		return nearestPt;
	}
	
	public static Vector3 getNearestPointOnMesh(Vector3 pt, Vector3[] verts, int[] tri, VertTriList vt) {
	//	First, find the nearest vertex (the nearest point must be on one of the triangles
	//	that uses this vertex if the mesh is convex).
		int nearest = -1;
		float nearestSqDist = 100000000f;
		
		for (int i = 0; i < verts.Length; i++) {
			float sqDist = (verts[i] - pt).sqrMagnitude;
			
			if (sqDist < nearestSqDist) {
				nearest = i;
				nearestSqDist = sqDist;
			}
		}
		
	//	Get the list of triangles in which the nearest vert "participates".
		int[] nearTris = vt[nearest];
		
		Vector3 nearestPt = Vector3.zero;
		nearestSqDist = 100000000f;
		
		for (int i = 0; i < nearTris.Length; i++) {
			int triOff = nearTris[i] * 3;
			Vector3 a = verts[tri[triOff]];
			Vector3 b = verts[tri[triOff + 1]];
			Vector3 c = verts[tri[triOff + 2]];
			
			Vector3 possNearestPt = NearestPointOnTri(pt, a, b, c);
			float possNearestSqDist = (pt - possNearestPt).sqrMagnitude;
			
			if (possNearestSqDist < nearestSqDist) {
				nearestPt = possNearestPt;
				nearestSqDist = possNearestSqDist;
			}
		}
		
		return nearestPt;
	}
	
	public static Vector3 NearestPointOnTri(Vector3 pt, Vector3 a, Vector3 b, Vector3 c) {
		Vector3 edge1 = b - a;
		Vector3 edge2 = c - a;
		Vector3 edge3 = c - b;
		float edge1Len = edge1.magnitude;
		float edge2Len = edge2.magnitude;
		float edge3Len = edge3.magnitude;
		
		Vector3 ptLineA = pt - a;
		Vector3 ptLineB = pt - b;
		Vector3 ptLineC = pt - c;
		Vector3 xAxis = edge1 / edge1Len;
		Vector3 zAxis = Vector3.Cross(edge1, edge2).normalized;
		Vector3 yAxis = Vector3.Cross(zAxis, xAxis);
		
		Vector3 edge1Cross = Vector3.Cross(edge1, ptLineA);
		Vector3 edge2Cross = Vector3.Cross(edge2, -ptLineC);
		Vector3 edge3Cross = Vector3.Cross(edge3, ptLineB);
		bool edge1On = Vector3.Dot(edge1Cross, zAxis) > 0f;
		bool edge2On = Vector3.Dot(edge2Cross, zAxis) > 0f;
		bool edge3On = Vector3.Dot(edge3Cross, zAxis) > 0f;
		
	//	If the point is inside the triangle then return its coordinate.
		if (edge1On && edge2On && edge3On) {
			float xExtent = Vector3.Dot(ptLineA, xAxis);
			float yExtent = Vector3.Dot(ptLineA, yAxis);
			return a + xAxis * xExtent + yAxis * yExtent;
		}
		
	//	Otherwise, the nearest point is somewhere along one of the edges.
		Vector3 edge1Norm = xAxis;
		Vector3 edge2Norm = edge2.normalized;
		Vector3 edge3Norm = edge3.normalized;
		
		float edge1Ext = Mathf.Clamp(Vector3.Dot(edge1Norm, ptLineA), 0f, edge1Len);
		float edge2Ext = Mathf.Clamp(Vector3.Dot(edge2Norm, ptLineA), 0f, edge2Len);
		float edge3Ext = Mathf.Clamp(Vector3.Dot(edge3Norm, ptLineB), 0f, edge3Len);

		Vector3 edge1Pt = a + edge1Ext * edge1Norm;
		Vector3 edge2Pt = a + edge2Ext * edge2Norm;
		Vector3 edge3Pt = b + edge3Ext * edge3Norm;
		
		float sqDist1 = (pt - edge1Pt).sqrMagnitude;
		float sqDist2 = (pt - edge2Pt).sqrMagnitude;
		float sqDist3 = (pt - edge3Pt).sqrMagnitude;
		
		if (sqDist1 < sqDist2) {
			if (sqDist1 < sqDist3) {
				return edge1Pt;
			} else {
				return edge3Pt;
			}
		} else if (sqDist2 < sqDist3) {
			return edge2Pt;
		} else {
			return edge3Pt;
		}
	}
}

 /* alternative to nearestPointOnMesh??? maybe... seems to always return (-8, 3.9, -3.1)...
public class BaryCentricDistance {
 
    public BaryCentricDistance(MeshFilter meshfilter)
    {
       _meshfilter = meshfilter;
       _mesh = _meshfilter.sharedMesh;
       _triangles = _mesh.triangles;
       _vertices = _mesh.vertices;
       _transform = meshfilter.transform;
    }
 
    public struct Result
    {
       public float distanceSquared;
       public float distance
       {
         get
         {
          return Mathf.Sqrt(distanceSquared);
         }
       }
 
       public int triangle;
       public Vector3 normal;
       public Vector3 centre;
       public Vector3 closestPoint;
    }
 
    int[] _triangles;
    Vector3[] _vertices;
    Mesh _mesh;
    MeshFilter _meshfilter;
    Transform _transform;
 
    public Result GetClosestTriangleAndPoint(Vector3 point)
    {
 
       point = _transform.InverseTransformPoint(point);
       var minDistance = float.PositiveInfinity;
       var finalResult = new Result();
       var length = (int)(_triangles.Length/3);
       for(var t = 0; t < length; t++)
       {
         var result = GetTriangleInfoForPoint(point, t);
         if(minDistance > result.distanceSquared)
         {
          minDistance = result.distanceSquared;
          finalResult = result;
         }
       }
       finalResult.centre = _transform.TransformPoint(finalResult.centre);
       finalResult.closestPoint = _transform.TransformPoint(finalResult.closestPoint);
       finalResult.normal = _transform.TransformDirection(finalResult.normal);
       finalResult.distanceSquared = (finalResult.closestPoint - point).sqrMagnitude;
       return finalResult;
    }
 
    Result GetTriangleInfoForPoint(Vector3 point, int triangle)
    {
       Result result = new Result();
 
       result.triangle = triangle;
       result.distanceSquared = float.PositiveInfinity;
 
       if(triangle >= _triangles.Length/3)
         return result;
 
 
       //Get the vertices of the triangle
       var p1 = _vertices[ _triangles[0 + triangle*3] ];
       var p2 = _vertices[ _triangles[1 + triangle*3] ];
       var p3 = _vertices[ _triangles[2 + triangle*3] ];
 
       result.normal = Vector3.Cross((p2-p1).normalized, (p3-p1).normalized);
 
       //Project our point onto the plane
       var projected = point + Vector3.Dot((p1 - point), result.normal) * result.normal;
 
       //Calculate the barycentric coordinates
       var u = ((projected.x * p2.y) - (projected.x * p3.y) - (p2.x * projected.y) + (p2.x * p3.y) + (p3.x * projected.y) - (p3.x  * p2.y)) /
          ((p1.x * p2.y)  - (p1.x * p3.y)  - (p2.x * p1.y) + (p2.x * p3.y) + (p3.x * p1.y)  - (p3.x * p2.y));
       var v = ((p1.x * projected.y) - (p1.x * p3.y) - (projected.x * p1.y) + (projected.x * p3.y) + (p3.x * p1.y) - (p3.x * projected.y))/
          ((p1.x * p2.y)  - (p1.x * p3.y)  - (p2.x * p1.y) + (p2.x * p3.y) + (p3.x * p1.y)  - (p3.x * p2.y));
       var w = ((p1.x * p2.y) - (p1.x * projected.y) - (p2.x * p1.y) + (p2.x * projected.y) + (projected.x * p1.y) - (projected.x * p2.y))/
          ((p1.x * p2.y)  - (p1.x * p3.y)  - (p2.x * p1.y) + (p2.x * p3.y) + (p3.x * p1.y)  - (p3.x * p2.y));
 
       result.centre = p1 * 0.3333f + p2 * 0.3333f + p3 * 0.3333f;
 
       //Find the nearest point
        var vector = (new Vector3(u,v,w)).normalized;
 
 
       //work out where that point is
       var nearest = p1 * vector.x + p2 * vector.y + p3 * vector.z;  
       result.closestPoint = nearest;
       result.distanceSquared = (nearest - point).sqrMagnitude;
 
       if(float.IsNaN(result.distanceSquared))
       {
         result.distanceSquared = float.PositiveInfinity;
       }
       return result;
    }
 
}
*/

/* ALTERNATIVE to barycentricdistance???
public Vector3 NearestVertexTo(Vector3 point)
{
    // convert point to local space
    point = transform.InverseTransformPoint(point);
 
    Mesh mesh = GetComponent<MeshFilter>().mesh;
    float minDistanceSqr = Mathf.Infinity;
    Vector3 nearestVertex = Vector3.zero;
 
    // scan all vertices to find nearest
    foreach (Vector3 vertex in mesh.vertices)
    {
        Vector3 diff = point-vertex;
        float distSqr = diff.sqrMagnitude;
 
        if (distSqr < minDistanceSqr)
        {
            minDistanceSqr = distSqr;
            nearestVertex = vertex;
        }
    }
 
    // convert nearest vertex back to world space
    return transform.TransformPoint(nearestVertex);
 
}
*/
