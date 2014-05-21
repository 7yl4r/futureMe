var radius = 1.0;	// radius of effect area
var pull = 10.0;	// strength (speed) of mesh deformation
var meshGoal : MeshFilter; // mesh we will move towards
private var unappliedMesh : MeshFilter;	// mesh we are moving from (chosen by raycasting)

enum FallOff { Gauss, Linear, Needle }
var fallOff = FallOff.Gauss;

static function LinearFalloff (distance : float , inRadius : float) {
	return Mathf.Clamp01(1.0 - distance / inRadius);
}

static function GaussFalloff (distance : float , inRadius : float) {
	return Mathf.Clamp01 (Mathf.Pow (360.0, -Mathf.Pow (distance / inRadius, 2.5) - 0.01));
}

function NeedleFalloff (dist : float, inRadius : float){
	return -(dist*dist) / (inRadius * inRadius) + 1.0;
}

function BulgeMesh (mesh : Mesh, position : Vector3, power : float, inRadius : float){
	var vertices = mesh.vertices;
	var normals = mesh.normals;
	var sqrRadius = inRadius * inRadius;
	
	// Calculate averaged normal of all surrounding vertices	
	var averageNormal = Vector3.zero;
	for (var i=0;i<vertices.length;i++)
	{
		var sqrMagnitude = (vertices[i] - position).sqrMagnitude;
		// Early out if too far away
		if (sqrMagnitude > sqrRadius)
			continue;

		var distance = Mathf.Sqrt(sqrMagnitude);
		var falloff = LinearFalloff(distance, inRadius);
		averageNormal += falloff * normals[i];
	}
	averageNormal = averageNormal.normalized;
	
	// Deform vertices along averaged normal
	for (i=0;i<vertices.length;i++)
	{
		sqrMagnitude = (vertices[i] - position).sqrMagnitude;
		// Early out if too far away
		if (sqrMagnitude > sqrRadius)
			continue;

		distance = Mathf.Sqrt(sqrMagnitude);
		switch (fallOff)
		{
			case FallOff.Gauss:
				falloff = GaussFalloff(distance, inRadius);
				break;
			case FallOff.Needle:
				falloff = NeedleFalloff(distance, inRadius);
				break;
			default:
				falloff = LinearFalloff(distance, inRadius);
				break;
		}
		
		vertices[i] += averageNormal * falloff * power;
	}
	
	mesh.vertices = vertices;
	mesh.RecalculateNormals();
	mesh.RecalculateBounds();
}

function getAvgNorm (vertices : Vector3[], normals : Vector3[], sqrRadius : float, position: Vector3, inRadius :float){
	// Calculate averaged normal of all surrounding vertices	
	var averageNormal = Vector3.zero;
	for (var i=0;i<vertices.length;i++){
		var sqrMagnitude = (vertices[i] - position).sqrMagnitude;
		// Early out if too far away
		if (sqrMagnitude > sqrRadius)
			continue;

		var distance = Mathf.Sqrt(sqrMagnitude);
		var falloff = LinearFalloff(distance, inRadius);
		averageNormal += falloff * normals[i];
	}
	return averageNormal.normalized;
}

function NearestVertexTo(point : Vector3){ 
	var cs = GameObject.Find("getNearestVertexGameObj");
//	var inst = cs.BaryCentricDistance(meshGoal);
	var barycentricDist = cs.GetComponent("BaryCentricDistance(meshGoal)");
//	var nearestPointCalc = barycentricDist(meshGoal);
	var result = barycentricDist.GetClosestTriangeAndPoint(point);
	return result.closestPoint;
}

function getTowardsGoalMesh(startPosition : Vector3){
	// create vector from 
	endPosition = NearestVertexTo(startPosition);
	towards = startPosition-endPosition;
	return towards.normalized;
}


function DeformMesh (mesh : Mesh, position : Vector3, power : float, inRadius : float){
	/*
	mesh = mesh to deform
	position = location of deformation
	power = strength of mesh deform
	inRadius = radius of effect area
	*/
	var vertices = mesh.vertices;
	var normals = mesh.normals;
	var sqrRadius = inRadius * inRadius;
		
//	averageNormal = getAvgNorm(vertices,normals,sqrRadius, position, inRadius);
	averageNormal = getTowardsGoalMesh(position);
		
	// Deform vertices along averaged normal
	for (i=0;i<vertices.length;i++)
	{
		sqrMagnitude = (vertices[i] - position).sqrMagnitude;
		// Early out if too far away
		if (sqrMagnitude > sqrRadius)
			continue;

		distance = Mathf.Sqrt(sqrMagnitude);
		switch (fallOff)
		{
			case FallOff.Gauss:
				falloff = GaussFalloff(distance, inRadius);
				break;
			case FallOff.Needle:
				falloff = NeedleFalloff(distance, inRadius);
				break;
			default:
				falloff = LinearFalloff(distance, inRadius);
				break;
		}
		
		vertices[i] += averageNormal * falloff * power;
	}
	
	mesh.vertices = vertices;
	mesh.RecalculateNormals();
	mesh.RecalculateBounds();
}

function Update () {

	// When no button is pressed we update the mesh collider
	if (!Input.GetMouseButton (0)){
		// Apply collision mesh when we let go of button
		ApplyMeshCollider();
		return;
	}
				
	// Did we hit the surface?
	var hit : RaycastHit;
	var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	if (Physics.Raycast (ray, hit)){
		var filter : MeshFilter = hit.collider.GetComponent(MeshFilter);
		if (filter)
		{
			// Don't update mesh collider every frame since physX
			// does some heavy processing to optimize the collision mesh.
			// So this is not fast enough for real time updating every frame
			if (filter != unappliedMesh)
			{
				ApplyMeshCollider();
				unappliedMesh = filter;
			}
			
			// Deform mesh
			var relativePoint = filter.transform.InverseTransformPoint(hit.point);
			DeformMesh(filter.mesh, relativePoint, pull * Time.deltaTime, radius);
		}
	}
}

function ApplyMeshCollider () {
	if (unappliedMesh && unappliedMesh.GetComponent(MeshCollider)) {
		unappliedMesh.GetComponent(MeshCollider).mesh = unappliedMesh.mesh;
	}
	unappliedMesh = null;
}