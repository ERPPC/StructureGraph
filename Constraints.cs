using System.Collections.Generic;
using UnityEngine;
using MathNet.Numerics;

namespace StructureGraph
{
	public class Constraint
	{
		public Node Node;
		public Vector3 ForceVector;
		public GameObject Model;
		public int ConstraintType;
		//0 = no constraints
		//1 = fully constrained
		//2 = x constrained
		//3 = y constrained
		//4 = z constrained

		public Constraint(Node node, int constraintType)
		{
			Node = node;
			ConstraintType = constraintType;
			if (ConstraintType != 0)
				CreateModel ();
			
		}

		private void CreateModel()
		{
				Model = GameObject.CreatePrimitive (PrimitiveType.Cube);
				Refresh ();
		}

		public void Refresh()
		{
			if (Model != null) {	
				Model.transform.position = Node.Model.transform.position;
				Model.transform.localScale = new Vector3 (20, 20, 20);
				Model.transform.eulerAngles = Quaternion.LookRotation (ForceVector).eulerAngles + new Vector3 (90, 0, 0);
			}
		}
			
	}

}
