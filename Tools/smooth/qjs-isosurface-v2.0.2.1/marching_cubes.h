// Modified from Paul Bourke, Polygonising a Scalar Field
// Source code by Shawn Halayka
// Source code is in the public domain


#ifndef MARCHING_CUBES_H
#define MARCHING_CUBES_H


#include "primitives.h"


#include <iostream>
using std::cout;
using std::endl;

#include <fstream>
using std::ofstream;

#include <iomanip>
using std::ios_base;

#include <vector>
using std::vector;

#include <set>
using std::set;




namespace marching_cubes
{
	class mc_grid_cube
	{
	public:
		vertex_3 vertex[8];
		float value[8];
	};

	// Marching Cubes will make a maximum of 5 triangles per cell.
	const size_t max_triangles_per_mc_cell = 5;

	vertex_3 vertex_interp(const float isovalue, const vertex_3 &p1, const vertex_3 &p2, const float valp1, const float valp2);
	short unsigned int tesselate_grid_cube(const float isovalue, const mc_grid_cube &grid, triangle *const triangles);
};

#endif
