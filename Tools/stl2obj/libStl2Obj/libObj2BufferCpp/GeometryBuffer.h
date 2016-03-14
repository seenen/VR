#pragma once
#include <vector>
#include "Define.h"
#include "ObjectData.h"
#include "GroupData.h"

using namespace std;

class GeometryBuffer
{
private:
	vector<ObjectData> objects;
	vector<Vector3> vertices;
	vector<Vector2> uvs;
	vector<Vector3> normals;

	ObjectData current;
	GroupData curgr;

public:
	GeometryBuffer();
	~GeometryBuffer();

	void PushObject(string name);
	void RemoveObject(ObjectData od);
	void PushGroup(string name);
	void RemoveGroup(GroupData od);
	void PushMaterialName(string name);
	void PushVertex(Vector3 v);
	void PushUV(Vector2 v);
	void PushNormal(Vector3 v);
	void PushFace(FaceIndices f);

	template <class T>
	void convertFromString(T &, const string& str);
};

