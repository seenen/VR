#pragma once

//# define _DLLExport __declspec (dllexport) //���Ϊ��������;

extern "C" __declspec (dllexport) long long dlltest();

extern "C" __declspec (dllexport) int formatDataStl2Obj(char* filename);