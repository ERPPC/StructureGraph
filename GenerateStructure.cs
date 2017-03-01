using System.Collections.Generic;
using UnityEngine;
using StructureGraph;
using MathNet.Numerics.LinearAlgebra;



// Attach this behavior to any GameObject in the scene for initialization upon play
public class GenerateStructure : MonoBehaviour
{

	public Structure CurrentStructure;
	public GameObject MaterialField;
	public TerrainData terrainData;

	public void Start()
	{
		CurrentStructure = new Structure (Example.Nodes (), Example.Connections (), Example.Forces (), Example.Constraints ());
		CreateHeightmap ();
	//	CurrentStructure.LoadCases.Add(new LoadCase(Example.Forces,Example.Constraints));
	
		//Example.AddLoadingCondition (Constants.LoadingCondition);
	}

	public void Update()
	{
		CurrentStructure.UpdateNodes();
		CurrentStructure.UpdateLinks();
		CurrentStructure.UpdateForces();
		CurrentStructure.UpdateConstraints();

		UpdateHeightmap ();

	}


	public void CreateHeightmap()
	{
		float Width = 300;
		float Height = 200;
		float Depth = 40;

		terrainData = new TerrainData ();
		terrainData.heightmapResolution = 32*2 + 1;
		terrainData.size = new Vector3(Width,Depth,Height);
		terrainData.heightmapResolution = 32*2;

		MaterialField = Terrain.CreateTerrainGameObject(terrainData);
		MaterialField.transform.position = new Vector3 (-Width/2, -Depth*1.5f, -Height/2);
	}

	public void UpdateHeightmap()
	{
		CurrentStructure.GenerateDensityField(terrainData);
		terrainData.SetHeights(0,0,CurrentStructure.DensityField);
	}

}