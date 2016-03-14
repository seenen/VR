#include <vector>
#include <iostream>
#include "Buff.h"
#include "Define.h"
#include "GeometryBuffer.h"
#include <Windows.h>

Buff::Buff()
{
}

Buff::~Buff()
{
}

//int Buff::splitString(const string & strSrc, const std::string& strDelims, vector<string>& strDest)
//{
//	typedef std::string::size_type ST;
//	string delims = strDelims;
//	std::string STR;
//	if (delims.empty()) delims = "/n/r";
//
//	ST pos = 0, LEN = strSrc.size();
//	while (pos < LEN) {
//		STR = "";
//		while ((delims.find(strSrc[pos]) != std::string::npos) && (pos < LEN)) ++pos;
//		if (pos == LEN) return strDest.size();
//		while ((delims.find(strSrc[pos]) == std::string::npos) && (pos < LEN)) STR += strSrc[pos++];
//		//std::cout << "[" << STR << "]";  
//		if (!STR.empty()) strDest.push_back(STR);
//	}
//
//	return strDest.size();
//}

int Buff::splitString(const string & s, const std::string& delim, vector<string>& ret)
{
	size_t last = 0;
	size_t index = s.find_first_of(delim, last);
	while (index != std::string::npos)
	{
		ret.push_back(s.substr(last, index - last));
		last = index + 1;
		index = s.find_first_of(delim, last);
	}
	if (index - last>0)
	{
		ret.push_back(s.substr(last, index - last));
	}

	return ret.size();
}

void Buff::ObjContent2Buffer(string data)
{
	DWORD dwStart, dwEnd, dwCount = 0;

	dwStart = GetTickCount();

	//	分割内容
	vector<string> splitStrs;

	int lines = splitString(data, "\n", splitStrs);

	dwEnd = GetTickCount();

	cout << "splitString " << (dwEnd - dwStart) / 10000000.0 << " lines = " << lines << endl;

	//	逐行解析
	vector<string> arr;

	vector<string>::iterator line_iter;

	GeometryBuffer buffer;

	dwStart = GetTickCount();
	//	解析每一行
	for (line_iter = splitStrs.begin(); line_iter != splitStrs.end(); ++line_iter)
	{
		//cout << "|" << *line_iter << "|/n";

		//	分割一行的四个元素
		splitString(*line_iter, " ", arr);

		vector<string>::iterator arr_iter;

		string head = arr[0];
		string p1 = arr[1];
		string p2 = arr[2];
		string p3 = arr[3];

		switch ((char)head[0])
		{
		case 'o':
			buffer.PushObject(p1);
			break;
		case 'g':
			buffer.PushGroup(p1);
			break;
		case 'v':
			buffer.PushVertex( Vector3( atof(p1.c_str()), 
										atof(p2.c_str()),
										atof(p3.c_str())) );
			break;
		case 'vt':
			buffer.PushUV(Vector2(	atof(p1.c_str()), 
									atof(p2.c_str())));
			break;
		case 'vn':
			buffer.PushNormal( Vector3( atof(p1.c_str()),
										atof(p2.c_str()),
										atof(p3.c_str())));
			break;
		case 'f':

			for (int j = 1; j < arr.size(); j++)
			{
				vector<string> splitarr;

				splitString(arr[j], "/", splitarr);

				//string[] c = arr[j].Trim().Split("/".ToCharArray());
				FaceIndices fi;
				fi.vi = atoi(splitarr[0].c_str()) - 1;
				if (splitarr.size() > 1 && splitarr[1] != "") fi.vu = atoi(splitarr[1].c_str()) - 1;
				if (splitarr.size() > 2 && splitarr[2] != "") fi.vn = atoi(splitarr[2].c_str()) - 1;
				buffer.PushFace(fi);
			}
			break;
		//case MTL:
		//	mtllib = p[1].Trim();
		//	break;
		//case UML:
		//	buffer.PushMaterialName(p[1].Trim());
		//	break;
		}

	}

		dwEnd = GetTickCount();

		cout << "GeometryBuffer " << (dwEnd - dwStart) / 10000000.0 << endl;

	//return buffer;
}

//模板函数：将string类型变量转换为常用的数值类型（此方法具有普遍适用性） 
template <class T>
void Buff::convertFromString(T &value, const string& str)
{
	istringstream iss(str);

	iss >> value;

}

int BuffContent(const char* filename)
{
	DWORD dwStart, dwEnd, dwCount = 0;

	dwStart = GetTickCount();

	Buff buf;

	buf.ObjContent2Buffer(filename);

	dwEnd = GetTickCount();

	cout << "BuffContent " << (dwEnd - dwStart) / 10000000.0 << endl;

	return 0;
}