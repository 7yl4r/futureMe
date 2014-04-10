using UnityEngine;
using System.Collections;
using System.Linq;
 
public class deformMesh : MonoBehaviour{
	public float radius = 0.0f;
	public float pull = 1.0f;
    public float percentTowards = .5f;
	public MeshFilter meshGoal;
	public MeshFilter meshScan;

	public Falloff.type fallOff = Falloff.type.Gauss;
	
	private MeshFilter unappliedMesh;
    private enum selection { paintDeform, bulge };
    private deformMesh.selection selected = selection.paintDeform;
    private Vector3[] originalVerts;
	
    void Start(){
        originalVerts = meshScan.mesh.vertices;
    }
    
	void Update(){
        Debug.Log("update()");
        // When no button is pressed we update the mesh collider
        if (!Input.GetMouseButton (0)){
            // Apply collision mesh when we let go of button
            ApplyMeshCollider();
            return;
        }
        
        // is mouse on the surface?
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast (ray, out hit)){
            MeshFilter hit_mf = hit.collider.GetComponent<MeshFilter>();
            if (hit_mf)
            {
                Debug.Log("hit "+hit.collider.gameObject.name.ToString());
                // Don't update mesh collider every frame since physX
                // does some heavy processing to optimize the collision mesh.
                // So this is not fast enough for real time updating every frame
                if (meshScan != unappliedMesh)
                {
                    ApplyMeshCollider();
                    unappliedMesh = meshScan;
                }
                
                // Deform mesh
                //Vector3 relativePoint = meshScan.transform.InverseTransformPoint(hit.point);
        
                switch (selected)
                {
                    case selection.paintDeform:
                    case selection.bulge:
                        selectiveDeform(hit.point);
                        break;
                    default:
                        Debug.Log("unrecognized selection");
                        break;
                }
            }
        }
	}
    
    void OnGUI () {
        // Make a background box
        GUI.Box(new Rect(10,10,100,150), "Menu");

        // Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
        if(GUI.Button(new Rect(20,40,80,20), "deform all")) {
            StartCoroutine(deformAll());
        }
        
        // Make the second button.
        if(GUI.Button(new Rect(20,70,80,20), "paint deform")) {
            selected = selection.paintDeform;
        }
        
        if(GUI.Button(new Rect(20,100,80,20), "reset mesh")) {
            resetMesh();
        }
        
        if(GUI.Button(new Rect(20,130,80,20), "paint bulge")) {
            selected = selection.bulge;
        }
    }
    
    void resetMesh(){
        meshScan.mesh.vertices = originalVerts;
        meshScan.mesh.RecalculateNormals();
		meshScan.mesh.RecalculateBounds();
        Debug.Log("mesh reset.");
    }
    
    Vector3 moveVertex(Vector3 vertex){
        // Don't update mesh collider every frame since physX
        // does some heavy processing to optimize the collision mesh.
        // So this is not fast enough for real time updating every frame
        if (meshScan != unappliedMesh)
        {
            ApplyMeshCollider();
            unappliedMesh = meshScan;
        }
        
    //    Vector3 relativePoint = meshScan.transform.InverseTransformPoint(vertex);
        
		Vector3 motion = NearestPointOnMesh.lerpMove(vertex,meshGoal,percentTowards);
			
        vertex += motion ;
		return vertex;
    }
	
	IEnumerator deformAll(){
		// deform all points on the meshScan
        Debug.Log("started mesh deformation");
		Vector3[] vertices = meshScan.mesh.vertices;
        int vertex_count = 0;
        for (int i=0; i<vertices.Length; i++){
            vertices[i] = moveVertex(vertices[i]);
            meshScan.mesh.vertices = vertices;
            meshScan.mesh.RecalculateNormals();
            meshScan.mesh.RecalculateBounds();
            vertex_count++;
            Debug.Log(vertex_count.ToString() + "vertices deformed");
            yield return 0;
        }
        Debug.Log("done.");
        yield return 1;
    }
	
	void selectiveDeform(Vector3 relativePoint){
		// deform area specified by mouse
        /*
		// Did we hit the surface?
		RaycastHit hit = new RaycastHit();
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast (ray, out hit)){
			if (meshScan)
			{
				// Don't update mesh collider every frame since physX
				// does some heavy processing to optimize the collision mesh.
				// So this is not fast enough for real time updating every frame
				if (meshScan != unappliedMesh)
				{
					ApplyMeshCollider();
					unappliedMesh = meshScan;
				}
				
				// Deform mesh
				Vector3 relativePoint = meshScan.transform.InverseTransformPoint(hit.point);
        */
                switch (selected)
                {
                    case selection.paintDeform:
                        DeformMesh(relativePoint, pull * Time.deltaTime);//hit.point, pull * Time.deltaTime);
                        break;
                    case selection.bulge:
                        BulgeMesh(meshScan, relativePoint,pull*Time.deltaTime, radius);
                        break;
                    default:
                        Debug.Log("unrecognized selection");
                        break;
                }
		//	}
		//}
	}
	
	void DeformMesh(Vector3 position, float power){
		Vector3[] vertices = meshScan.mesh.vertices;
		float sqrRadius = radius * radius;

		Vector3 averageNormal = NearestPointOnMesh.lerpMove(position,meshGoal,percentTowards);
		
		// Deform vertices along averaged normal
		for (int i=0; i<vertices.Length; i++){
			float sqrMagnitude = (vertices[i] - position).sqrMagnitude;
			// Early out if vertex too far away form position
			if (sqrMagnitude > sqrRadius)
				continue;
				
			float distance = Mathf.Sqrt(sqrMagnitude);
			float falloff = 0.0f;
			switch (fallOff)
			{
				case Falloff.type.Gauss:
					falloff = Falloff.Gauss(distance, radius);
					break;
				case Falloff.type.Needle:
					falloff = Falloff.Needle(distance, radius);
					break;
                case Falloff.type.Linear:
					falloff = Falloff.Linear(distance, radius);
					break;
                default:
                    Debug.Log("unrecognized falloff type");
                    break;
			}
			
			vertices[i] += averageNormal * falloff * power;
			
		}
		
		meshScan.mesh.vertices = vertices;
		meshScan.mesh.RecalculateNormals();
		meshScan.mesh.RecalculateBounds();
	}
	
    void BulgeMesh (MeshFilter mf, Vector3 pos, float power, float inRadius){
        Vector3 position = mf.transform.InverseTransformPoint(pos);
        Mesh mesh = mf.mesh;
    
        Vector3[] vertices = mesh.vertices;
        Vector3[] normals = mesh.normals;
        float sqrRadius = inRadius * inRadius;
        
        // Calculate averaged normal of all surrounding vertices	
        Vector3 averageNormal = Vector3.zero;
        for (int i=0;i<vertices.Length;i++)
        {
            float sqrMagnitude = (vertices[i] - position).sqrMagnitude;
            // Early out if too far away
            if (sqrMagnitude > sqrRadius)
                continue;

            float distance = Mathf.Sqrt(sqrMagnitude);
            float falloff = Falloff.Linear(distance, inRadius);
            averageNormal += falloff * normals[i];
        }
        averageNormal = averageNormal.normalized;
        
        // Deform vertices along averaged normal
        for (int i=0;i<vertices.Length;i++)
        {
            float sqrMagnitude = (vertices[i] - position).sqrMagnitude;
            // Early out if too far away
            if (sqrMagnitude > sqrRadius)
                continue;

            float distance = Mathf.Sqrt(sqrMagnitude);
            float falloff = 0f;
            switch (fallOff)
            {
                case Falloff.type.Gauss:
                    falloff = Falloff.Gauss(distance, inRadius);
                    break;
                case Falloff.type.Needle:
                    falloff = Falloff.Needle(distance, inRadius);
                    break;
                default:
                    falloff = Falloff.Linear(distance, inRadius);
                    break;
            }
            
            vertices[i] += averageNormal * falloff * power;
        }
        
        mesh.vertices = vertices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }
    
	void ApplyMeshCollider () {
		if (unappliedMesh && unappliedMesh.GetComponent<MeshCollider>()) {
			unappliedMesh.GetComponent<MeshCollider>().mesh = unappliedMesh.mesh;
		}
		unappliedMesh = null;
	}

}
