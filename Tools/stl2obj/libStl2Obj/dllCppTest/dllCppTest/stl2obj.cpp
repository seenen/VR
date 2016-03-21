#include "stl2obj.h"

#include "UtilFile.h"

#include "STLReader.h"

long long dlltest()
{
	long long a = 1;
	int b = 0;
	while (b<1000000000)
	{
		a = a + b;
		b++;
	}
	return a;
}

///	STLÐ´³ÉOBJÎÄ¼þ
int formatDataStl2Obj(char* filename)
{
	int code = 0;
	STLReader* stlreader = new STLReader();

	do
	{
		code = stlreader->AsciiSTLReader(filename);
		if (code != 0) 
			break;

		string pathFolder = UtilFile::GetFileFolder(filename);
		string name = UtilFile::GetFileTitleFromFileName(filename);

		code = stlreader->Write2Obj(pathFolder.c_str(), name.c_str());
		if (code != 0) 
			break;

	} while (false);

	delete stlreader;
	stlreader = NULL;

	return code;
}
