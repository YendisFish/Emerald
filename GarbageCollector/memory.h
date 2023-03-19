#include "stdlib.h"
#include<stdbool.h>

#ifndef MEMORY_H
#define MEMORY_H

typedef struct Memory
{
    void *first;
    int block_size;
    bool marked;
};

typedef struct Variable
{
    void *ptr;
    int size;
    bool marked;
};

typedef struct Stack
{
    Variable *mem;
    int length;
    int *sizes;

    void Pop()
    {
        int last_index = sizeof(mem) / sizeof(Variable *) - 1;
        free(&mem[last_index]);
        length = length - 1;
    }

    Variable * Push(void *ptr, int size)
    {
        Variable *newmem = (Variable *)malloc((sizeof(mem) / sizeof(Variable *)) + (sizeof(struct Variable)));
        int last = FillVarPtr(mem, newmem);

        Variable topush;
        topush.marked = true;
        topush.ptr = ptr;
        topush.size = size;

        newmem[last] = topush;

        mem = newmem;
        length = length + 1;
        return &newmem[last];
    }
};


int FillVarPtr(Variable *ptr, Variable *newptr)
{
    int last_index = sizeof(ptr) / sizeof(Variable *) - 1;
    int new_last_index = sizeof(newptr) / sizeof(Variable *) - 1;

    for(int i = 0; i <= last_index; i++)
    {
        newptr[i] = ptr[i];
    }

    newptr[new_last_index] = *(Variable *)malloc(sizeof(Variable));

    return new_last_index;
}

#endif