using System.Collections.Generic;
using UnityEngine;
using MathNet.Numerics;

namespace StructureGraph
{
	public class Link
	{
		public Node Node1;
		public Node Node2;
		public GameObject Model;
		public float[,] DensityField;

		public float Radius = 20;

		//Properties for Steel
//		float ElasticModulus = 200*10^9; //Pa
//		float PoissonRatio = 0.30f; //unitless
//		float YieldStress = 250*10^6; //Pa
//		float Density = 8050f; //kg/m^3

		public float TensileForce;
		public float CrossSectionalArea;
		public float Stress;

		public Link(Node node1, Node node2)
		{
			Node1 = node1;
			Node2 = node2;
			CreateModel ();
		}

		private void CreateModel()
		{
			Model = GameObject.CreatePrimitive (PrimitiveType.Cylinder);
			Refresh ();
		}

		public void Refresh()
		{
			if (Model != null) {
				var rod = Node2.Model.transform.position - Node1.Model.transform.position;
				Model.transform.position = (Node1.Model.transform.position + Node2.Model.transform.position) / 2;
				Model.transform.localScale = new Vector3 (Radius, rod.magnitude / 2, Radius);
				Model.transform.eulerAngles = Quaternion.LookRotation (rod).eulerAngles + new Vector3 (90, 0, 0);
			}
		}

		public void GenerateDensityField (TerrainData td)
		{
			DensityField = new float[td.heightmapResolution,td.heightmapResolution];
			Vector3 size = td.size;
			Vector3 pixelSize = td.heightmapScale;//Samples per unit in each direction
			
			//Condensing Terms
			Vector3 n1Pos = Node1.Model.transform.position;
			Vector3 n2Pos = Node2.Model.transform.position;

			//Figuring out Slopes and y intercepts of parralell line segment boundaries
			//Perpendicular line has a negative inverse of slope
			float mb = -(n2Pos.x-n1Pos.x)/(n2Pos.z-n1Pos.z);

			// b = y - mx 
			float b1 = n1Pos.z - mb*n1Pos.x;
			float b2 = n2Pos.z - mb*n2Pos.x;

			//Flip the inequality when slope is negative

			bool slopeNegative = true;
			if (mb > 0)
				slopeNegative = false; 
			
			bool node2OnTop = false;
			if (n1Pos.z < n2Pos.z)
				node2OnTop = true; 
			
			bool inequalityFlip = false;
			if (node2OnTop) {
				inequalityFlip = true;
			}

			for (int i = 0; i < td.heightmapResolution; i++) {
				for (int j = 0; j < td.heightmapResolution; j++) {
					float xCord = j * pixelSize.x - td.size.x / 2;
					float zCord = i * pixelSize.z - td.size.z / 2;

					//Is in hemisphere on node 1
					if (node2OnTop) {
						//If below both lines, Is in hemisphere on node 1
						if (zCord < LineSolveY (mb, xCord, b1) && zCord < LineSolveY (mb, xCord, b2))
							DensityField [i, j] += 1.0f - FindDistance (xCord - n1Pos.x, zCord - n1Pos.z) / Radius;					
						//If above both lines, Is in hemisphere on node 2
				 		else if (zCord > LineSolveY (mb, xCord, b1) && zCord > LineSolveY (mb, xCord, b2))
							DensityField [i, j] += 1.0f - FindDistance (xCord - n2Pos.x, zCord - n2Pos.z) / Radius;
						//Must be in Parralell Line Segment Space					
						else
							DensityField [i, j] += 1.0f - FindDistanceToLine (n1Pos.x, n1Pos.z, n2Pos.x, n2Pos.z, xCord, zCord) / Radius;
					} 

					else {
						//If above both lines, Is in hemisphere on node 1
						if (zCord > LineSolveY (mb, xCord, b1) && zCord > LineSolveY (mb, xCord, b2))
							DensityField [i, j] += 1.0f - FindDistance (xCord - n1Pos.x, zCord - n1Pos.z) / Radius;					
						//If below both lines, Is in hemisphere on node 2
						else if (zCord < LineSolveY (mb, xCord, b1) && zCord < LineSolveY (mb, xCord, b2))
							DensityField [i, j] += 1.0f - FindDistance (xCord - n2Pos.x, zCord - n2Pos.z) / Radius;					
						//Must be in Parralell Line Segment Space					
						else
							DensityField [i, j] += 1.0f - FindDistanceToLine (n1Pos.x, n1Pos.z, n2Pos.x, n2Pos.z, xCord, zCord) / Radius;
					}
				}
			}
		}

		public static float LineSolveY (float m, float x, float b)
		{return m*x+b;}
		public static float LineSolveX (float m, float y, float b)
		{return (y-b)/m;}
			

		public static float FindDistance(float x, float y) 
		{return Mathf.Sqrt (Mathf.Pow (x, 2.0f) + Mathf.Pow (y, 2.0f));}

		public static float FindDistanceToLine(float x1, float y1, float x2, float y2, float x, float y)
		{return Mathf.Abs ((y2 - y1) * x - (x2 - x1) * y + x2 * y1 - y2 * x1) / FindDistance (x2 - x1, y2 - y1);}
	}

}

