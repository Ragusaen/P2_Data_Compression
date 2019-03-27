#ifndef ENCODEDBYTE_H
#define ENCODEDBYTE_H

#include "common.h"

#define ENCODEDBYTE_RAW 0
#define ENCODEDBYTE_POINTER 1

#define ENCODEDBYTE_RAW_LENGTH 9
#define ENCODEDBYTE_POINTER_LENGTH 17

#define ENCODEDBYTE_POINTER_OFFSET_SIZE 12
#define ENCODEDBYTE_POINTER_LENGTH_SIZE 4

typedef struct Pointer {
    int offset;
    int length;
} Pointer;

typedef struct EncodedByte {
    byte  type;
    union {
        Pointer pointer;
        byte raw;
    };
} EncodedByte;

typedef struct EncodedByteArray {
    EncodedByte* data;
    uint length;
} EncodedByteArray;

typedef struct UnevenBits {
    usint data;
    usint length;
} UnevenBits;

typedef struct UnevenBitsArray {
    UnevenBits *data;
    uint length;
} UnevenBitsArray;

UnevenBitsArray encoded_bytes_to_uneven_bits(EncodedByteArray encoded_bytes);
ByteArray uneven_bits_to_bytes(UnevenBitsArray uneven_bits);

#endif
