#include "GeometryBuffer.h"

GeometryBuffer::GeometryBuffer()
{
	ObjectData d;
	d.name = "default";
	objects.push_back(d);
	current = d;

	GroupData g;
	g.name = "default";
	d.groups.push_back(g);
	curgr = g;

	vertices.clear();
	uvs.clear();
	normals.clear();

}

GeometryBuffer::~GeometryBuffer()
{
	vertices.clear();
	uvs.clear();
	normals.clear();
}

void GeometryBuffer::RemoveObject(ObjectData od)
{
	vector<ObjectData>::iterator iter;

	//	解析每一行
	for (iter = objects.begin(); iter != objects.end(); ++iter)
	{
		if ((*iter).name == od.name)
			break;
	}

	objects.erase(iter);

}

void GeometryBuffer::PushObject(string name)
{
	if (vertices.size() == 0)
		RemoveObject(current);
		//objects.Remove(current);

	ObjectData n;
	n.name = name;
	objects.push_back(n);

	GroupData g;
	g.name = "default";
	n.groups.push_back(g);

	curgr = g;
	current = n;
}

void GeometryBuffer::RemoveGroup(GroupData gd)
{
	vector<GroupData>::iterator iter;

	//	解析每一行
	for (iter = current.groups.begin(); iter != current.groups.end(); ++iter)
	{
		if ((*iter).name == gd.name)
			break;
	}

	current.groups.erase(iter);

}

void GeometryBuffer::PushGroup(string name)
{
	if (curgr.IsEmpty())
		RemoveGroup(curgr);
		//current.groups.Remove(curgr);

	GroupData g;
	g.name = name;
	current.groups.push_back(g);
	curgr = g;
}

void GeometryBuffer::PushMaterialName(string name)
{
	if (!curgr.IsEmpty())
		PushGroup(name);

	if (curgr.name == "default") 
		curgr.name = name;
	curgr.materialName = name;
}

void GeometryBuffer::PushVertex(Vector3 v)
{
	vertices.push_back(v);
}

void GeometryBuffer::PushUV(Vector2 v)
{
	uvs.push_back(v);
}

void GeometryBuffer::PushNormal(Vector3 v)
{
	normals.push_back(v);
}

void GeometryBuffer::PushFace(FaceIndices f)
{
	curgr.faces.push_back(f);
	current.allFaces.push_back(f);
}

