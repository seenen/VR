#include "UtilFile.h"
#include <fstream>
#include <iostream>

UtilFile::UtilFile()
{
}

UtilFile::~UtilFile()
{
}

string UtilFile::GetFileFolder(char* filepath)
{
	string s = string(filepath);

	std::string p = s.substr(0, s.find_last_of('/'));

	return p;
}

string UtilFile::GetFileFolder(string filepath)
{
	std::string p = filepath.substr(0, filepath.find_last_of('/'));

	return p;
}

string UtilFile::GetFileTitleFromFileName(string path_buffer)
{
	//char path_buffer[_MAX_PATH];
	char drive[_MAX_DRIVE];
	char dir[_MAX_DIR];
	char fname[_MAX_FNAME];
	char ext[_MAX_EXT];

	//_makepath(path_buffer, "c", "\\sample\\crt\\", "makepath", "c");
	//printf("Path created with _makepath: %s\n\n", path_buffer);
	_splitpath(path_buffer.c_str(), drive, dir, fname, ext);
	//printf("Path extracted with _splitpath:\n");
	//printf("  Drive: %s\n", drive);
	//printf("  Dir: %s\n", dir);
	//printf("  Filename: %s\n", fname);
	//printf("  Ext: %s\n", ext);

	return fname;
}

bool UtilFile::CopyFile(char* SourceFile, char* NewFile)
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
