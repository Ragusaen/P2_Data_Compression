#ifndef COMMON_H
#define COMMON_H

typedef char byte;
typedef unsigned int uint;
typedef unsigned short int usint;

typedef struct ByteArray {
    byte *data;
    uint length;
} ByteArray;

#endif
