#include <stdlib.h>
#include <stdio.h> //REMOVE

#include "EncodedByte.h"

static UnevenBits _encoded_byte_to_uneven_bits(EncodedByte eb);
 byte _get_bits(UnevenBits ub, usint count);
static uint _count_actual_bytes(UnevenBitsArray uneven_bits);

UnevenBitsArray encoded_bytes_to_uneven_bits(EncodedByteArray encoded_bytes) {
    //Create UnevenBitsArray and allocate data
    UnevenBitsArray uneven_bits = {0, encoded_bytes.length};
    uneven_bits.data = malloc(sizeof(UnevenBits) * uneven_bits.length);

    //Convert each EncodedByte to UnevenBits
    for (int i = 0; i < encoded_bytes.length; ++i) {
        uneven_bits.data[i] = _encoded_byte_to_uneven_bits(encoded_bytes.data[i]);
    }

    return uneven_bits;
}

ByteArray uneven_bits_to_bytes(UnevenBitsArray uneven_bits) {
    //Create byte array and allocate memory
    ByteArray bytes = {0, _count_actual_bytes(uneven_bits)};
    bytes.data = calloc(sizeof(byte), bytes.length);

    uint byte_index = 0;
    uint bit_index = 0;

    // ubai = unevenBitsArrayIndex
    for (int ubai = 0; ubai < uneven_bits.length; ubai++) {
        UnevenBits ub = uneven_bits.data[ubai];

        while (ub.length > 0) {
            if (ub.length >= 8 - bit_index) {
                bytes.data[byte_index] += (byte) _get_bits(ub, 8 - bit_index);
                ub.length -= 8 - bit_index;
                bit_index = 0;
                byte_index++;
            }
            else {
                bytes.data[byte_index] += (byte)(_get_bits(ub, ub.length) << (int)(8 - bit_index - ub.length));
                bit_index += ub.length;
                ub.length = 0;
            }
        }
    }

    bytes.length = byte_index + 1;
    return bytes;
}

static UnevenBits _encoded_byte_to_uneven_bits(EncodedByte eb) {
    if (eb.type == ENCODEDBYTE_POINTER) {
        uint data = (1 << ENCODEDBYTE_POINTER_OFFSET_SIZE) + (uint)(eb.pointer.offset % ENCODEDBYTE_POINTER_OFFSET_SIZE);
        data = (data << ENCODEDBYTE_POINTER_LENGTH_SIZE) + (uint)(eb.pointer.length % ENCODEDBYTE_POINTER_LENGTH_SIZE);

        return (UnevenBits){data, ENCODEDBYTE_POINTER_LENGTH};
    }
    return (UnevenBits){eb.raw, ENCODEDBYTE_RAW_LENGTH};
}

 byte _get_bits(UnevenBits ub, usint count) {
    return (byte)((ub.data >> (int)(ub.length - count)) % (1 << (int)(ub.length)));
}

static uint _count_actual_bytes(UnevenBitsArray uneven_bits) {
    uint total_bits = 0;
    for (int i = 0; i < uneven_bits.length; ++i) {
        total_bits += uneven_bits.length;
    }
    return total_bits % 8 == 0? total_bits / 8: (total_bits + 1) / 8;
}
