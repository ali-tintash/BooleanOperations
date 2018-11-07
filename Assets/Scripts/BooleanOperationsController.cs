using UnityEngine;
using Parabox.CSG;
using System.Collections;
using ProBuilder.MeshOperations;
using System.Collections.Generic;

[ExecuteInEditMode]
public class BooleanOperationsController : MonoBehaviour {

	public MeshRenderer _obj1;
	public GameObject [] _objsToSubtract;

	public MeshRenderer _plane1;
	public MeshRenderer _plane2;

	public void OnSubtractButtonClick ()
	{
		CombineInstance[] combinesPlanes = new CombineInstance[2];

		MeshFilter meshFilterPlane1 = _plane1.gameObject.GetComponent<MeshFilter>();
		combinesPlanes[0].mesh = meshFilterPlane1.sharedMesh;
		combinesPlanes[0].transform = meshFilterPlane1.transform.localToWorldMatrix;

		MeshFilter meshFilterPlane2 = _plane2.gameObject.GetComponent<MeshFilter>();
		combinesPlanes[1].mesh = meshFilterPlane2.sharedMesh;
		combinesPlanes[1].transform = meshFilterPlane2.transform.localToWorldMatrix;

		Mesh combinedMeshPlanes = new Mesh();
		combinedMeshPlanes.CombineMeshes(combinesPlanes);

		GameObject combinedPlanes = new GameObject();
		combinedPlanes.AddComponent<MeshFilter>().sharedMesh = combinedMeshPlanes;
		combinedPlanes.AddComponent<MeshRenderer>().sharedMaterial = _plane1.material;


		// Perform boolean operation
		CombineInstance[] combines = new CombineInstance[_objsToSubtract.Length];

		for (int i = 0; i < _objsToSubtract.Length; i++)
		{
			MeshFilter meshFilter = _objsToSubtract[i].gameObject.GetComponent<MeshFilter>();
			combines[i].mesh = meshFilter.sharedMesh;
			combines[i].transform = meshFilter.transform.localToWorldMatrix;
		}

		Mesh combinedMesh = new Mesh();
		combinedMesh.CombineMeshes(combines);

		GameObject combinedObj = new GameObject();
		combinedObj.AddComponent<MeshFilter>().sharedMesh = combinedMesh;
		combinedObj.AddComponent<MeshRenderer>().sharedMaterial = _obj1.material;

		//Mesh m = CSG.Subtract(_obj1.gameObject, combinedPlanes);
		Mesh m = CSG.Subtract(combinedPlanes, combinedObj);

		// Create a gameObject to render the result
		GameObject composite = new GameObject();
		composite.AddComponent<MeshFilter>().sharedMesh = m;
		composite.AddComponent<MeshRenderer>().sharedMaterial = _obj1.material;

		Debug.Log("m.triangles.Length: " + m.triangles.Length);
		Debug.Log("m.vertexCount: " + m.vertexCount);

		_obj1.gameObject.SetActive(false);
	}

	public void OnUnionButtonClick ()
	{
		MeshFilter obj1MeshFilter = _obj1.GetComponent<MeshFilter>();

		for (int i = 0; i < _objsToSubtract.Length; i++)
		{
			MeshFilter meshFilter = _objsToSubtract[i].GetComponent<MeshFilter>();
			meshFilter.mesh = CSG.Subtract(_obj1.gameObject, _objsToSubtract[i]);
		}

		for (int i = 0; i < _objsToSubtract.Length; i++)
		{
			MeshFilter meshFilter = _obj1.GetComponent<MeshFilter>();
			meshFilter.mesh = CSG.Union(_obj1.gameObject, _objsToSubtract[i]);
		}
	}
}
