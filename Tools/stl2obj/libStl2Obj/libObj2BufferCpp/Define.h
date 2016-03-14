#pragma once

typedef struct FaceIndices
{
	int vi;
	int vu;
	int vn;

public:
	FaceIndices()  //默认构造函数
	{
		vi = 0;
		vu = 0;
		vn = 0;
	}
};

typedef struct Vector3
{
private:
	float x;
	float y;
	float z;

	void set(Vector3* s1, Vector3* s2)
	{
		s1->x = s2->x;
		s1->y = s2->y;
		s1->z = s2->z;
	}

public:
	Vector3()
	{
         x = 0;
         y = 0;
		 z = 0;
	}

	Vector3(float _x, float _y, float _z)
	{
         x = _x;
         y = _y;
		 z = _z;
	}

	//Vector3& operator=(const Vector3& s)
	//{
	//	set(this, (Vector3*)&s);
	//}

	//Vector3(const Vector3& s)
 //   {
	//	*this = s;
 //   }
};

typedef struct Vector2
{
private:
	int x;
	int y;

public:
	Vector2()  //默认构造函数
	{
		x = 0;
		y = 0;
	}

	Vector2(float _x, float _y) 
	{
		x = _x;
		y = _y;
	}

	//void set(Vector2* s1, Vector2* s2)//赋值函数
	//{
	//	s1->x = s2->x;
	//	s1->y = s2->y;
	//}

	//Vector2& operator=(const Vector2& s)//重载运算符
	//{
	//	set(this, (Vector2*)&s);

	//	return this;
	//}

	Vector2(const Vector2& s)//复制构造函数
	{
		*this = s;
	}
};