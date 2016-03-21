#include "Smooth.h"



smooth::smooth()
{
}

smooth::smooth(vector<vertex_3> vertices, vector<indexed_triangle> triangles)
{
	this->vertices = vertices;
	this->triangles = triangles;
}

smooth::~smooth()
{

}

void smooth::laplace_smooth(const float scale)
{
	vector<vertex_3> displacements(vertices.size(), vertex_3(0, 0, 0));

	// Get per-vertex displacement.
	for (size_t i = 0; i < vertices.size(); i++)
	{
		// Skip rogue vertices (which were probably made rogue during a previous
		// attempt to fix mesh cracks).
		if (0 == vertex_to_vertex_indices[i].size())
			continue;

		const float weight = 1.0f / static_cast<float>(vertex_to_vertex_indices[i].size());

		for (size_t j = 0; j < vertex_to_vertex_indices[i].size(); j++)
		{
			size_t neighbour_j = vertex_to_vertex_indices[i][j];
			displacements[i] += (vertices[neighbour_j] - vertices[i])*weight;
		}
	}

	// Apply per-vertex displacement.
	for (size_t i = 0; i < vertices.size(); i++)
		vertices[i] += displacements[i] * scale;
}

void smooth::taubin_smooth(const float lambda, const float mu, const size_t steps)
{
	cout << "Smoothing mesh using Taubin lambda|mu algorithm ";
	cout << "(inverse neighbour count weighting)" << endl;

	for (size_t s = 0; s < steps; s++)
	{
		cout << "Step " << s + 1 << " of " << steps << endl;

		laplace_smooth(lambda);
		laplace_smooth(mu);
	}

	// Recalculate normals, if necessary.
	regenerate_vertex_and_triangle_normals_if_exists();
}

///
///
///

void smooth::regenerate_vertex_and_triangle_normals_if_exists(void)
{
	if (triangle_normals.size() > 0)
		generate_triangle_normals();

	if (vertex_normals.size() > 0)
		generate_vertex_normals();
}

void smooth::generate_vertex_normals(void)
{
	if (triangles.size() == 0 || vertices.size() == 0)
		return;

	vertex_normals.clear();
	vertex_normals.resize(vertices.size());

	for (size_t i = 0; i < triangles.size(); i++)
	{
		vertex_3 v0 = vertices[triangles[i].vertex_indices[1]] - vertices[triangles[i].vertex_indices[0]];
		vertex_3 v1 = vertices[triangles[i].vertex_indices[2]] - vertices[triangles[i].vertex_indices[0]];
		vertex_3 v2 = v0.cross(v1);

		vertex_normals[triangles[i].vertex_indices[0]] = vertex_normals[triangles[i].vertex_indices[0]] + v2;
		vertex_normals[triangles[i].vertex_indices[1]] = vertex_normals[triangles[i].vertex_indices[1]] + v2;
		vertex_normals[triangles[i].vertex_indices[2]] = vertex_normals[triangles[i].vertex_indices[2]] + v2;
	}

	for (size_t i = 0; i < vertex_normals.size(); i++)
		vertex_normals[i].normalize();
}

void smooth::generate_triangle_normals(void)
{
	if (triangles.size() == 0)
		return;

	triangle_normals.clear();
	triangle_normals.resize(triangles.size());

	for (size_t i = 0; i < triangles.size(); i++)
	{
		vertex_3 v0 = vertices[triangles[i].vertex_indices[1]] - vertices[triangles[i].vertex_indices[0]];
		vertex_3 v1 = vertices[triangles[i].vertex_indices[2]] - vertices[triangles[i].vertex_indices[0]];
		triangle_normals[i] = v0.cross(v1);
		triangle_normals[i].normalize();
	}
}
