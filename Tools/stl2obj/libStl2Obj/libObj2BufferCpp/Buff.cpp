#include <vector>
#include <iostream>
#include "Buff.h"
#include "Define.h"

Buff::Buff()
{
}

Buff::~Buff()
{
}

int Buff::splitString(const string & strSrc, const std::string& strDelims, vector<string>& strDest)
{
	typedef std::string::size_type ST;
	string delims = strDelims;
	std::string STR;
	if (delims.empty()) delims = "/n/r";

	ST pos = 0, LEN = strSrc.size();
	while (pos < LEN) {
		STR = "";
		while ((delims.find(strSrc[pos]) != std::string::npos) && (pos < LEN)) ++pos;
		if (pos == LEN) return strDest.size();
		while ((delims.find(strSrc[pos]) == std::string::npos) && (pos < LEN)) STR += strSrc[pos++];
		//std::cout << "[" << STR << "]";  
		if (!STR.empty()) strDest.push_back(STR);
	}
	return strDest.size();
}

void Buff::ObjContent2Buffer(string data)
{
	vector<string> splitStrs;

	splitString(data, "\n", splitStrs);

	//	ÖðÐÐ½âÎö
	vector<string> arr;

	vector<string>::iterator line_iter;

	for (line_iter = splitStrs.begin(); line_iter != splitStrs.end(); ++line_iter)
	{
		cout << "|" << *line_iter << "|/n";

		splitString(*line_iter, " ", arr);

		vector<string>::iterator arr_iter;

		char head = (*line_iter)[0];

		switch (head)
		{
		case 'o':
			////buffer.PushObject(p[1].Trim());
			break;
		case 'g':
			////buffer.PushGroup(p[1].Trim());
			break;
		case 'v':
			////buffer.PushVertex(new Vector3((p[1]), (p[2]), (p[3])));
			break;
		case 'vt':
			////buffer.PushUV(new Vector2((p[1]), (p[2])));
			break;
		case 'vn':
			////buffer.PushNormal(new Vector3((p[1]), (p[2]), (p[3])));
			break;
		case 'f':
			//for (int j = 1; j < p.Length; j++)
			//{
			//	string[] c = p[j].Trim().Split("/".ToCharArray());
			//	FaceIndices fi = new FaceIndices();
			//	fi.vi = ci(c[0]) - 1;
			//	if (c.Length > 1 && c[1] != "") fi.vu = ci(c[1]) - 1;
			//	if (c.Length > 2 && c[2] != "") fi.vn = ci(c[2]) - 1;
			//	buffer.PushFace(fi);
			//}
			break;
		//case MTL:
		//	mtllib = p[1].Trim();
		//	break;
		//case UML:
		//	buffer.PushMaterialName(p[1].Trim());
		//	break;
		}
	}
}

int BuffContent(char* filename)
{
	return 0;
}