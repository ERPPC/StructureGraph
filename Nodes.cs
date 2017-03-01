using System.Collections.Generic;
using UnityEngine;
using MathNet.Numerics;


namespace StructureGraph
{
	//Classes to define structure
	public class Node
		{
		public List<Link> Links = new List<Link>();
		public GameObject Model;
		public float[,] DensityField;
		public float Radius;
		public List<Force> Forces = new List<Force>();
		public List<Constraint> Constraints = new List<Constraint>();

//		public List<Force> Forces;
//		public Vector3 Displacement;

		public Node(Vector3 location)
		{
			CreateModel(location);
		}

		public void Refresh()
		{
			List<float> linkRadii = new List<float> ();
			foreach (var link in Links)
				linkRadii.Add(link.Radius);
			Radius = Mathf.Max(linkRadii.ToArray());
			if (Model != null)
				Model.transform.localScale = new Vector3 (1.5f*Radius, 1.5f*Radius, 1.5f*Radius);
		}

		private void CreateModel(Vector3 location)
		{
			Model = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			Model.transform.position = location;
		}


		public void GenerateDensityField (TerrainData td)
		{
			DensityField = new float[td.heightmapResolution,td.heightmapResolution];
			Vector3 size = td.size;
			Vector3 PixelSize = td.heightmapScale; //Samples per unit in each direction

			for (int i = 0; i < td.heightmapResolution; i++) {
				for (int j = 0; j < td.heightmapResolution; j++) {
					var xCord = j * td.heightmapScale.x - td.size.x / 2;
					var zCord = i * td.heightmapScale.z - td.size.z / 2;
					var xDistance = xCord - Model.transform.position.x;
					var zDistance = zCord - Model.transform.position.z;
					DensityField [i, j] = 1.0f - FindDistance(xDistance, zDistance)/Radius;					
				}
			}
		}

		public static float FindDistance(float x, float y) 
		{return Mathf.Sqrt (Mathf.Pow (x, 2.0f) + Mathf.Pow (y, 2.0f));}
	}
}