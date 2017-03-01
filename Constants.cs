using System.Collections.Generic;
using UnityEngine;

namespace StructureGraph
{

	//Constants and Example Parameters 
	public class Example
	{



		public static List<Vector3> Nodes()
		{
			//Sets up a joints to make simple michell truss
			return new List<Vector3>
			{new Vector3(-100, 0, 80),
				new Vector3(-100, 0, -80),
				new Vector3(0, 0, 60),
				new Vector3(0, 0, -60),
				new Vector3(-40, 0,0),
				new Vector3(100, 0,0)};

		}

		public static List<int[]> Connections()
		{
			//Sets up a connections to make simple michell truss
			return new List<int[]>
			{
				new int[]{0,2},
				new int[]{0,4},
				new int[]{1,3},
				new int[]{1,4},
				new int[]{2,5},
				new int[]{3,5},
				new int[]{4,2},
				new int[]{4,3}
			};

		}

		public static List<Vector3> Forces()
		{
			//List has 1 entry for each joint, and uses the same indexing
			return new List<Vector3>
			{new Vector3(),
				new Vector3(),
				new Vector3(),
				new Vector3(),
				new Vector3(),
				new Vector3(0, 0, -50)};

		}

		public static List<int> Constraints()
		{
			//List has 1 entry for each joint, and uses the same indexing
			//0 = no constraints
			//1 = fully constrained
			//2 = x constrained
			//3 = y constrained
			//4 = z constrained
			return new List<int>
			{1,1,0,0,0,0};

		}
	
	}
}

