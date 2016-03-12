#pragma once

#include <string>

using namespace std;

class UtilFile
{
public:
	UtilFile();
	~UtilFile();

	static string GetFileFolder(char* filepath);
	static string GetFileFolder(string filepath);

	static string GetFileTitleFromFileName(string filepath);

	static bool CopyFile(char* srcfilepath, char* destfilepath);
};

