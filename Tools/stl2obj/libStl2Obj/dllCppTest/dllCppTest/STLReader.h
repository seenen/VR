#pragma once
#include "dataStruct.h"


class STLReader
{
	vector<vec3f> vec1;
	vector<vec3f> normals;
	vector<face> myvector;


public:
	STLReader();
	~STLReader();

	int AsciiSTLReader(const char* filename);
	int BinarySTLReader(const char* filename);
	int Write2Obj(const char* filepath, const char* filename);

};

