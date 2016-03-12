#include<iostream>
#include<stdlib.h>
#include<vector>
using namespace std;

struct vec3f
{
	float x,y,z;
	int index;
	inline vec3f operator =(const vec3f &v)
	{
		this->x=v.x;
		this->y=v.y;
		this->z=v.z;
		this->index=v.index;
		return *this;
	}
	inline bool operator==(vec3f &v)
	{
		if(this->x==v.x&&this->y==v.y&&this->z==v.z)
			return true;
		else
			return false;
	}
	inline bool operator!=(vec3f &v)
	{
		return !(*this==v);
	}
};

struct face
{
	vec3f verts[3];
	face operator=(const face &f)
	{
		for (int n=0;n<3;n++)
		{
			this->verts[n]=f.verts[n];
		}
		return *this;
	}
};

//	¸ñÊ½×ª»»
char* fileformatchange(char* filename)
{
	FILE*filein;
	FILE*fileout;
	float x,y,z;
    char buf[999];
	char str[100];
	vector<vec3f> vec1;
    vector<face> myvector;
	if(!(filein=fopen(filename,"r")))exit(1);
	while(fscanf(filein,"%s",buf)!=EOF)
	{
		switch(buf[0])
		{
		case's'://solid CATIA STL
		case'o'://outer loop
		case'e'://endloop or endfacet or endsolid C...
			fgets(buf,sizeof(buf),filein);
			break;
		case'f'://facet normal ...!
			{
				face temp;
				fscanf(filein,"%s",str);
				fscanf(filein,"%f %f %f",&x,&y,&z);
				fgets(buf,sizeof(buf),filein);//...might be '\0'
				fgets(buf,sizeof(buf),filein);//skip "outer loop"
				for(int k=0;k<3;k++)
				{
					float xx,yy,zz;
					fscanf(filein,"%s",str);//skip "vertex"
					fscanf(filein,"%f %f %f",&xx,&yy,&zz);
					temp.verts[k].x=xx;
					temp.verts[k].y=yy;
					temp.verts[k].z=zz;
					int w=0;
					if (vec1.size()==0)
					{
						temp.verts[k].index=w+1;
					}
					else
					{
						for( w=0;w<vec1.size();w++)
						{
							if(temp.verts[k]==vec1[w])
							{
								temp.verts[k].index=w+1;
								break;
							}
							else if(temp.verts[k]!=vec1[w])
								continue;

							else
								;
						}
					}
					
					if(w==vec1.size())
					{
						temp.verts[k].index=w+1;
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
    if(!(fileout=fopen("chao.txt","w")))exit(1);
	fprintf(fileout,"# %d vertex %d face\n",(int)vec1.size(),(int)myvector.size());
    for(int j=0;j<vec1.size();j++)
		fprintf(fileout,"v %f %f %f\n", vec1[j].x,vec1[j].y,vec1[j].z);
	for(int j=0;j<myvector.size();j++)
		fprintf(fileout,"f %d %d %d\n",myvector[j].verts[0].index,myvector[j].verts[1].index,myvector[j].verts[2].index);
	fclose(fileout);

	return "chao.txt";
}
void main()
{
	fileformatchange("Exercise02.stl");
}