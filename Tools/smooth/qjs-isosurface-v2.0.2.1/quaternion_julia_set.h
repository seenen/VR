// Source code by Shawn Halayka
// Source code is in the public domain


#ifndef QJS_H
#define QJS_H

#include <cstdlib>
#include <GL/glew.h>
#include <GL/glut.h>


// **For Win32/MSVC++**
#ifdef _MSC_VER
	#pragma comment(lib, "glew32")
	#pragma comment(lib, "glut32")
#endif




#include "primitives.h"
#include "marching_cubes.h"
using marching_cubes::mc_grid_cube;
using marching_cubes::max_triangles_per_mc_cell;

#include "quaternion_math.h"
#include "eqparse.h"

#include "string_utilities.h"
using string_utilities::lower_string;
using string_utilities::trim_whitespace_string;
using string_utilities::stl_str_tok;
using string_utilities::is_real_number;
using string_utilities::is_unsigned_int;


#include <cstring> // For memcpy()
#include <ctime>


#include <iostream>
using std::cout;
using std::endl;

#include <cmath>
using std::sqrt;

#include <vector>
using std::vector;

#include <string>
using std::string;

#include <fstream>
using std::ifstream;

#include <sstream>
using std::istringstream;

#include <iomanip>
using std::ios_base;
using std::setprecision;
using std::fixed;


class quaternion_julia_set
{
public:
	quaternion_julia_set(void)
	{
		z_w = 0;
		C.x = 0;
		C.y = 0;
		C.z = 0;
		C.w = 0;
		x_grid_max = y_grid_max = z_grid_max = 1.5;
		x_grid_min = y_grid_min = z_grid_min = -x_grid_max;
		x_res = y_res = z_res = 100;
		max_iterations = 8;
		threshold = 4;
		equation_text = "";

		parameters_configured = false;

		// Initialize OpenGL using the GLUT helper library.
		// Fake the command-line argument(s) that GLUT expects.
		int argc = 1;
		char *argv[] = {"qjs"};

		glutInit(&argc, argv);
		glutInitDisplayMode(GLUT_RGBA);
		glut_window_handle = glutCreateWindow("");

		// Load OpenGL 2.x extensions using GLEW helper library.
		if(GLEW_OK != glewInit())
			opengl_init_ok = false;
		else
			opengl_init_ok = true;
	}

	~quaternion_julia_set(void)
	{
		glutDestroyWindow(glut_window_handle);
	}

	bool load_parameters_from_file(const char *file_name);
	bool generate_and_write_isosurface_to_binary_stl_file(const char *file_name, const bool try_gpu = false, const bool make_border = false);

protected:
	bool generate_and_write_via_CPU(const char *file_name, const bool make_border = false);
	bool generate_and_write_via_GPU(const char *file_name, const bool make_border = false);

	void tesselate_adjacent_xy_plane_pair(const vector<float> &xyplane0, const vector<float> &xyplane1, const size_t z, vector<char> &buffer, size_t &plane_pair_triangle_count, const float isovalue, const float x_grid_min, const float x_grid_max, const size_t x_res, const float y_grid_min, const float y_grid_max, const size_t y_res, const float z_grid_min, const float z_grid_max, const size_t z_res);
	bool initialize_shader(const char *frag_filename, GLint &shader);

	bool setup_equation_text(const string &src_formula_text, string &error_string);

	float z_w;
	quaternion C;
	float x_grid_max, y_grid_max, z_grid_max;
	float x_grid_min, y_grid_min, z_grid_min;
	size_t x_res, y_res, z_res;
	short unsigned int max_iterations;
	float threshold;
	string equation_text;
	quaternion_julia_set_equation_parser eqparser;
	bool parameters_configured;

	bool opengl_init_ok;
	int glut_window_handle;
};


#endif
