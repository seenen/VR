// IvconSample.cpp : 定义控制台应用程序的入口点。
//

#include "stdafx.h"

#include <string>
#include <iostream>
#include <fstream>

using namespace std;

extern int obj_read(FILE *filein);

int Read_Obj()
{
	int code = -1;

	std::string obj_file = "F:/GitHub/VR/Tools/stl2obj/Resources/DataFileObj/1.obj";

	FILE* fr;

	fr = fopen(obj_file.c_str(), "r");

	code = obj_read(fr);

	return code;
}

int main()
{
	cout << "Read_Obj " << Read_Obj() << endl;

    return 0;
}
