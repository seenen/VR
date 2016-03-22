// Source code by Shawn Halayka
// Source code is in the public domain


#include "quaternion_julia_set.h"



bool quaternion_julia_set::load_parameters_from_file(const char *file_name)
{
	vector<string> tokens;
	parameters_configured = false;

	ifstream config_file(file_name);

	if(config_file.fail())
		return false;

	string line;
	istringstream iss;

	getline(config_file, line);
	if("" == line) return false;
	tokens = stl_str_tok("//", line);
	line = trim_whitespace_string(tokens[0]);
	if("" == line) return false;

	iss.str(line);
	iss	>> x_res;
	y_res = x_res;
	z_res = x_res;

	if(x_res < 1 || x_res > 10000)
		x_res = y_res = z_res = 100;

	cout << "Grid resolution: " << x_res << endl;

	getline(config_file, line);
	if("" == line) return false;
	tokens = stl_str_tok("//", line);
	line = trim_whitespace_string(tokens[0]);
	if("" == line) return false;

	iss.clear();
	iss.str(line);
	iss	>> x_grid_min;
	y_grid_min = x_grid_min;
	z_grid_min = x_grid_min;

	getline(config_file, line);
	if("" == line) return false;
	tokens = stl_str_tok("//", line);
	line = trim_whitespace_string(tokens[0]);
	if("" == line) return false;

	iss.clear();
	iss.str(line);
	iss	>> x_grid_max;
	y_grid_max = x_grid_max;
	z_grid_max = x_grid_max;

	if(x_grid_min == x_grid_max)
	{
		x_grid_min = y_grid_min = z_grid_min = -1.5;
		x_grid_max = y_grid_max = z_grid_max = 1.5;
	}
	else if(x_grid_min > x_grid_max)
	{
		float min = x_grid_max;
		float max = x_grid_min;

		x_grid_min = y_grid_min = z_grid_min = min;
		x_grid_max = y_grid_max = z_grid_max = max;
	}

	cout << "Grid minimum extent: " << x_grid_min << endl;
	cout << "Grid maximum extent: " << x_grid_max << endl;

	getline(config_file, line);
	if("" == line) return false;
	tokens = stl_str_tok("//", line);
	line = trim_whitespace_string(tokens[0]);
	if("" == line) return false;

	iss.clear();
	iss.str(line);
	iss	>> max_iterations;

	cout << "Maximum iterations: " << max_iterations << endl;

	getline(config_file, line);
	if("" == line) return false;
	tokens = stl_str_tok("//", line);
	line = trim_whitespace_string(tokens[0]);
	if("" == line) return false;

	iss.clear();
	iss.str(line);
	iss	>> threshold;

	cout << "Threshold: " << threshold << endl;

	getline(config_file, line);
	if("" == line) return false;
	tokens = stl_str_tok("//", line);
	line = trim_whitespace_string(tokens[0]);
	if("" == line) return false;

	iss.clear();
	iss.str(line);
	iss	>> z_w;

	cout << "Z.w: " << z_w << endl;

	getline(config_file, line);
	if("" == line) return false;
	tokens = stl_str_tok("//", line);
	line = trim_whitespace_string(tokens[0]);
	if("" == line) return false;

	iss.clear();
	iss.str(line);
	iss	>> C.x;

	cout << "C.x: " << C.x << endl;

	getline(config_file, line);
	if("" == line) return false;
	tokens = stl_str_tok("//", line);
	line = trim_whitespace_string(tokens[0]);
	if("" == line) return false;

	iss.clear();
	iss.str(line);
	iss	>> C.y;

	cout << "C.y: " << C.y << endl;

	getline(config_file, line);
	if("" == line) return false;
	tokens = stl_str_tok("//", line);
	line = trim_whitespace_string(tokens[0]);
	if("" == line) return false;

	iss.clear();
	iss.str(line);
	iss	>> C.z;

	cout << "C.z: " << C.z << endl;

	getline(config_file, line);
	if("" == line) return false;
	tokens = stl_str_tok("//", line);
	line = trim_whitespace_string(tokens[0]);
	if("" == line) return false;

	iss.clear();
	iss.str(line);
	iss	>> C.w;

	cout << "C.w: " << C.w << endl;

	getline(config_file, line);
	if("" == line) return false;
	tokens = stl_str_tok("//", line);
	equation_text = tokens[0];

	string error_string;

	if(false == setup_equation_text(equation_text, error_string))
	{
		cout << "Error parsing formula -- " << error_string << endl;
		return false;
	}

	cout << "Equation: " << equation_text << endl;
	cout << endl;

	parameters_configured = true;
	return true;
}

bool quaternion_julia_set::setup_equation_text(const string &src_equation_text, string &error_string)
{
	if(eqparser.setup(src_equation_text, error_string, C))
		return true;
	else
		return false;
}


bool quaternion_julia_set::generate_and_write_isosurface_to_binary_stl_file(const char *file_name, const bool try_gpu, const bool make_border)
{
	if(false == parameters_configured)
	{
		cout << "Error: The quaternion Julia set parameters have not yet been configured." << endl;
		return false;
	}

	if(true == opengl_init_ok && true == try_gpu)
		return quaternion_julia_set::generate_and_write_via_GPU(file_name, make_border);

	return quaternion_julia_set::generate_and_write_via_CPU(file_name, make_border);
}

bool quaternion_julia_set::generate_and_write_via_CPU(const char *file_name, const bool make_border)
{
	time_t start_time;
	time(&start_time);

	cout << "Generating set via CPU ... " << endl;

	ofstream out(file_name, ios_base::binary);

	if(out.fail())
		return false;

	const size_t header_size = 80;
	vector<char> buffer(header_size, 0);
	unsigned int num_triangles = 0; // Must be 4-byte unsigned int.
	vertex_3 normal;

	// Write blank header.
	out.write(reinterpret_cast<const char *>(&(buffer[0])), header_size);

	// Write number of triangles (0, for now... will overwrite this later with the
	// actual number).
	out.write(reinterpret_cast<const char *>(&num_triangles), sizeof(unsigned int));

	// Enough bytes for twelve 4-byte floats plus one 2-byte integer, per triangle.
	const size_t per_triangle_data_size = (12*sizeof(float) + sizeof(short unsigned int));

	// There are (x_res - 1)*(y_res - 1) cells per plane pair.
	// (Note: As an aside, there are z_res - 1 plane pairs).
	const size_t max_plane_pair_data_size = per_triangle_data_size*(x_res - 1)*(y_res - 1)*max_triangles_per_mc_cell;
	buffer.resize(max_plane_pair_data_size, 0);

	// When adding a border, use a value that is "much" greater than the threshold.
	const float border_value = 1.0f + threshold;

	vector<float> xyplane0(x_res*y_res, 0);
	vector<float> xyplane1(x_res*y_res, 0);

	const float x_step_size = (x_grid_max - x_grid_min) / (x_res - 1);
	const float y_step_size = (y_grid_max - y_grid_min) / (y_res - 1);
	const float z_step_size = (z_grid_max - z_grid_min) / (z_res - 1);

	size_t z = 0;

	cout << "Calculating xy-plane " << z + 1 << " of " << z_res << endl;

	quaternion Z(x_grid_min, y_grid_min, z_grid_min, z_w);

	// Calculate 0th xy plane.
	for(size_t x = 0; x < x_res; x++, Z.x += x_step_size)
	{
		Z.y = y_grid_min;

		for(size_t y = 0; y < y_res; y++, Z.y += y_step_size)
		{
			if(true == make_border && (x == 0 || y == 0 || z == 0 || x == x_res - 1 || y == y_res - 1 || z == z_res - 1))
				xyplane0[x*y_res + y] = border_value;
			else
				xyplane0[x*y_res + y] = eqparser.iterate(Z, max_iterations, threshold);
		}
	}

	// Prepare for 1st xy plane.
	z++;
	Z.z += z_step_size;

	// Calculate 1st and subsequent xy planes.
	for(; z < z_res; z++, Z.z += z_step_size)
	{
		Z.x = x_grid_min;

		cout << "Calculating xy-plane " << z + 1 << " of " << z_res << endl;

		for(size_t x = 0; x < x_res; x++, Z.x += x_step_size)
		{
			Z.y = y_grid_min;

			for(size_t y = 0; y < y_res; y++, Z.y += y_step_size)
			{
				if(true == make_border && (x == 0 || y == 0 || z == 0 || x == x_res - 1 || y == y_res - 1 || z == z_res - 1))
					xyplane1[x*y_res + y] = border_value;
				else
					xyplane1[x*y_res + y] = eqparser.iterate(Z, max_iterations, threshold);
			}
		}

		size_t plane_pair_triangle_count = 0;

		// Calculate triangles for the xy-planes corresponding to z - 1 and z by marching cubes.
		tesselate_adjacent_xy_plane_pair(
			xyplane0, xyplane1,
			z - 1,
			buffer,
			plane_pair_triangle_count,
			threshold, // Use threshold as isovalue.
			x_grid_min, x_grid_max, x_res,
			y_grid_min, y_grid_max, y_res,
			z_grid_min, z_grid_max, z_res);

		if(0 < plane_pair_triangle_count)
		{
			out.write(reinterpret_cast<const char *>(&buffer[0]), plane_pair_triangle_count*per_triangle_data_size);
			num_triangles += plane_pair_triangle_count;
		}

		// Swap memory pointers (fast) instead of performing a memory copy (slow).
		xyplane1.swap(xyplane0);
	}

	cout << endl;
	cout << "Wrote " << num_triangles << " triangles to file: " << file_name << endl;

	out.close();
	out.open(file_name, ios_base::out|ios_base::in|ios_base::binary);

	if(out.fail())
		return false;

	// Skip past the header.
	out.seekp(header_size, ios_base::beg);

	// Write actual number of triangles.
	out.write(reinterpret_cast<const char *>(&num_triangles), sizeof(unsigned int));

	out.close();

	time_t end_time;
	time(&end_time);

	cout << "Elapsed time: " << end_time - start_time << " seconds." << endl;

	return true;
}

bool quaternion_julia_set::generate_and_write_via_GPU(const char *file_name, const bool make_border)
{
	time_t start_time;
	time(&start_time);

	cout << "Generating set via GPU ... " << endl;

	GLint shader_handle = 0;
	GLuint fbo_handle = 0;
	GLuint tex_fbo_handle = 0;
	GLuint tex_in_handle = 0;
	GLuint tex_out_handle = 0;
	const GLint tex_in_internal_format = GL_RGB32F_ARB;
	const GLint tex_in_format = GL_RGB;
	const GLint tex_out_internal_format = GL_ALPHA32F_ARB;
	const GLint tex_out_format = GL_ALPHA;
	const GLint var_type = GL_FLOAT;
	const GLint tex_size_x = x_res;
	const GLint tex_size_y = y_res;
	GLint max_tex_size = 0;

	glGetIntegerv(GL_MAX_TEXTURE_SIZE, &max_tex_size);

	if(max_tex_size < tex_size_x || max_tex_size < tex_size_y)
	{
		cout << "GPU max texture size (" << max_tex_size << ") is not large enough. Falling back onto the CPU." << endl;
		return generate_and_write_via_CPU(file_name, make_border);
	}

	string shader_code = eqparser.emit_glsl_shader_code();

	shader_code += "\n// Equation: " + equation_text + '\n';

	ofstream shader_file("fragment.glsl");
	shader_file << shader_code;
	shader_file.close();

	// Load and compile shader.
	if(false == initialize_shader("fragment.glsl", shader_handle))
	{
		cout << "Shader compile error. Falling back onto the CPU." << endl;
		return generate_and_write_via_CPU(file_name, make_border);
	}

	// Allocate OpenGL objects.
	glGenTextures(1, &tex_in_handle);
	glBindTexture(GL_TEXTURE_2D, tex_in_handle);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP);

	glGenTextures(1, &tex_out_handle);
	glBindTexture(GL_TEXTURE_2D, tex_out_handle);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP);

	// Initialize FBO and FBO output texture.
	// These are used to "draw" to a texture instead of to the screen (render-to-texture).
	glGenFramebuffersEXT(1, &fbo_handle);
	glBindFramebufferEXT(GL_FRAMEBUFFER_EXT, fbo_handle);
	glGenTextures(1, &tex_fbo_handle);
	glBindTexture(GL_TEXTURE_2D, tex_fbo_handle);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP);
	glTexImage2D(GL_TEXTURE_2D, 0, tex_out_internal_format, tex_size_x, tex_size_y, 0, tex_out_format, var_type, 0);
	glFramebufferTexture2DEXT(GL_FRAMEBUFFER_EXT, GL_COLOR_ATTACHMENT0_EXT, GL_TEXTURE_2D, tex_fbo_handle, 0);
	GLenum gl_colour_buffer_enum = GL_COLOR_ATTACHMENT0_EXT;
	glDrawBuffers(1, &gl_colour_buffer_enum);

	// Initial setup for "drawing".
	glUseProgram(shader_handle);
	glUniform1i(glGetUniformLocation(shader_handle, "z_xyz"), 0); // Use texture 0.
	glUniform1f(glGetUniformLocation(shader_handle, "z_w"), z_w);
	glUniform4f(glGetUniformLocation(shader_handle, "c"), C.x, C.y, C.z, C.w);
	glUniform1i(glGetUniformLocation(shader_handle, "max_iterations"), max_iterations);
	glUniform1f(glGetUniformLocation(shader_handle, "threshold"), threshold);

	ofstream out(file_name, ios_base::binary);

	if(out.fail())
		return false;

	const size_t header_size = 80;
	vector<char> buffer(header_size, 0);
	unsigned int num_triangles = 0; // Must be 4-byte unsigned int.
	vertex_3 normal;

	// Write blank header.
	out.write(reinterpret_cast<const char *>(&(buffer[0])), header_size);

	// Write number of triangles (0, for now... will overwrite this later with the
	// actual number).
	out.write(reinterpret_cast<const char *>(&num_triangles), sizeof(unsigned int));

	// Enough bytes for twelve 4-byte floats plus one 2-byte integer, per triangle.
	const size_t per_triangle_data_size = (12*sizeof(float) + sizeof(short unsigned int));

	// There are (x_res - 1)*(y_res - 1) cells per plane pair.
	// (Note: As an aside, there are z_res - 1 plane pairs).
	const size_t max_plane_pair_data_size = per_triangle_data_size*(x_res - 1)*(y_res - 1)*max_triangles_per_mc_cell;
	buffer.resize(max_plane_pair_data_size, 0);

	// When adding a border, use a value that is "much" greater than the threshold.
	const float border_value = 1.0f + threshold;

	vector<float> input(x_res*y_res*3, 0); // one float per channel, three channels (RGB)
	vector<float> xyplane0(x_res*y_res, 0);
	vector<float> xyplane1(x_res*y_res, 0);

	const float x_step_size = (x_grid_max - x_grid_min) / (x_res - 1);
	const float y_step_size = (y_grid_max - y_grid_min) / (y_res - 1);
	const float z_step_size = (z_grid_max - z_grid_min) / (z_res - 1);

	size_t z = 0;

	cout << "Calculating xy-plane " << z + 1 << " of " << z_res << endl;

	quaternion Z(x_grid_min, y_grid_min, z_grid_min, z_w);

	// Generate input for 0th xy plane.
	for(size_t x = 0; x < x_res; x++, Z.x += x_step_size)
	{
		Z.y = y_grid_min;

		for(size_t y = 0; y < y_res; y++, Z.y += y_step_size)
		{
			size_t index = 3*(x*x_res + y);
			input[index + 0] = Z.x;
			input[index + 1] = Z.y;
			input[index + 2] = Z.z;
		}
	}

	// Write to GPU memory.
	glActiveTexture(GL_TEXTURE0);
	glBindTexture(GL_TEXTURE_2D, tex_in_handle);
	glTexImage2D(GL_TEXTURE_2D, 0, tex_in_internal_format, tex_size_x, tex_size_y, 0, tex_in_format, var_type, &input[0]);

	// Calculate by "drawing".
	glMatrixMode(GL_PROJECTION);
	glLoadIdentity();
	glOrtho(0, 1, 0, 1, 0, 1);
	glMatrixMode(GL_MODELVIEW);
	glLoadIdentity();
	glViewport(0, 0, tex_size_x, tex_size_y);

	glBegin(GL_QUADS);
		glTexCoord2f(0, 1);	glVertex2f(0, 1);
		glTexCoord2f(0, 0);	glVertex2f(0, 0);
		glTexCoord2f(1, 0);	glVertex2f(1, 0);
		glTexCoord2f(1, 1);	glVertex2f(1, 1);
	glEnd();

	// Read from GPU memory.
	glReadBuffer(GL_COLOR_ATTACHMENT0_EXT);
	glReadPixels(0, 0, tex_size_x, tex_size_y, tex_out_format, var_type, &xyplane0[0]);

	// Add blank border if wanted.
	// Future optimization note: z == 0 by definition, so every element can be blanked out.
	if(true == make_border)
	{
		for(size_t x = 0; x < x_res; x++)
		{
			for(size_t y = 0; y < y_res; y++)
			{
				if(x == 0 || y == 0 || z == 0 || x == x_res - 1 || y == y_res - 1 || z == z_res - 1)
					xyplane0[x*y_res + y] = border_value;
			}
		}
	}

	// Prepare for 1st xy plane.
	z++;
	Z.z += z_step_size;

	// Calculate 1st and subsequent xy planes.
	for(; z < z_res; z++, Z.z += z_step_size)
	{
		Z.x = x_grid_min;

		cout << "Calculating xy-plane " << z + 1 << " of " << z_res << endl;

		// Generate input for 1st and subsequent xy planes.
		for(size_t x = 0; x < x_res; x++, Z.x += x_step_size)
		{
			Z.y = y_grid_min;

			for(size_t y = 0; y < y_res; y++, Z.y += y_step_size)
			{
				size_t index = 3*(x*x_res + y);
				input[index + 0] = Z.x;
				input[index + 1] = Z.y;
				input[index + 2] = Z.z;
			}
		}

		// Write to GPU memory.
		glActiveTexture(GL_TEXTURE0);
		glBindTexture(GL_TEXTURE_2D, tex_in_handle);
		glTexImage2D(GL_TEXTURE_2D, 0, tex_in_internal_format, tex_size_x, tex_size_y, 0, tex_in_format, var_type, &input[0]);

		// Calculate by "drawing".
		glMatrixMode(GL_PROJECTION);
		glLoadIdentity();
		glOrtho(0, 1, 0, 1, 0, 1);
		glMatrixMode(GL_MODELVIEW);
		glLoadIdentity();
		glViewport(0, 0, tex_size_x, tex_size_y);

		glBegin(GL_QUADS);
			glTexCoord2f(0, 1);	glVertex2f(0, 1);
			glTexCoord2f(0, 0);	glVertex2f(0, 0);
			glTexCoord2f(1, 0);	glVertex2f(1, 0);
			glTexCoord2f(1, 1);	glVertex2f(1, 1);
		glEnd();

		// Read from GPU memory.
		glReadBuffer(GL_COLOR_ATTACHMENT0_EXT);
		glReadPixels(0, 0, tex_size_x, tex_size_y, tex_out_format, var_type, &xyplane1[0]);
		
		// Add blank border if wanted.
		if(true == make_border)
		{
			for(size_t x = 0; x < x_res; x++)
			{
				for(size_t y = 0; y < y_res; y++)
				{
					if(x == 0 || y == 0 || z == 0 || x == x_res - 1 || y == y_res - 1 || z == z_res - 1)
						xyplane1[x*y_res + y] = border_value;
				}
			}
		}

		size_t plane_pair_triangle_count = 0;

		// Calculate triangles for the xy-planes corresponding to z - 1 and z by marching cubes.
		// Todo: Look into using a geometry shader to perform the marching cubes calculations.
		tesselate_adjacent_xy_plane_pair(
			xyplane0, xyplane1,
			z - 1,
			buffer,
			plane_pair_triangle_count,
			threshold, // Use threshold as isovalue.
			x_grid_min, x_grid_max, x_res,
			y_grid_min, y_grid_max, y_res,
			z_grid_min, z_grid_max, z_res);

		if(0 < plane_pair_triangle_count)
		{
			out.write(reinterpret_cast<const char *>(&buffer[0]), plane_pair_triangle_count*per_triangle_data_size);
			num_triangles += plane_pair_triangle_count;
		}

		// Swap memory pointers (fast) instead of performing a memory copy (slow).
		xyplane1.swap(xyplane0);
	}

	cout << endl;
	cout << "Wrote " << num_triangles << " triangles to file: " << file_name << endl;

	out.close();
	out.open(file_name, ios_base::out|ios_base::in|ios_base::binary);

	if(out.fail())
		return false;

	// Skip past the header.
	out.seekp(header_size, ios_base::beg);

	// Write actual number of triangles.
	out.write(reinterpret_cast<const char *>(&num_triangles), sizeof(unsigned int));

	out.close();

	time_t end_time;
	time(&end_time);

	cout << "Elapsed time: " << end_time - start_time << " seconds." << endl;

	// Cleanup OpenGL objects.
	glDeleteTextures(1, &tex_in_handle);
	glDeleteTextures(1, &tex_out_handle);
	glDeleteTextures(1, &tex_fbo_handle);
	glDeleteFramebuffersEXT(1, &fbo_handle);
	glUseProgram(0);
	glDeleteProgram(shader_handle);

	return true;
}

void quaternion_julia_set::tesselate_adjacent_xy_plane_pair(const vector<float> &xyplane0, const vector<float> &xyplane1, const size_t z, vector<char> &buffer, size_t &plane_pair_triangle_count, const float isovalue, const float x_grid_min, const float x_grid_max, const size_t x_res, const float y_grid_min, const float y_grid_max, const size_t y_res, const float z_grid_min, const float z_grid_max, const size_t z_res)
{
	const float x_step_size = (x_grid_max - x_grid_min) / (x_res - 1);
	const float y_step_size = (y_grid_max - y_grid_min) / (y_res - 1);
	const float z_step_size = (z_grid_max - z_grid_min) / (z_res - 1);

	char *cp = &buffer[0];
	vertex_3 normal;

	for(size_t x = 0; x < x_res - 1; x++)
	{
		for(size_t y = 0; y < y_res - 1; y++)
		{
			mc_grid_cube temp_cube;

			size_t x_offset = 0;
			size_t y_offset = 0;
			size_t z_offset = 0;

			// Setup vertex 0
			x_offset = 0;
			y_offset = 0;
			z_offset = 0;
			temp_cube.vertex[0].x = x_grid_min + ((x+x_offset) * x_step_size);
			temp_cube.vertex[0].y = y_grid_min + ((y+y_offset) * y_step_size);
			temp_cube.vertex[0].z = z_grid_min + ((z+z_offset) * z_step_size);

			if(0 == z_offset)
				temp_cube.value[0] = xyplane0[(x + x_offset)*y_res + (y + y_offset)];
			else
				temp_cube.value[0] = xyplane1[(x + x_offset)*y_res + (y + y_offset)];

			// Setup vertex 1
			x_offset = 1;
			y_offset = 0;
			z_offset = 0;
			temp_cube.vertex[1].x = x_grid_min + ((x+x_offset) * x_step_size);
			temp_cube.vertex[1].y = y_grid_min + ((y+y_offset) * y_step_size);
			temp_cube.vertex[1].z = z_grid_min + ((z+z_offset) * z_step_size);

			if(0 == z_offset)
				temp_cube.value[1] = xyplane0[(x + x_offset)*y_res + (y + y_offset)];
			else
				temp_cube.value[1] = xyplane1[(x + x_offset)*y_res + (y + y_offset)];

			// Setup vertex 2
			x_offset = 1;
			y_offset = 0;
			z_offset = 1;
			temp_cube.vertex[2].x = x_grid_min + ((x+x_offset) * x_step_size);
			temp_cube.vertex[2].y = y_grid_min + ((y+y_offset) * y_step_size);
			temp_cube.vertex[2].z = z_grid_min + ((z+z_offset) * z_step_size);

			if(0 == z_offset)
				temp_cube.value[2] = xyplane0[(x + x_offset)*y_res + (y + y_offset)];
			else
				temp_cube.value[2] = xyplane1[(x + x_offset)*y_res + (y + y_offset)];

			// Setup vertex 3
			x_offset = 0; 
			y_offset = 0;
			z_offset = 1;
			temp_cube.vertex[3].x = x_grid_min + ((x+x_offset) * x_step_size);
			temp_cube.vertex[3].y = y_grid_min + ((y+y_offset) * y_step_size);
			temp_cube.vertex[3].z = z_grid_min + ((z+z_offset) * z_step_size);

			if(0 == z_offset)
				temp_cube.value[3] = xyplane0[(x + x_offset)*y_res + (y + y_offset)];
			else
				temp_cube.value[3] = xyplane1[(x + x_offset)*y_res + (y + y_offset)];

			// Setup vertex 4
			x_offset = 0;
			y_offset = 1;
			z_offset = 0;
			temp_cube.vertex[4].x = x_grid_min + ((x+x_offset) * x_step_size);
			temp_cube.vertex[4].y = y_grid_min + ((y+y_offset) * y_step_size);
			temp_cube.vertex[4].z = z_grid_min + ((z+z_offset) * z_step_size);

			if(0 == z_offset)
				temp_cube.value[4] = xyplane0[(x + x_offset)*y_res + (y + y_offset)];
			else
				temp_cube.value[4] = xyplane1[(x + x_offset)*y_res + (y + y_offset)];

			// Setup vertex 5
			x_offset = 1;
			y_offset = 1;
			z_offset = 0;
			temp_cube.vertex[5].x = x_grid_min + ((x+x_offset) * x_step_size);
			temp_cube.vertex[5].y = y_grid_min + ((y+y_offset) * y_step_size);
			temp_cube.vertex[5].z = z_grid_min + ((z+z_offset) * z_step_size);

			if(0 == z_offset)
				temp_cube.value[5] = xyplane0[(x + x_offset)*y_res + (y + y_offset)];
			else
				temp_cube.value[5] = xyplane1[(x + x_offset)*y_res + (y + y_offset)];

			// Setup vertex 6
			x_offset = 1;
			y_offset = 1;
			z_offset = 1;
			temp_cube.vertex[6].x = x_grid_min + ((x+x_offset) * x_step_size);
			temp_cube.vertex[6].y = y_grid_min + ((y+y_offset) * y_step_size);
			temp_cube.vertex[6].z = z_grid_min + ((z+z_offset) * z_step_size);

			if(0 == z_offset)
				temp_cube.value[6] = xyplane0[(x + x_offset)*y_res + (y + y_offset)];
			else
				temp_cube.value[6] = xyplane1[(x + x_offset)*y_res + (y + y_offset)];

			// Setup vertex 7
			x_offset = 0;
			y_offset = 1;
			z_offset = 1;
			temp_cube.vertex[7].x = x_grid_min + ((x+x_offset) * x_step_size);
			temp_cube.vertex[7].y = y_grid_min + ((y+y_offset) * y_step_size);
			temp_cube.vertex[7].z = z_grid_min + ((z+z_offset) * z_step_size);

			if(0 == z_offset)
				temp_cube.value[7] = xyplane0[(x + x_offset)*y_res + (y + y_offset)];
			else
				temp_cube.value[7] = xyplane1[(x + x_offset)*y_res + (y + y_offset)];

			// Generate triangles from cube.
			static triangle temp_triangle_array[max_triangles_per_mc_cell];

			short unsigned int number_of_triangles_generated = tesselate_grid_cube(isovalue, temp_cube, temp_triangle_array);

			plane_pair_triangle_count += number_of_triangles_generated;

			for(short unsigned int i = 0; i < number_of_triangles_generated; i++)
			{
				vertex_3 v0 = temp_triangle_array[i].vertex[1] - temp_triangle_array[i].vertex[0];
				vertex_3 v1 = temp_triangle_array[i].vertex[2] - temp_triangle_array[i].vertex[0];
				normal = v0.cross(v1);
				normal.normalize();

				memcpy(cp, &normal.x, sizeof(float)); cp += sizeof(float);
				memcpy(cp, &normal.y, sizeof(float)); cp += sizeof(float);
				memcpy(cp, &normal.z, sizeof(float)); cp += sizeof(float);

				memcpy(cp, &temp_triangle_array[i].vertex[0].x, sizeof(float)); cp += sizeof(float);
				memcpy(cp, &temp_triangle_array[i].vertex[0].y, sizeof(float)); cp += sizeof(float);
				memcpy(cp, &temp_triangle_array[i].vertex[0].z, sizeof(float)); cp += sizeof(float);
				memcpy(cp, &temp_triangle_array[i].vertex[1].x, sizeof(float)); cp += sizeof(float);
				memcpy(cp, &temp_triangle_array[i].vertex[1].y, sizeof(float)); cp += sizeof(float);
				memcpy(cp, &temp_triangle_array[i].vertex[1].z, sizeof(float)); cp += sizeof(float);
				memcpy(cp, &temp_triangle_array[i].vertex[2].x, sizeof(float)); cp += sizeof(float);
				memcpy(cp, &temp_triangle_array[i].vertex[2].y, sizeof(float)); cp += sizeof(float);
				memcpy(cp, &temp_triangle_array[i].vertex[2].z, sizeof(float)); cp += sizeof(float);

				cp += sizeof(short unsigned int);
			}
		}
	}
}

bool quaternion_julia_set::initialize_shader(const char *frag_filename, GLint &shader)
{
	// Open shader file.
	ifstream frag_file(frag_filename);
	string frag_source, temp_string;

	if(!frag_file.is_open())
	{
		cout << "Couldn't load shader file: " << frag_filename << endl;
		return false;
	}

	// Copy the entire file into a single char*-compatible buffer.
	while(getline(frag_file, temp_string))
	{
		frag_source += temp_string;
		frag_source += '\n';
	}

	frag_file.close();

	// Compile shader.
	const char *cch = 0;
	GLint status = GL_FALSE;
	GLint frag = glCreateShader(GL_FRAGMENT_SHADER);

	glShaderSource(frag, 1, &(cch = frag_source.c_str()), 0);
	glCompileShader(frag);
	glGetShaderiv(frag, GL_COMPILE_STATUS, &status);

	if(GL_FALSE == status)
	{
		cout << "Fragment shader compile error." << endl;
		vector<GLchar> buf(4096, '\0');
		glGetShaderInfoLog(frag, 4095, 0, &buf[0]);

		for(size_t i = 0; i < buf.size(); i++)
			if(0 != buf[i])
				cout << buf[i];

		cout << endl;

		return false;
	}

	// Link to get final shader.
	shader = glCreateProgram();
	glAttachShader(shader, frag);
	glLinkProgram(shader);
	glGetProgramiv(shader, GL_LINK_STATUS, &status);

	if(GL_FALSE == status)
	{
		cout << "Program link error." << endl;
		vector<GLchar> buf(4096, '\0');
		glGetShaderInfoLog(shader, 4095, 0, &buf[0]);

		for(size_t i = 0; i < buf.size(); i++)
			if(0 != buf[i])
				cout << buf[i];

		cout << endl;

		glDetachShader(shader, frag);
		glDeleteShader(frag);
		return false;
	}

	// Cleanup.
	glDetachShader(shader, frag);
	glDeleteShader(frag);

	return true;
}
