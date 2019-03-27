#include <stdio.h>
#include <stdlib.h>
#include <time.h>

#include "common.h"
#include "EncodedByte.h"
#include "LZ77.h"

ByteArray read_file_to_bytes(char *path);
void write_bytes_to_file(ByteArray bytes, char *path);

int main(int argc, char *argv[]) {
    char* output_path = "out";
    char* input_path;

    if (argc < 2)
        printf("Please supply a path to file\n");
    input_path = argv[1];

    if (argc >= 3)
        output_path = argv[2];

    time_t start = clock();

    ByteArray input_data = read_file_to_bytes(input_path);

    EncodedByteArray encoded_bytes = lz77_encode(input_data);
    free(input_data.data);

    UnevenBitsArray uneven_bits = encoded_bytes_to_uneven_bits(encoded_bytes);
    free(encoded_bytes.data);

    ByteArray output = uneven_bits_to_bytes(uneven_bits);
    free(uneven_bits.data);

    write_bytes_to_file(output, output_path);
    free(output.data);

    printf("Time elapsed (ms): %ld\n", (clock()-start)/(CLOCKS_PER_SEC / 1000));

    return EXIT_SUCCESS;
}

ByteArray read_file_to_bytes(char *path) {
    //Open file
    FILE* fp = fopen(path, "r");

    //Get file length
    fseek(fp, 0L, SEEK_END);
    usint file_size = ftell(fp);
    fseek(fp, 0L, SEEK_SET);

    //Create byte array
    ByteArray result;
    result.length = file_size;

    //Allocate space for the data
    result.data = malloc(sizeof(byte) * file_size);

    //Load the data
    byte d;
    for (int i = 0; (d = fgetc(fp)) != EOF; i++ ) {
        result.data[i] = d;
    }

    //Close the file
    fclose(fp);

    return result; //Return success
}

void write_bytes_to_file(ByteArray bytes, char *path) {
    //Open file
    FILE* fp = fopen(path, "w");

    //Write bytes
    fwrite(bytes.data, sizeof(byte), bytes.length, fp);

    //Close file
    fclose(fp);
}
