#include "stl2obj.h"
#include "dataStruct.h"
#include "UtilFile.h"

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

int formatDataStl2Obj(char* filename)
{
	FILE*filein;
	FILE*fileout;
	float x, y, z;
	char buf[999];
	char str[100];
	vector<vec3f> vec1;
	vector<face> myvector;
	if (!(filein = fopen(filename, "r")))
		return 1;

	while (fscanf(filein, "%s", buf) != EOF)
	{
		switch (buf[0])
		{
		case's'://solid CATIA STL
		case'o'://outer loop
		case'e'://endloop or endfacet or endsolid C...
			fgets(buf, sizeof(buf), filein);
			break;
		case'f'://facet normal ...!
		{
			face temp;
			fscanf(filein, "%s", str);
			fscanf(filein, "%f %f %f", &x, &y, &z);
			fgets(buf, sizeof(buf), filein);//...might be '\0'
			fgets(buf, sizeof(buf), filein);//skip "outer loop"
			for (int k = 0; k<3; k++)
			{
				float xx, yy, zz;
				fscanf(filein, "%s", str);//skip "vertex"
				fscanf(filein, "%f %f %f", &xx, &yy, &zz);
				temp.verts[k].x = xx;
				temp.verts[k].y = yy;
				temp.verts[k].z = zz;
				int w = 0;
				if (vec1.size() == 0)
				{
					temp.verts[k].index = w + 1;
				}
				else
				{
					for (w = 0; w<vec1.size(); w++)
					{
						if (temp.verts[k] == vec1[w])
						{
							temp.verts[k].index = w + 1;
							break;
						}
						else if (temp.verts[k] != vec1[w])
							continue;

						else
							;
					}
				}

				if (w == vec1.size())
				{
					temp.verts[k].index = w + 1;
					vec1.push_back(temp.verts[k]);
				}
			}
			myvector.push_back(temp);
			break;
		}
		default:
			fgets(buf, sizeof(buf), filein);
			break;
		}
	}
	fclose(filein);

	string pathFolder = UtilFile::GetFileFolder(filename);
	string name = UtilFile::GetFileTitleFromFileName(filename);

	char outfilepath[_MAX_PATH];
	memset(outfilepath, 0, _MAX_PATH);
	sprintf(outfilepath, "%s/%s.obj", pathFolder.c_str(), name.c_str());

	if (!(fileout = fopen(outfilepath, "w"))) 
		return 2;
	//if (!(fileout = fopen("F:/GitHub/VR/Tools/stl2obj/Resources/_tmp.obj", "w"))) 
		//return 2;
	fprintf(fileout, "# %d vertex %d face\n", (int)vec1.size(), (int)myvector.size());
	for (int j = 0; j<vec1.size(); j++)
		fprintf(fileout, "v %f %f %f\n", vec1[j].x, vec1[j].y, vec1[j].z);
	for (int j = 0; j<myvector.size(); j++)
		fprintf(fileout, "f %d %d %d\n", myvector[j].verts[0].index, myvector[j].verts[1].index, myvector[j].verts[2].index);
	fclose(fileout);

	return 0;
	//return "chao.txt";
}
