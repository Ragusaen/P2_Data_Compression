typedef struct match_pointer_t {
    int index;
    int length;
} MatchPointer;

typedef unsigned char byte;

typedef struct array_indexer_t {
    byte* array;
    int index;
    int length;
} ArrayIndexer;

MatchPointer find_longest_match(ArrayIndexer haystack, ArrayIndexer needle);
static int matching_bytes_count(ArrayIndexer a, int index, ArrayIndexer b );

MatchPointer find_longest_match(ArrayIndexer haystack, ArrayIndexer needle) {
    int longestMatch = 1;
    int indexOfLongestMatch = 0;

    //Find the longest match in the haystack
    for (int i = 0; i < haystack.length - longestMatch; ++i) {
        // Check if the first two are a match
        if (haystack.array[haystack.index + i]     == needle.array[needle.index] &&
            haystack.array[haystack.index + i + 1] == needle.array[needle.index + 1])
        {
            int matchedBytes = matching_bytes_count(haystack, i, needle);
            if (matchedBytes > longestMatch) {
                longestMatch = matchedBytes;
                indexOfLongestMatch = i;
            }
        }
    }

    return (longestMatch > 1)? (MatchPointer){indexOfLongestMatch, longestMatch}: (MatchPointer){0};
}


static int matching_bytes_count(ArrayIndexer a, int index, ArrayIndexer b ) {
    int i = 2;
    for (; i < b.length && index + i < a.length; ++i)
        if (a.array[a.index + index + i] != b.array[b.index + i])
            return i;
    return i;
}
