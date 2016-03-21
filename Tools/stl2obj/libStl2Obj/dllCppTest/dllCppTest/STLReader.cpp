#include "STLReader.h"


STLReader::STLReader()
{
}


STLReader::~STLReader()
{
}

int STLReader::AsciiSTLReader(const char* filename)
{
	FILE* filein;
	float x, y, z;
	char buf[999];
	char str[100];

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

	return 0;
}

//****************************************************************************80
//
//  Purpose:
//
//    STLB_READ reads a binary STL (stereolithography) file.
//
//  Example:
//
//    80 byte string = header containing nothing in particular
//
//    4 byte int = number of faces
//
//    For each face:
//
//      3 4-byte floats = components of normal vector to face;
//      3 4-byte floats = coordinates of first node;
//      3 4-byte floats = coordinates of second node;
//      3 4-byte floats = coordinates of third and final node;
//        2-byte int = attribute, whose value is 0.
//
//  Licensing:
//
//    This code is distributed under the GNU LGPL license.
//
//  Modified:
//
//    24 May 1999
//
//  Author:
//
//    John Burkardt
//
//  Reference:
//
//    3D Systems, Inc,
//    Stereolithography Interface Specification,
//    October 1989.
//
int STLReader::BinarySTLReader(const char* filename)
{
	//short int attribute = 0;
	//char c;
	//float cvec[3];
	//int icor3;
	//int i;
	//int iface;
	//int ivert;
	////
	////  80 byte Header.
	////
	//for (i = 0; i < 80; i++)
	//{
	//	c = ch_read(filein);
	//	if (debug)
	//	{
	//		cout << c << "\n";
	//	}
	//	bytes_num = bytes_num + 1;
	//}
	////
	////  Number of faces.
	////
	//face_num = long_int_read(filein);
	//bytes_num = bytes_num + 4;
	////
	////  For each (triangular) face,
	////    components of normal vector,
	////    coordinates of three vertices,
	////    2 byte "attribute".
	////
	//for (iface = 0; iface < face_num; iface++)
	//{
	//	face_order[iface] = 3;
	//	face_material[iface] = 0;

	//	for (i = 0; i < 3; i++)
	//	{
	//		face_normal[i][iface] = float_read(filein);
	//		bytes_num = bytes_num + 4;
	//	}

	//	for (ivert = 0; ivert < face_order[iface]; ivert++)
	//	{
	//		for (i = 0; i < 3; i++)
	//		{
	//			cvec[i] = float_read(filein);
	//			bytes_num = bytes_num + 4;
	//		}

	//		if (cor3_num < 1000)
	//		{
	//			icor3 = rcol_find(cor3, 3, cor3_num, cvec);
	//		}
	//		else
	//		{
	//			icor3 = -1;
	//		}

	//		if (icor3 == -1)
	//		{
	//			icor3 = cor3_num;
	//			if (cor3_num < COR3_MAX)
	//			{
	//				cor3[0][cor3_num] = cvec[0];
	//				cor3[1][cor3_num] = cvec[1];
	//				cor3[2][cor3_num] = cvec[2];
	//			}
	//			cor3_num = cor3_num + 1;
	//		}
	//		else
	//		{
	//			dup_num = dup_num + 1;
	//		}

	//		face[ivert][iface] = icor3;

	//	}
	//	attribute = short_int_read(filein);
	//	if (debug)
	//	{
	//		cout << "ATTRIBUTE = " << attribute << "\n";
	//	}
	//	bytes_num = bytes_num + 2;
	//}

	return 0;
}

int STLReader::Write2Obj(const char* filepath, const char* filename)
{
	FILE* fileout;

	char outfilepath[_MAX_PATH];
	memset(outfilepath, 0, _MAX_PATH);
	sprintf(outfilepath, "%s/%s.obj", filepath, filename);

	if (!(fileout = fopen(outfilepath, "w")))
		return 2;

	fprintf(fileout, "# %d vertex %d face\n", (int)vec1.size(), (int)myvector.size());
	for (int j = 0; j<vec1.size(); j++)
		fprintf(fileout, "v %f %f %f\n", vec1[j].x, vec1[j].y, vec1[j].z);
	for (int j = 0; j<myvector.size(); j++)
		fprintf(fileout, "f %d %d %d\n", myvector[j].verts[0].index, myvector[j].verts[1].index, myvector[j].verts[2].index);
	fclose(fileout);
}
