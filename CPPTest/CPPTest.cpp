// CPPTest.cpp : �������̨Ӧ�ó������ڵ㡣
//

#include "stdafx.h"
#using "../Debug/ClassLibraryTest.dll"  
using namespace ClassLibraryTest;

int main()
{
	CStoCPPTest^ t = gcnew CStoCPPTest();
	int sum = t->Add(5, 7);
	printf("add result: %d\n",sum);
	char* list[] = { "abc","d","efg" };
	int index = 1;
	char* word;
	
	t->getStringInList(list, index, word);

    return 0;
}


