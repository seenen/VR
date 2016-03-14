#pragma once

#include <string>

using namespace std;

class Buff
{
private:
	const char O = 'o';
	const char G = 'g';
	const char V = 'v';
	const char VT = 'vt';
	const char VN = 'vn';
	const char F = 'f';
	const string MTL = "mtllib";
	const string UML = "usemtl";
	static string mtllib;


	int splitString(const string & strSrc, const std::string& strDelims, vector<string>& strDest);
	float cf(string v);
	float ci(string v);

public:
	Buff();
	~Buff();
	
	void ObjContent2Buffer(string data);

};

extern "C" __declspec (dllexport) int BuffContent(const char* filename);

