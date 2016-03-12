#pragma once

//# define _DLLExport __declspec (dllexport) //标记为导出函数;

extern "C" __declspec (dllexport) long long dlltest();

extern "C" __declspec (dllexport) int formatDataStl2Obj(char* filename);