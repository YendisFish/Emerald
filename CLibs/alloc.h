#include "string.h"
#include "stdlib.h"

#ifndef ALLOC_H
#define ALLOC_H

string *allocate_str(int length)
{
    string *newstr = (string *)malloc(sizeof(string));
    char *fc = (char *)malloc(length * sizeof(char));

    newstr->firstchar = fc;
    newstr->len = length;

    return newstr;
}

char *allocate_char()
{
    char *ret = (char *)malloc(sizeof(char));
    return ret;
}

#endif