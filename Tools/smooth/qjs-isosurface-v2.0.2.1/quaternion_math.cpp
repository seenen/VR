// Source code by Shawn Halayka
// Source code is in the public domain


#include "quaternion_math.h"

void quaternion_math::add(const quaternion *const qA, const quaternion *const qB, quaternion *const qOut)
{
	qOut->x = qA->x + qB->x;
	qOut->y = qA->y + qB->y;
	qOut->z = qA->z + qB->z;
	qOut->w = qA->w + qB->w;
}

void quaternion_math::sub(const quaternion *const qA, const quaternion *const qB, quaternion *const qOut)
{
	qOut->x = qA->x - qB->x;
	qOut->y = qA->y - qB->y;
	qOut->z = qA->z - qB->z;
	qOut->w = qA->w - qB->w;
}

void quaternion_math::mul(const quaternion *const qA, const quaternion *const qB, quaternion *const qOut)
{
	// in case qA and qOut point to the same variable...
	temp_a_x = qA->x;
	temp_a_y = qA->y;
	temp_a_z = qA->z;
	temp_a_w = qA->w;

	temp_b_x = qB->x;
	temp_b_y = qB->y;
	temp_b_z = qB->z;
	temp_b_w = qB->w;

	// perform multiply
	qOut->x = temp_a_x*temp_b_x - temp_a_y*temp_b_y - temp_a_z*temp_b_z - temp_a_w*temp_b_w;
	qOut->y = temp_a_x*temp_b_y + temp_a_y*temp_b_x + temp_a_z*temp_b_w - temp_a_w*temp_b_z;
	qOut->z = temp_a_x*temp_b_z - temp_a_y*temp_b_w + temp_a_z*temp_b_x + temp_a_w*temp_b_y;
	qOut->w = temp_a_x*temp_b_w + temp_a_y*temp_b_z - temp_a_z*temp_b_y + temp_a_w*temp_b_x;
}

void quaternion_math::div(const quaternion *const qA, const quaternion *const qB, quaternion *const qOut)
{
	// c = a/b

	// c = inv(b) * a
	// inv(b) = conjugate(b) / norm(b)
	// c = (conjugate(b) / norm(b)) * a

	float temp_b_norm = qB->x*qB->x + qB->y*qB->y + qB->z*qB->z + qB->w*qB->w;

	temp_b_x =  qB->x;
	temp_b_y = -qB->y;
	temp_b_z = -qB->z;
	temp_b_w = -qB->w;

	temp_b_x /= temp_b_norm;
	temp_b_y /= temp_b_norm;
	temp_b_z /= temp_b_norm;
	temp_b_w /= temp_b_norm;

	temp_a_x = qA->x;
	temp_a_y = qA->y;
	temp_a_z = qA->z;
	temp_a_w = qA->w;

	qOut->x = temp_b_x*temp_a_x - temp_b_y*temp_a_y - temp_b_z*temp_a_z - temp_b_w*temp_a_w;
	qOut->y = temp_b_x*temp_a_y + temp_b_y*temp_a_x + temp_b_z*temp_a_w - temp_b_w*temp_a_z;
	qOut->z = temp_b_x*temp_a_z - temp_b_y*temp_a_w + temp_b_z*temp_a_x + temp_b_w*temp_a_y;
	qOut->w = temp_b_x*temp_a_w + temp_b_y*temp_a_z - temp_b_z*temp_a_y + temp_b_w*temp_a_x;
}

void quaternion_math::sin(const quaternion *const qA, const quaternion *const qB, quaternion *const qOut)
{
	//|V| = sqrt(v.x^2 + v.y^2 + v.z^2)
	//sin(q) = sin(float) * cosh(|V|), cos(float) * sinh(|V|) * V / |V|

	float mag_vector = std::sqrt(qA->y*qA->y + qA->z*qA->z + qA->w*qA->w);

	temp_a_x = qA->x;

	qOut->x = std::sin(temp_a_x) * std::cosh(mag_vector);
	qOut->y = std::cos(temp_a_x) * std::sinh(mag_vector) * qA->y / mag_vector;
	qOut->z = std::cos(temp_a_x) * std::sinh(mag_vector) * qA->z / mag_vector;
	qOut->w = std::cos(temp_a_x) * std::sinh(mag_vector) * qA->w / mag_vector;
}

void quaternion_math::sinh(const quaternion *const qA, const quaternion *const qB, quaternion *const qOut)
{
	//|V| = sqrt(v.x^2 + v.y^2 + v.z^2)
	//sin(q) = sin(float) * cosh(|V|), cos(float) * sinh(|V|) * V / |V|

	float mag_vector = std::sqrt(qA->y*qA->y + qA->z*qA->z + qA->w*qA->w);

	temp_a_x = qA->x;

	qOut->x = std::sinh(temp_a_x) * std::cos(mag_vector);
	qOut->y = std::cosh(temp_a_x) * std::sin(mag_vector) * qA->y / mag_vector;
	qOut->z = std::cosh(temp_a_x) * std::sin(mag_vector) * qA->z / mag_vector;
	qOut->w = std::cosh(temp_a_x) * std::sin(mag_vector) * qA->w / mag_vector;
}

void quaternion_math::cos(const quaternion *const qA, const quaternion *const qB, quaternion *const qOut)
{
	//|V| = sqrt(v.x^2 + v.y^2 + v.z^2)
	//cos(q) = cos(float) * cosh(|V|), -sin(float) * sinh(|V|) * V / |V|

	float mag_vector = std::sqrt(qA->y*qA->y + qA->z*qA->z + qA->w*qA->w);

	temp_a_x = qA->x;

	qOut->x =  std::cos(temp_a_x) * std::cosh(mag_vector);
	qOut->y = -std::sin(temp_a_x) * std::sinh(mag_vector) * qA->y / mag_vector;
	qOut->z = -std::sin(temp_a_x) * std::sinh(mag_vector) * qA->z / mag_vector;
	qOut->w = -std::sin(temp_a_x) * std::sinh(mag_vector) * qA->w / mag_vector;
}

void quaternion_math::cosh(const quaternion *const qA, const quaternion *const qB, quaternion *const qOut)
{
	//|V| = sqrt(v.x^2 + v.y^2 + v.z^2)
	//cos(q) = cos(float) * cosh(|V|), -sin(float) * sinh(|V|) * V / |V|

	float mag_vector = std::sqrt(qA->y*qA->y + qA->z*qA->z + qA->w*qA->w);

	temp_a_x = qA->x;

	qOut->x = std::cosh(temp_a_x) * std::cos(mag_vector);
	qOut->y = std::sinh(temp_a_x) * std::sin(mag_vector) * qA->y / mag_vector;
	qOut->z = std::sinh(temp_a_x) * std::sin(mag_vector) * qA->z / mag_vector;
	qOut->w = std::sinh(temp_a_x) * std::sin(mag_vector) * qA->w / mag_vector;
}

void quaternion_math::tan(const quaternion *const qA, const quaternion *const qB, quaternion *const qOut)
{
	quaternion sin_quat;
	quaternion cos_quat;

	sin(qA, 0, &sin_quat);
	cos(qA, 0, &cos_quat);

	div(&sin_quat, &cos_quat, qOut);
}

void quaternion_math::tanh(const quaternion *const qA, const quaternion *const qB, quaternion *const qOut)
{
	quaternion sinh_quat;
	quaternion cosh_quat;

	sinh(qA, 0, &sinh_quat);
	cosh(qA, 0, &cosh_quat);

	div(&sinh_quat, &cosh_quat, qOut);
}

void quaternion_math::pow(const quaternion *const qA, const quaternion *const qB, quaternion *const qOut)
{
	long unsigned int exp = static_cast<long unsigned int>(fabs(qB->x));

	if(0 == exp)
	{
		qOut->x = 1;
		qOut->y = 0;
		qOut->z = 0;
		qOut->w = 0;
	}
	else if(1 == exp)
	{
		qOut->x = qA->x;
		qOut->y = qA->y;
		qOut->z = qA->z;
		qOut->w = qA->w;
	}
	else
	{
		static quaternion temp_quat;
		temp_quat = *qOut = *qA;

		for(long unsigned int i = 1; i < exp; i++)
		{
			mul(qOut, &temp_quat, qOut);
		}
	}
}

void quaternion_math::ln(const quaternion *const qA, const quaternion *const qB, quaternion *const qOut)
{
	temp_a_x = qA->x;
	temp_a_y = qA->y;
	temp_a_z = qA->z;
	temp_a_w = qA->w;

	float quat_length = std::sqrt(temp_a_x*temp_a_x + temp_a_y*temp_a_y + temp_a_z*temp_a_z + temp_a_w*temp_a_w);

	// make into unit quaternion_t if necessary
	if(1 != quat_length)
	{
		temp_a_x /= quat_length;
		temp_a_y /= quat_length;
		temp_a_z /= quat_length;
		temp_a_w /= quat_length;
	}

	//ln(q) = 0.5 * ln(float^2 + V.V), atan2(|V|, float) * V / |V|
	float vector_dot_prod = temp_a_y*temp_a_y + temp_a_z*temp_a_z + temp_a_w*temp_a_w;

	float vector_length = std::sqrt(vector_dot_prod);

	qOut->x = 0.5f * std::log(temp_a_x*temp_a_x + vector_dot_prod);
	qOut->y = (std::atan2(vector_length, temp_a_x) * temp_a_y) / vector_length;
	qOut->z = (std::atan2(vector_length, temp_a_x) * temp_a_z) / vector_length;
	qOut->w = (std::atan2(vector_length, temp_a_x) * temp_a_w) / vector_length;
}

void quaternion_math::exp(const quaternion *const qA, const quaternion *const qB, quaternion *const qOut)
{
	//exp(q) = exp(float) * cos(|V|), exp(float) * sin(|V|) * V / |V|

	float mag_vector = std::sqrt(qA->y*qA->y + qA->z*qA->z + qA->w*qA->w);

	temp_a_x = qA->x;

	qOut->x = std::exp(temp_a_x) * std::cos(mag_vector);
	qOut->y = std::exp(temp_a_x) * std::sin(mag_vector) * qA->y / mag_vector;
	qOut->z = std::exp(temp_a_x) * std::sin(mag_vector) * qA->z / mag_vector;
	qOut->w = std::exp(temp_a_x) * std::sin(mag_vector) * qA->w / mag_vector;
}

void quaternion_math::sqrt(const quaternion *const qA, const quaternion *const qB, quaternion *const qOut)
{
	if(qA->y == 0 && qA->z == 0 && qA->w == 0)
	{
		if(qA->x >= 0)
		{
			qOut->x = std::sqrt(qA->x);
			qOut->y = 0;
			qOut->z = 0;
			qOut->w = 0;
		}
		else
		{
			qOut->x = std::sqrt(-qA->x); //0;
			qOut->y = 0; //std::sqrt(-qA->x);
			qOut->z = 0;
			qOut->w = 0;
		}
	}
	else
	{
		temp_a_x = std::sqrt(qA->y*qA->y + qA->z*qA->z + qA->w*qA->w);

		if(qA->x >= 0)
		{
			float m = std::sqrt(0.5f * (std::sqrt(qA->x*qA->x + temp_a_x*temp_a_x) + qA->x));
			float l = temp_a_x / (2 * m);
			float t = l / temp_a_x;

			qOut->x = m;
			qOut->y = qA->y * t;
			qOut->z = qA->z * t;
			qOut->w = qA->w * t;
		}
		else
		{
			float l = std::sqrt(0.5f * (std::sqrt(qA->x*qA->x + temp_a_x*temp_a_x) - qA->x));
			float m = temp_a_x / (2 * l);
			float t = l / temp_a_x;

			qOut->x = m;
			qOut->y = qA->y * t;
			qOut->z = qA->z * t;
			qOut->w = qA->w * t;
		}
	}
}

void quaternion_math::inverse(const quaternion *const qA, const quaternion *const qB, quaternion *const qOut)
{
	// inv(a) = conjugate(a) / norm(a)

	float temp_a_norm = qA->x*qA->x + qA->y*qA->y + qA->z*qA->z + qA->w*qA->w;

	temp_a_x =  qA->x;
	temp_a_y = -qA->y;
	temp_a_z = -qA->z;
	temp_a_w = -qA->w;

	qOut->x = temp_a_x / temp_a_norm;
	qOut->y = temp_a_y / temp_a_norm;
	qOut->z = temp_a_z / temp_a_norm;
	qOut->w = temp_a_w / temp_a_norm;
}

void quaternion_math::conjugate(const quaternion *const qA, const quaternion *const qB, quaternion *const qOut)
{
	qOut->x =  qA->x;
	qOut->y = -qA->y;
	qOut->z = -qA->z;
	qOut->w = -qA->w;
}

void quaternion_math::copy(const quaternion *const qA, const quaternion *const qB, quaternion *const qOut)
{
	qOut->x = qA->x;
	qOut->y = qA->y;
	qOut->z = qA->z;
	qOut->w = qA->w;
}

void quaternion_math::copy_masked(const quaternion *const qA, const quaternion *const qB, quaternion *const qOut)
{
	temp_a_x = qA->x;
	temp_a_y = qA->y;
	temp_a_z = qA->z;
	temp_a_w = qA->w;

	if(qB->x != 0.0)
	{
		if(qB->x == 1.0)
			qOut->x = temp_a_x;
		else if(qB->x == -1.0)
			qOut->x = -temp_a_x;
		else if(qB->x == 2.0)
			qOut->x = temp_a_y;
		else if(qB->x == -2.0)
			qOut->x = -temp_a_y;
		else if(qB->x == 3.0)
			qOut->x = temp_a_z;
		else if(qB->x == -3.0)
			qOut->x = -temp_a_z;
		else if(qB->x == 4.0)
			qOut->x = temp_a_w;
		else if(qB->x == -4.0)
			qOut->x = -temp_a_w;
	}

	if(qB->y != 0.0)
	{
		if(qB->y == 1.0)
			qOut->y = temp_a_x;
		else if(qB->y == -1.0)
			qOut->y = -temp_a_x;
		else if(qB->y == 2.0)
			qOut->y = temp_a_y;
		else if(qB->y == -2.0)
			qOut->y = -temp_a_y;
		else if(qB->y == 3.0)
			qOut->y = temp_a_z;
		else if(qB->y == -3.0)
			qOut->y = -temp_a_z;
		else if(qB->y == 4.0)
			qOut->y = temp_a_w;
		else if(qB->y == -4.0)
			qOut->y = -temp_a_w;
	}

	if(qB->z != 0.0)
	{
		if(qB->z == 1.0)
			qOut->z = temp_a_x;
		else if(qB->z == -1.0)
			qOut->z = -temp_a_x;
		else if(qB->z == 2.0)
			qOut->z = temp_a_y;
		else if(qB->z == -2.0)
			qOut->z = -temp_a_y;
		else if(qB->z == 3.0)
			qOut->z = temp_a_z;
		else if(qB->z == -3.0)
			qOut->z = -temp_a_z;
		else if(qB->z == 4.0)
			qOut->z = temp_a_w;
		else if(qB->z == -4.0)
			qOut->z = -temp_a_w;
	}

	if(qB->w != 0.0)
	{
		if(qB->w == 1.0)
			qOut->w = temp_a_x;
		else if(qB->w == -1.0)
			qOut->w = -temp_a_x;
		else if(qB->w == 2.0)
			qOut->w = temp_a_y;
		else if(qB->w == -2.0)
			qOut->w = -temp_a_y;
		else if(qB->w == 3.0)
			qOut->w = temp_a_z;
		else if(qB->w == -3.0)
			qOut->w = -temp_a_z;
		else if(qB->w == 4.0)
			qOut->w = temp_a_w;
		else if(qB->w == -4.0)
			qOut->w = -temp_a_w;
	}

}

void quaternion_math::swizzle(const quaternion *const qA, const quaternion *const qB, quaternion *const qOut)
{
	temp_a_x = qA->x;
	temp_a_y = qA->y;
	temp_a_z = qA->z;
	temp_a_w = qA->w;

	if(qB->x == 1.0)
		qOut->x = temp_a_x;
	else if(qB->x == 2.0)
		qOut->x = temp_a_y;
	else if(qB->x == 3.0)
		qOut->x = temp_a_z;
	else
		qOut->x = temp_a_w;

	if(qB->y == 1.0)
		qOut->y = temp_a_x;
	else if(qB->y == 2.0)
		qOut->y = temp_a_y;
	else if(qB->y == 3.0)
		qOut->y = temp_a_z;
	else
		qOut->y = temp_a_w;

	if(qB->z == 1.0)
		qOut->z = temp_a_x;
	else if(qB->z == 2.0)
		qOut->z = temp_a_y;
	else if(qB->z == 3.0)
		qOut->z = temp_a_z;
	else
		qOut->z = temp_a_w;

	if(qB->w == 1.0)
		qOut->w = temp_a_x;
	else if(qB->w == 2.0)
		qOut->w = temp_a_y;
	else if(qB->w == 3.0)
		qOut->w = temp_a_z;
	else
		qOut->w = temp_a_w;
}

string quaternion_math::emit_glsl_function_definitions(void)
{
	string code;

	code += "// A decent GLSL compiler should optimize this out.\n";
	code += "vec4 qadd(vec4 qa, vec4 qb)\n";
	code += "{\n";
	code += "    return qa + qb;\n";
	code += "}\n";
	code += "\n";
	code += "// A decent GLSL compiler should optimize this out.\n";
	code += "vec4 qsub(vec4 qa, vec4 qb)\n";
	code += "{\n";
	code += "    return qa - qb;\n";
	code += "}\n";
	code += "\n";
	code += "vec4 qmul(vec4 qa, vec4 qb)\n";
	code += "{\n";
	code += "   vec4 qout;\n";
	code += "   qout.x = qa.x*qb.x - dot(qa.yzw, qb.yzw);\n";
	code += "   qout.yzw = qa.x*qb.yzw + qb.x*qa.yzw + cross(qa.yzw, qb.yzw);\n";
	code += "\n";
	code += "   return qout;\n";
	code += "}\n";
	code += "\n";
	code += "vec4 qdiv(vec4 qa, vec4 qb)\n";
	code += "{\n";
	code += "    float d = dot(qb, qb);\n";
	code += "\n";
	code += "    vec4 temp_b = qb;\n";
	code += "    temp_b.yzw = -temp_b.yzw;\n";
	code += "\n";
	code += "    return qmul(temp_b / d, qa);\n";
	code += "}\n";
	code += "\n";
	code += "vec4 qsin(vec4 qa)\n";
	code += "{\n";
	code += "    float mag_vector = length(qa.yzw);\n";
	code += "\n";
	code += "    vec4 qout;\n";
	code += "    qout.x = sin(qa.x) * cosh(mag_vector);\n";
	code += "    qout.yzw = cos(qa.x) * sinh(mag_vector) * qa.yzw / mag_vector;\n";
	code += "\n";
	code += "    return qout;\n";
	code += "}\n";
	code += "\n";
	code += "vec4 qsinh(vec4 qa)\n";
	code += "{\n";
	code += "    float mag_vector = length(qa.yzw);\n";
	code += "\n";
	code += "    vec4 qout;\n";
	code += "    qout.x = sinh(qa.x) * cos(mag_vector);\n";
	code += "    qout.yzw = cosh(qa.x) * sin(mag_vector) * qa.yzw / mag_vector;\n";
	code += "\n";
	code += "    return qout;\n";
	code += "}\n";
	code += "\n";
	code += "vec4 qcos(vec4 qa)\n";
	code += "{\n";
	code += "    float mag_vector = length(qa.yzw);\n";
	code += "\n";
	code += "    vec4 qout;\n";
	code += "    qout.x = cos(qa.x) * cosh(mag_vector);\n";
	code += "    qout.yzw = -sin(qa.x) * sinh(mag_vector) * qa.yzw / mag_vector;\n";
	code += "\n";
	code += "    return qout;\n";
	code += "}\n";
	code += "\n";
	code += "vec4 qcosh(vec4 qa)\n";
	code += "{\n";
	code += "    float mag_vector = length(qa.yzw);\n";
	code += "\n";
	code += "    vec4 qout;\n";
	code += "    qout.x = cosh(qa.x) * cos(mag_vector);\n";
	code += "    qout.yzw = sinh(qa.x) * sin(mag_vector) * qa.yzw / mag_vector;\n";
	code += "\n";
	code += "    return qout;\n";
	code += "}\n";
	code += "\n";
	code += "vec4 qtan(vec4 qa)\n";
	code += "{\n";
	code += "    return qdiv(qsin(qa), qcos(qa));\n";
	code += "}\n";
	code += "\n";
	code += "vec4 qtanh(vec4 qa)\n";
	code += "{\n";
	code += "    return qdiv(qsinh(qa), qcosh(qa));\n";
	code += "}\n";
	code += "\n";
	code += "vec4 qpow(vec4 qa, vec4 qb)\n";
	code += "{\n";
	code += "    highp int pow_exponent = int(abs(qb.x));\n";
	code += "    vec4 qout = qa;\n";
	code += "\n";
	code += "    if(pow_exponent == 0)\n";
	code += "    {\n";
	code += "        qout.x = 1;\n";
	code += "        qout.y = 0;\n";
	code += "        qout.z = 0;\n";
	code += "        qout.w = 0;\n";
	code += "    }\n";
	code += "    else\n";
	code += "    {\n";
	code += "        for(highp int i = 1; i < pow_exponent; i++)\n";
	code += "            qout = qmul(qout, qa);\n";
	code += "    }\n";
	code += "\n";
	code += "    return qout;\n";
	code += "}\n";
	code += "\n";
	code += "vec4 qln(vec4 qa)\n";
	code += "{\n";
	code += "    qa = normalize(qa);\n";
	code += "\n";
	code += "    float dot_vector = dot(qa.yzw, qa.yzw);\n";
	code += "    float mag_vector = sqrt(dot_vector);\n";
	code += "\n";
	code += "    vec4 qout;\n";
	code += "\n";
	code += "    qout.x = 0.5f * log(qa.x*qa.x + dot_vector);\n";
	code += "    qout.yzw = (atan(mag_vector, qa.x) * qa.yzw) / mag_vector;\n";
	code += "\n";
	code += "    return qout;\n";
	code += "}\n";
	code += "\n";
	code += "vec4 qexp(vec4 qa)\n";
	code += "{\n";
	code += "    float mag_vector = length(qa.yzw);\n";
	code += "\n";
	code += "    vec4 qout;\n";
	code += "    qout.x = exp(qa.x) * cos(mag_vector);\n";
	code += "    qout.yzw = exp(qa.x) * sin(mag_vector) * qa.yzw / mag_vector;\n";
	code += "\n";
	code += "    return qout;\n";
	code += "}\n";
	code += "\n";
	code += "vec4 qsqrt(vec4 qa)\n";
	code += "{\n";
	code += "    vec4 qout;\n";
	code += "\n";
	code += "    if(qa.y == 0 && qa.z == 0 && qa.w == 0)\n";
	code += "    {\n";
	code += "        if(qa.x >= 0)\n";
	code += "        {\n";
	code += "            qout.x = sqrt(qa.x);\n";
	code += "            qout.y = 0;\n";
	code += "            qout.z = 0;\n";
	code += "            qout.w = 0;\n";
	code += "        }\n";
	code += "        else\n";
	code += "        {\n";
	code += "            qout.x = sqrt(-qa.x);\n";
	code += "            qout.y = 0;\n";
	code += "            qout.z = 0;\n";
	code += "            qout.w = 0;\n";
	code += "        }\n";
	code += "    }\n";
	code += "    else\n";
	code += "    {\n";
	code += "        float mag_vector = length(qa.yzw);\n";
	code += "\n";
	code += "        if(qa.x >= 0)\n";
	code += "        {\n";
	code += "            float m = sqrt(0.5f * (sqrt(qa.x*qa.x + mag_vector*mag_vector) + qa.x));\n";
	code += "            float l = mag_vector / (2 * m);\n";
	code += "            float t = l / mag_vector;\n";
	code += "\n";
	code += "            qout.x = m;\n";
	code += "            qout.y = qa.y * t;\n";
	code += "            qout.z = qa.z * t;\n";
	code += "            qout.w = qa.w * t;\n";
	code += "        }\n";
	code += "        else\n";
	code += "        {\n";
	code += "            float l = sqrt(0.5f * (sqrt(qa.x*qa.x + mag_vector*mag_vector) - qa.x));\n";
	code += "            float m = mag_vector / (2 * l);\n";
	code += "            float t = l / mag_vector;\n";
	code += "\n";
	code += "            qout.x = m;\n";
	code += "            qout.y = qa.y * t;\n";
	code += "            qout.z = qa.z * t;\n";
	code += "            qout.w = qa.w * t;\n";
	code += "        }\n";
	code += "    }\n";
	code += "\n";
	code += "    return qout;\n";
	code += "}\n";
	code += "\n";
	code += "vec4 qinverse(vec4 qa)\n";
	code += "{\n";
	code += "    vec4 qout = qa;\n";
	code += "    qout.yzw = -qout.yzw;\n";
	code += "\n";
	code += "    return qout / dot(qa, qa);\n";
	code += "}\n";
	code += "\n";
	code += "vec4 qconjugate(vec4 qa)\n";
	code += "{\n";
	code += "    qa.yzw = -qa.yzw;\n";
	code += "    return qa;\n";
	code += "}\n";
	code += "\n";
	code += "// A decent GLSL compiler should optimize this out.\n";
	code += "vec4 qcopy(vec4 qa)\n";
	code += "{\n";
	code += "    return qa;\n";
	code += "}\n";
	code += "\n";
	code += "vec4 qcopy_masked(vec4 qa, vec4 qb)\n";
	code += "{\n";
	code += "    vec4 qout;\n";
	code += "\n";
	code += "    if(qb.x != 0.0)\n";
	code += "    {\n";
	code += "        if(qb.x == 1.0)\n";
	code += "            qout.x = qa.x;\n";
	code += "        else if(qb.x == -1.0)\n";
	code += "            qout.x = -qa.x;\n";
	code += "        else if(qb.x == 2.0)\n";
	code += "            qout.x = qa.y;\n";
	code += "        else if(qb.x == -2.0)\n";
	code += "            qout.x = -qa.y;\n";
	code += "        else if(qb.x == 3.0)\n";
	code += "            qout.x = qa.z;\n";
	code += "        else if(qb.x == -3.0)\n";
	code += "            qout.x = -qa.z;\n";
	code += "        else if(qb.x == 4.0)\n";
	code += "            qout.x = qa.w;\n";
	code += "        else if(qb.x == -4.0)\n";
	code += "            qout.x = -qa.w;\n";
	code += "    }\n";
	code += "\n";
	code += "    if(qb.y != 0.0)\n";
	code += "    {\n";
	code += "        if(qb.y == 1.0)\n";
	code += "            qout.y = qa.x;\n";
	code += "        else if(qb.y == -1.0)\n";
	code += "            qout.y = -qa.x;\n";
	code += "        else if(qb.y == 2.0)\n";
	code += "            qout.y = qa.y;\n";
	code += "        else if(qb.y == -2.0)\n";
	code += "            qout.y = -qa.y;\n";
	code += "        else if(qb.y == 3.0)\n";
	code += "            qout.y = qa.z;\n";
	code += "        else if(qb.y == -3.0)\n";
	code += "            qout.y = -qa.z;\n";
	code += "        else if(qb.y == 4.0)\n";
	code += "            qout.y = qa.w;\n";
	code += "        else if(qb.y == -4.0)\n";
	code += "            qout.y = -qa.w;\n";
	code += "    }\n";
	code += "\n";
	code += "    if(qb.z != 0.0)\n";
	code += "    {\n";
	code += "        if(qb.z == 1.0)\n";
	code += "            qout.z = qa.x;\n";
	code += "        else if(qb.z == -1.0)\n";
	code += "            qout.z = -qa.x;\n";
	code += "        else if(qb.z == 2.0)\n";
	code += "            qout.z = qa.y;\n";
	code += "        else if(qb.z == -2.0)\n";
	code += "            qout.z = -qa.y;\n";
	code += "        else if(qb.z == 3.0)\n";
	code += "            qout.z = qa.z;\n";
	code += "        else if(qb.z == -3.0)\n";
	code += "            qout.z = -qa.z;\n";
	code += "        else if(qb.z == 4.0)\n";
	code += "            qout.z = qa.w;\n";
	code += "        else if(qb.z == -4.0)\n";
	code += "            qout.z = -qa.w;\n";
	code += "    }\n";
	code += "\n";
	code += "    if(qb.w != 0.0)\n";
	code += "    {\n";
	code += "        if(qb.w == 1.0)\n";
	code += "            qout.w = qa.x;\n";
	code += "        else if(qb.w == -1.0)\n";
	code += "            qout.w = -qa.x;\n";
	code += "        else if(qb.w == 2.0)\n";
	code += "            qout.w = qa.y;\n";
	code += "        else if(qb.w == -2.0)\n";
	code += "            qout.w = -qa.y;\n";
	code += "        else if(qb.w == 3.0)\n";
	code += "            qout.w = qa.z;\n";
	code += "        else if(qb.w == -3.0)\n";
	code += "            qout.w = -qa.z;\n";
	code += "        else if(qb.w == 4.0)\n";
	code += "            qout.w = qa.w;\n";
	code += "        else if(qb.w == -4.0)\n";
	code += "            qout.w = -qa.w;\n";
	code += "    }\n";
	code += "\n";
	code += "    return qout;\n";
	code += "}\n";
	code += "\n";
	code += "vec4 qswizzle(vec4 qa, vec4 qb)\n";
	code += "{\n";
	code += "    vec4 qout;\n";
	code += "\n";
	code += "    if(qb.x == 1.0)\n";
	code += "        qout.x = qa.x;\n";
	code += "    else if(qb.x == 2.0)\n";
	code += "        qout.x = qa.y;\n";
	code += "    else if(qb.x == 3.0)\n";
	code += "        qout.x = qa.z;\n";
	code += "    else\n";
	code += "        qout.x = qa.w;\n";
	code += "\n";
	code += "    if(qb.y == 1.0)\n";
	code += "        qout.y = qa.x;\n";
	code += "    else if(qb.y == 2.0)\n";
	code += "        qout.y = qa.y;\n";
	code += "    else if(qb.y == 3.0)\n";
	code += "        qout.y = qa.z;\n";
	code += "    else\n";
	code += "        qout.y = qa.w;\n";
	code += "\n";
	code += "    if(qb.z == 1.0)\n";
	code += "        qout.z = qa.x;\n";
	code += "    else if(qb.z == 2.0)\n";
	code += "        qout.z = qa.y;\n";
	code += "    else if(qb.z == 3.0)\n";
	code += "        qout.z = qa.z;\n";
	code += "    else\n";
	code += "        qout.z = qa.w;\n";
	code += "\n";
	code += "    if(qb.w == 1.0)\n";
	code += "        qout.w = qa.x;\n";
	code += "    else if(qb.w == 2.0)\n";
	code += "        qout.w = qa.y;\n";
	code += "    else if(qb.w == 3.0)\n";
	code += "        qout.w = qa.z;\n";
	code += "    else\n";
	code += "        qout.w = qa.w;\n";
	code += "\n";
	code += "    return qout;\n";
	code += "}\n";

	return code;
}
