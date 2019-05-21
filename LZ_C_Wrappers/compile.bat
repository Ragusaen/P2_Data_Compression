gcc findmatchingbytes.c -o findmatchingbytes.o -c -fPIC -O2
gcc findmatchingbytes.o -o findmatchingbytes.dll -shared -flinker-output=dyn
cp findmatchingbytes.dll ../compression/Compression/findmatchingbytes.dll
