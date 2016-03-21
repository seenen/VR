#pragma once

#include "primitives.h"

#include <vector>
using std::vector;

class smooth
{
	void regenerate_vertex_and_triangle_normals_if_exists(void);
	void generate_vertex_normals(void);
	void generate_triangle_normals(void);

public:
	smooth();
	smooth(vector<vertex_3> vertices, vector<indexed_triangle> triangles);
	~smooth();

	void clear(void)
	{
		triangles.clear();
		vertices.clear();
		vertex_to_triangle_indices.clear();
		vertex_to_vertex_indices.clear();
		vertex_normals.clear();
		triangle_normals.clear();
	}

	vector<indexed_triangle> triangles;
	vector<vertex_3> vertices;
	vector< vector<size_t> > vertex_to_triangle_indices;
	vector< vector<size_t> > vertex_to_vertex_indices;
	vector<vertex_3> vertex_normals;
	vector<vertex_3> triangle_normals;

	void laplace_smooth(const float scale);
	void taubin_smooth(const float lambda, const float mu, const size_t steps);

};

