using System.Collections.Generic;
using UnityEngine;
using MathNet.Numerics;

namespace StructureGraph
{
	public class Force
	{
		public Node Node;
		public Vector3 ForceVector;
		public GameObject Model;
		public GameObject ArrowModel;
		public float Diameter = 5;


		public Force(Node node, Vector3 force)
		{
			Node = node;
			ForceVector = force;
			if (ForceVector.magnitude > 0.1)
				CreateModel ();
		}

		private void CreateModel()
		{
				Model = GameObject.CreatePrimitive (PrimitiveType.Cylinder);
				//ArrowModel = GameObject.CreatePrimitive (PrimitiveType.Sphere);
				Refresh ();
		}

		public void Refresh()
		{
			if (Model != null) {
				Model.transform.position = Node.Model.transform.position + ForceVector / 2;
				Model.transform.localScale = new Vector3 (Diameter, ForceVector.magnitude / 2, Diameter);
				Model.transform.eulerAngles = Quaternion.LookRotation (ForceVector).eulerAngles + new Vector3 (90, 0, 0);
			}

			//Model.transform.position = Node.Model.transform.position + ForceVector;
			//Model.transform.localScale = new Vector3(Radius, Radius, Radius);
			//Model.transform.eulerAngles = Quaternion.LookRotation(ForceVector).eulerAngles + new Vector3(90,0,0);
		}
	}
}
