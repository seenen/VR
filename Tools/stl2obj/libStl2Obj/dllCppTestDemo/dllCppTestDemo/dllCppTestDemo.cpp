// dllCppTestDemo.cpp : 定义控制台应用程序的入口点。
//

#include "stdafx.h"

#include <iostream>
#include <Windows.h>
#include <time.h>
#include <stdlib.h>  
#include <fstream>
#include <iostream>
#include <omp.h> // openmp头文件

using namespace std;

string GetFileTitleFromFileName(string path_buffer)
{
	//char path_buffer[_MAX_PATH];
	char drive[_MAX_DRIVE];
	char dir[_MAX_DIR];
	char fname[_MAX_FNAME];
	char ext[_MAX_EXT];

	//_makepath(path_buffer, "c", "\\sample\\crt\\", "makepath", "c");
	//printf("Path created with _makepath: %s\n\n", path_buffer);
	_splitpath_s(path_buffer.c_str(), drive, dir, fname, ext);
	//printf("Path extracted with _splitpath:\n");
	//printf("  Drive: %s\n", drive);
	//printf("  Dir: %s\n", dir);
	//printf("  Filename: %s\n", fname);
	//printf("  Ext: %s\n", ext);

	return fname;
}

bool CopyFile(char* SourceFile, char* NewFile)
{
	ifstream in;
	ofstream out;
	in.open(SourceFile, ios::binary);//打开源文件
	if (in.fail())//打开源文件失败
	{
		cout << "Error 1: Fail to open the source file." << endl;
		in.close();
		out.close();

		return false;
	}
	out.open(NewFile, ios::binary);//创建目标文件 
	if (out.fail())//创建文件失败
	{
		cout << "Error 2: Fail to create the new file." << endl;
		out.close();
		in.close();

		return false;
	}
	else//复制文件
	{
		out << in.rdbuf();
		out.close();
		in.close();

		return true;
	}

	return true;
}

typedef long long (*Dlltest)();

void DllTest()
{
	//	动态调用
	Dlltest maopao1;
	HINSTANCE hdll;
	hdll = LoadLibrary(TEXT("F:/GitHub/VR/Tools/stl2obj/libStl2Obj/dllCppTest/Debug/dllCppTest.dll"));
	if (hdll == NULL)
	{
		cout << " LoadLibrary LastError : " << GetLastError() << endl;
		FreeLibrary(hdll);
	}
	maopao1 = (Dlltest)GetProcAddress(hdll, "dlltest");
	if (maopao1 == NULL)
	{
		cout << " GetProcAddress LastError : " << GetLastError() << endl;
		FreeLibrary(hdll);
	}

	if (maopao1 != NULL)
	{
		long long ret = maopao1();

		cout << "Age : " << ret << endl;

		FreeLibrary(hdll);
	}
}

typedef int (*Dllstl2obj)(char*);

void STLTest(char* filepath)
{
	//	动态调用
	Dllstl2obj maopao1;
	HINSTANCE hdll;
	hdll = LoadLibrary(TEXT("F:/GitHub/VR/Tools/stl2obj/libStl2Obj/dllCppTest/Debug/dllCppTest.dll"));
	if (hdll == NULL)
	{
		cout << " LoadLibrary LastError : " << GetLastError() << endl;
		FreeLibrary(hdll);
	}
	maopao1 = (Dllstl2obj)GetProcAddress(hdll, "formatDataStl2Obj");
	if (maopao1 == NULL)
	{
		cout << " GetProcAddress LastError : " << GetLastError() << endl;
		FreeLibrary(hdll);
	}

	//	临时路径存储
	char tmp[MAX_PATH];

	if (maopao1 != NULL)
	{
		cout << "filepath : " << filepath << endl;

		DWORD dwStart, dwEnd, dwCount = 0;

#pragma omp parallel for   //特别注意点。
		for (int i = 1; i < 37; ++i)
		{
			memset(tmp, 0, MAX_PATH);

			dwStart = GetTickCount();

			sprintf_s(tmp, "F:/GitHub/VR/Tools/stl2obj/Resources/DataFileObj/%d.stl", i);

			//CopyFile(filepath, tmp);

			cout << "tmp : " << tmp << endl;

			int ret = maopao1(tmp);

			cout << "tmp ret : " << ret << endl;

			dwEnd = GetTickCount();

			dwCount += (dwEnd - dwStart);
		}

		cout << "dwCount : " << dwCount / 1000000.0 << endl;


		FreeLibrary(hdll);
	}
}


int main()
{
	//DllTest();

	STLTest("F:/GitHub/VR/Tools/stl2obj/Resources/dannang_after.stl");

	getchar();

	return 0;
}
