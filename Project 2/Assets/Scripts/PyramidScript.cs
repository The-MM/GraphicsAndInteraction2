using UnityEngine;
using System.Collections;

public class PyramidScript : MonoBehaviour
{
	// Use this for initialization
	void Start()
	{
		// Add a MeshFilter component to this entity. This essentially comprises of a
		// mesh definition, which in this example is a collection of vertices, colours 
		// and triangles (groups of three vertices). 
		MeshFilter cubeMesh = this.gameObject.AddComponent<MeshFilter>();
		cubeMesh.mesh = this.CreatePyramidMesh();

		// Add a MeshRenderer component. This component actually renders the mesh that
		// is defined by the MeshFilter component.
		MeshRenderer renderer = this.gameObject.AddComponent<MeshRenderer>();
		renderer.material.shader = Shader.Find("Unlit/VertexColorShader");
	}

	// (Task 3 solution)
	// Method to create a pyramid mesh with coloured vertices
	Mesh CreatePyramidMesh()
	{
		Mesh m = new Mesh();
		m.name = "Cube";

		// Define the vertices. These are the "points" in 3D space that allow us to
		// construct 3D geometry (by connecting groups of 3 points into triangles).
		m.vertices = new[] {
			new Vector3(-1.0f, -1.0f, -1.0f), // Base
			new Vector3(1.0f, -1.0f, 1.0f),
			new Vector3(-1.0f, -1.0f, 1.0f),
			new Vector3(-1.0f, -1.0f, -1.0f),
			new Vector3(1.0f, -1.0f, -1.0f),
			new Vector3(1.0f, -1.0f, 1.0f),

			new Vector3(-1.0f, -1.0f, 1.0f), // Front
			new Vector3(1.0f, -1.0f, 1.0f),
			new Vector3(0.0f, 1.0f, 0.0f),

			new Vector3(1.0f, -1.0f, -1.0f), // Back
			new Vector3(-1.0f, -1.0f, -1.0f),
			new Vector3(0.0f, 1.0f, 0.0f),

			new Vector3(-1.0f, -1.0f, -1.0f), // Left
			new Vector3(-1.0f, -1.0f, 1.0f),
			new Vector3(0.0f, 1.0f, 0.0f),

			new Vector3(1.0f, -1.0f, 1.0f), // Right
			new Vector3(1.0f, -1.0f, -1.0f),
			new Vector3(0.0f, 1.0f, 0.0f)
		};

		// Define the vertex colours
		m.colors = new[] {
			Color.red, // Base
			Color.red,
			Color.red,
			Color.red,
			Color.red,
			Color.red,

			Color.yellow, // Front
			Color.yellow,
			Color.yellow,

			Color.green, // Back
			Color.green,
			Color.green,

			Color.blue, // Left
			Color.blue,
			Color.blue,

			Color.grey, // Right
			Color.grey,
			Color.grey
		};

		// Automatically define the triangles based on the number of vertices
		int[] triangles = new int[m.vertices.Length];
		for (int i = 0; i < m.vertices.Length; i++)
			triangles[i] = i;

		m.triangles = triangles;

		return m;
	}
}
