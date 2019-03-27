#include <stdlib.h>
#include <string.h>
#include <stdio.h> //REMOVE

#include "LZ77.h"

#define POINTER_MAX_OFFSET 4096
#define POINTER_LENGTH 16

static Pointer find_pointer_of_longest_match(ByteArray history, ByteArray look_ahead);
static int find_index_of_match(ByteArray haystack, ByteArray needle);
static EncodedByte encode_byte(ByteArray input, uint index);

static int min(int a, int b);
static int last(ByteArray bytes, int index);

EncodedByteArray lz77_encode(ByteArray input) {
    EncodedByteArray eb_array = {NULL, input.length};
    eb_array.data = malloc(sizeof(EncodedByte) * input.length);

    uint byte_index = 0, eb_index = 0;
    while (byte_index < input.length) {
        //Encode current byte
        EncodedByte eb = encode_byte(input, byte_index);
        eb_array.data[eb_index++] = eb;

        //Adjust the current index accordingly
        if ( eb.type == ENCODEDBYTE_POINTER)
            byte_index += eb.pointer.length + 1;
        else
            ++byte_index;
    }

    eb_array.length = eb_index;
    return eb_array;
}

static EncodedByte encode_byte(ByteArray input, uint index) {
    //Create the history buffer
    ByteArray history;
    if (index < POINTER_MAX_OFFSET)
        history = (ByteArray){input.data, index};
    else
        history = (ByteArray){input.data + index - POINTER_MAX_OFFSET, POINTER_MAX_OFFSET};

    //Create the look ahead buffer
    ByteArray look_ahead;
    if (index > input.length - POINTER_LENGTH)
        look_ahead = (ByteArray){input.data + index, input.length - index};
    else
        look_ahead = (ByteArray){input.data + index, POINTER_LENGTH};

    //Get the longest match
    Pointer p = find_pointer_of_longest_match(history, look_ahead);

    //Encode as pointer or raw byte
    if (p.length >= 0) {
        EncodedByte eb = {ENCODEDBYTE_POINTER, 0};
        eb.pointer = p;
        return eb;
    }
    return (EncodedByte){ENCODEDBYTE_RAW, input.data[index]};
}

static Pointer find_pointer_of_longest_match(ByteArray history, ByteArray look_ahead) {
    //Look for all lengths of the look ahead
    for (; look_ahead.length >= 2; --look_ahead.length) {
        //Look through the history buffer
        int offset = find_index_of_match(history, look_ahead);
        if (offset >= 0)
            return (Pointer){history.length - offset - 1, look_ahead.length - 1};
    }
    return (Pointer){0,-1}; //If no match was found
}

static int find_index_of_match(ByteArray haystack, ByteArray needle) {
    int m = needle.length, n = haystack.length;
    int i = m - 1, j = m - 1;

    while (i <= n - 1) {
        if (needle.data[j] == haystack.data[i]) {
            if (j == 0)
                return i;
            else {
                --i;
                --j;
            }
        } else {
            i = i + m - min(j,1+last(needle,j));
            j = m - 1;
        }
    }
}

static int min(int a, int b) {
    return a < b? a: b;
}

static int last(ByteArray bytes, int index) {
    for (int i = index; i < bytes.length; ++i)
        if (bytes.data[i] == bytes.data[index] )
            return i;
    return -1;
}
