gcc findmatchingbytes.c -o findmatchingbytes.o -c -fPIC -O2
gcc findmatchingbytes.o -o libfindmatchingbytes.so -shared -flinker-output=dyn
cp libfindmatchingbytes.so ../compression/Compression/libfindmatchingbytes.so
