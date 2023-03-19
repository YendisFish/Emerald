#include "memory.h"

#ifndef GC_H
#define GC_H

typedef struct GC
{
    struct Stack stack;
    struct Memory** heap;

    void GCSweep()
    {
    }
};

#endif