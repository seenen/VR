#include "GroupData.h"

GroupData::GroupData()
{
	faces.clear();
}


GroupData::~GroupData()
{
}

bool GroupData::IsEmpty()
{ 
	return faces.size() == 0 ? true : false;
}

