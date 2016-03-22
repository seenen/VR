#pragma once

#include "primitives.h"

#include <iostream>
using std::cout;
using std::endl;

#include <fstream>
using std::ifstream;
using std::ofstream;

#include <iomanip>
using std::setiosflags;

#include <ios>
using std::ios_base;
using std::ios;

#include <set>
using std::set;

#include <vector>
using std::vector;

#include <limits>
using std::numeric_limits;

#include <cstring>
#include <cctype>


class indexed_mesh
{
public:
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

	bool load_from_ascii_stereo_lithography_file(const char *const file_name, const bool generate_normals = true, const size_t buffer_width = 65536);
	bool load_from_binary_stereo_lithography_file(const char *const file_name, const bool generate_normals = true, const size_t buffer_width = 65536);
	bool save_to_binary_stereo_lithography_file(const char *const file_name, const size_t buffer_width = 65536);
	bool save_to_povray_mesh2_file(const char *const file_name, const bool write_vertex_normals = false);

	void set_max_extent(float max_extent);

	// See: Geometric Signal Processing on Polygonal Meshes by G. Taubin
	void laplace_smooth(const float scale);
	void taubin_smooth(const float lambda, const float mu, const size_t steps);

	void fix_cracks(void);

private:
	void generate_vertex_normals(void);
	void generate_triangle_normals(void);
	void generate_vertex_and_triangle_normals(void);
	void regenerate_vertex_and_triangle_normals_if_exists(void);
	template<typename T> void eliminate_vector_duplicates(vector<T> &v);
	bool merge_vertex_pair(const size_t keeper, const size_t goner);
};
