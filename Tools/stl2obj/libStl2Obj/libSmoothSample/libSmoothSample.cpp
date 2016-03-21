// libSmoothSample.cpp : 定义控制台应用程序的入口点。
//
#include <iostream>
#include <Windows.h>
#include <time.h>

#include "stdafx.h"

#include "mesh.h"

void Sample()
{
	DWORD dwStart, dwEnd, dwCount = 0;

	dwStart = GetTickCount();

	std::string filepath = "F:/GitHub/VR/Tools/stl2obj/Resources/pacman_ghost_holder_coarse.stl";

	indexed_mesh im;

	im.load_from_binary_stereo_lithography_file(filepath.c_str());

	dwEnd = GetTickCount();

	cout << "load_from_binary_stereo_lithography_file : " << (GetTickCount() - dwStart) / 1000000.0 << endl;

	dwStart = GetTickCount();

	im.laplace_smooth(0.5);

	cout << "laplace_smooth : " << (GetTickCount() - dwStart) / 1000000.0 << endl;

	getchar();

}

int main()
{
	Sample();

    return 0;
}

