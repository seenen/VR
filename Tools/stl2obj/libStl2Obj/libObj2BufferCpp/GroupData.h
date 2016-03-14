#pragma once
#include <vector>
#include <string>
#include "Define.h"
#include "GroupData.h"

using namespace std;

class GroupData
{
public:
	string name;
	string materialName;
	vector<FaceIndices> faces;

	GroupData();
	~GroupData();

	bool IsEmpty();

};

