#pragma once
#include <Windows.h>
#include <iostream>

///	对字符串的操作类
class UtilString
{
public:
	UtilString();
	~UtilString();

	static char* w2c(char *pcstr, const wchar_t *pwstr, size_t len);
	static void c2w(wchar_t *pwstr, size_t len, const char *str);

};

