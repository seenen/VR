#pragma once
#include <vector>
#include <string>
#include "Define.h"
#include "GroupData.h"

using namespace std;

class ObjectData
{
public:
	string name;
	vector<GroupData> groups;
	vector<FaceIndices> allFaces;

	ObjectData();
	~ObjectData();

};

