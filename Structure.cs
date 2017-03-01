
using System.Collections.Generic;
using UnityEngine;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;

namespace StructureGraph 
{
	public class Structure
	{
		public List<Node> Nodes = new List<Node> ();
		public List<Link> Links = new List<Link> ();
		public List<Force> Forces = new List<Force> ();
		public List<Constraint> Constraints = new List<Constraint> ();

		public float[,] DensityField;

		public int[,] ConnectivityMatrix;
		public float[,] ForceMatrix;
		public float[] ExternalForceVector;

		//public List<LoadCase> LoadCases = new List<LoadCase>();




		public Structure (List<Vector3> locations, List<int[]> connections, List<Vector3> forces, List<int> constraints)
		{
			//Add all Nodes, forces, and constraints
			Nodes = new List<Node> ();
			for (int n = 0; n < locations.Count; n++){
				Nodes.Add (new Node (locations [n]));
				Nodes[n].Model.name = "Node " + n;

				Forces.Add (new Force (Nodes [n], forces [n]));
				if (Forces[n].Model != null) 
					Forces[n].Model.name = "Node " + n + " Force";
			    
				Constraints.Add (new Constraint (Nodes [n], constraints [n]));
				if (Constraints[n].Model != null) 
					Constraints[n].Model.name = "Node " + n + " Constraint";
				
				//Add References from the Nodes to the Forces
				Forces [n].Node.Forces.Add (Forces [n]);
				Constraints [n].Node.Constraints.Add (Constraints [n]);
			}
			

			//Add all Links and add Links to Nodes
			for (int l = 0; l < connections.Count; l++){
				Links.Add(new Link(Nodes[connections[l][0]], Nodes[connections[l][1]]));
				Nodes [connections [l] [0]].Links.Add (Links [l]);
				Nodes [connections [l] [1]].Links.Add (Links [l]);
				Links[l].Model.name = "Link " + l;
			}


		}

		//For Updating Per Frame
		public void UpdateLinks()
		{foreach (var link in Links)
			link.Refresh();}

		public void UpdateNodes()
		{foreach (var node in Nodes)
			node.Refresh();}

		public void UpdateForces()
		{foreach (var force in Forces)
			force.Refresh();}

		public void UpdateConstraints()
		{foreach (var constraint in Constraints)
			if (constraint.Model != null) 
				constraint.Refresh();}


		public void GenerateConnectivityMatrix()
		{
			ConnectivityMatrix = new int[Links.Count, Nodes.Count];
			for (int l = 0; l < Links.Count; l++)
				for (int n = 0; n < Nodes.Count; n++) {
					 
					if (Links[l].Node1 == Nodes[n]) {	
					ConnectivityMatrix[l,n] = 1;
					}

					if (Links[l].Node2 == Nodes[n]) {	
						ConnectivityMatrix[l,n] = -1;
					}
				}
		}

		public void GenerateForceMatrix()
		{
			ForceMatrix = new float[3 * Links.Count, 3 * Nodes.Count]; 
			ExternalForceVector = new float[3 * Links.Count];

			//Build ExternalForceVector
			for (int l = 0; l < Links.Count; l++) {
				for (int n = 0; n < Nodes.Count; n++) {
			
					//Force on each node is 0, so read from the list of forces

				}
			}
		}

					//var fbd = new double[Sim.NumLinks * 3, Sim.NumJoints * 3];
					//var solutions = new double[Sim.NumLinks * 3];

				//for (int i = 0; i < Sim.NumLinks; i++)
				//{
				//	var linkData = LinkDataList[i];
				//	foreach (var joint in Sim.Links[i].joints)
				//		if (joint.JustATracer)
				//			linkData.ComJointIndex = Sim.Joints.IndexOf(joint);
				//	var comJointData = JointDataList[linkData.ComJointIndex];
		

			/*
					solutions[3*i] = linkData.Mass * comJointData.Acc.X;        //x
					solutions[3 * i + 1] = linkData.Mass * comJointData.Acc.Y;  //y
					solutions[3 * i + 2] = linkData.MoI * linkData.AngAcc;      //theta
		
					for (int j = 0; j < Sim.NumJoints; j++)
					{
						var jointData = JointDataList[j];
						var connection = ConnectivityMatrix[i, j];
						if (connection == 0 || j == linkData.ComJointIndex)
						{
							continue;
						}
						if (connection == -1 || connection == 1)
						{
							fbd[3 * i + 0,3 * j + 0] = connection;         //x
							fbd[3 * i + 2,3 * j + 0] = -(jointData.Pos.Y - comJointData.Pos.Y) * connection;        //xtheta
							fbd[3 * i + 1,3 * j + 1] = connection; //y
							fbd[3 * i + 2,3 * j + 1] = (jointData.Pos.X - comJointData.Pos.X) * connection;    //ytheta
							fbd[3 * i + 2,3 * j + 2] = connection; //theta
						}
					}
				}
*/
	


 
		public void GenerateDensityField (TerrainData td)
		{
			DensityField = new float[td.heightmapResolution,td.heightmapResolution];
			
		//	foreach (var node in Nodes)
		//		node.GenerateDensityField(td);
		//	foreach (var node in Nodes)
		//		for (int i = 0; i < td.heightmapResolution; i++)
		//			for (int j = 0; j < td.heightmapResolution; j++)
		//				if (node.DensityField [i, j] > 0) 
		//					DensityField [i, j] += node.DensityField [i, j];


	     	foreach (var link in Links)
	     		link.GenerateDensityField(td);
	     	foreach (var link in Links)
	     		for (int i = 0; i < td.heightmapResolution; i++)
	     			for (int j = 0; j < td.heightmapResolution; j++)
						//Picks Maximum Field Value
						if (link.DensityField [i, j] > DensityField [i, j] ) 
							DensityField [i, j] = link.DensityField [i, j];
			
			//			//Combines Fields
			//			if (link.DensityField [i, j] > 0) 
			//				DensityField [i, j] += link.DensityField [i, j];

			
		}
	}
}
