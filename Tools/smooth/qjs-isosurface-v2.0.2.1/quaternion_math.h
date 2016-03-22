// Source code by Shawn Halayka
// Source code is in the public domain

#ifndef QUATERNION_MATH_H
#define QUATERNION_MATH_H


// For console debug purposes only
//#include <iostream>
//using namespace std;


#include "primitives.h"

#include <string>
using std::string;

#include <cmath>


class quaternion_math
{
public:
	quaternion_math(void)
	{
		temp_a_x = temp_a_y = temp_a_z = temp_a_w = 0;
		temp_b_x = temp_b_y = temp_b_z = temp_b_w = 0;
	}

	void add(const quaternion *const qA, const quaternion *const qB, quaternion *const qOut);
	void sub(const quaternion *const qA, const quaternion *const qB, quaternion *const qOut);
	void mul(const quaternion *const qA, const quaternion *const qB, quaternion *const qOut);
	void div(const quaternion *const qA, const quaternion *const qB, quaternion *const qOut);

	void sin(const quaternion *const qA, const quaternion *const qB, quaternion *const qOut);
	void sinh(const quaternion *const qA, const quaternion *const qB, quaternion *const qOut);
	void cos(const quaternion *const qA, const quaternion *const qB, quaternion *const qOut);
	void cosh(const quaternion *const qA, const quaternion *const qB, quaternion *const qOut);
	void tan(const quaternion *const qA, const quaternion *const qB, quaternion *const qOut);
	void tanh(const quaternion *const qA, const quaternion *const qB, quaternion *const qOut);

	void pow(const quaternion *const qA, const quaternion *const qB, quaternion *const qOut);
	void ln(const quaternion *const qA, const quaternion *const qB, quaternion *const qOut);
	void exp(const quaternion *const qA, const quaternion *const qB, quaternion *const qOut);
	void sqrt(const quaternion *const qA, const quaternion *const qB, quaternion *const qOut);
	void inverse(const quaternion *const qA, const quaternion *const qB, quaternion *const qOut);
	void conjugate(const quaternion *const qA, const quaternion *const qB, quaternion *const qOut);

	void copy(const quaternion *const qA, const quaternion *const qB, quaternion *const qOut);
	void copy_masked(const quaternion *const qA, const quaternion *const qB, quaternion *const qOut);
	void swizzle(const quaternion *const qA, const quaternion *const qB, quaternion *const qOut);

	string emit_glsl_function_definitions(void);

protected:
	float temp_a_x, temp_a_y, temp_a_z, temp_a_w;
	float temp_b_x, temp_b_y, temp_b_z, temp_b_w;
};



#endif
