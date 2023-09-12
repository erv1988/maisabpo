typedef unsigned char byte;
typedef unsigned int uint;
#include <random>
#include <Windows.h>
uint chksum_xor(byte*, int n);

int main()
{
    int N = 64 * 1024 * 1024;
	byte *memory = new byte[N];
	time_t t = time(nullptr);
	srand(t);

	printf("Ptr = %016llX\n", (unsigned long long)memory);
    printf("press Enter key for continue");
    getchar();

    // цикл
    while (true)
    {
        for (int i = 0; i < 16; i++)
            memory[i] = 0xab;
        for (int i = 16; i < N - 16; ++i)
            memory[i] = (byte)rand();

        uint chksum_0 = chksum_xor(memory, N);
        printf("Chksum before= %08X\n",chksum_0);

        for (int i = 0; i < 32; ++i)
            printf("%02X ", memory[i]);
        printf("\n");

        // задержка на 1 сек
        Sleep(1000);
        uint chksum_1 = chksum_xor(memory,N);
        printf("Chksum after= %08X\n", chksum_1);

        for (int i = 0; i < 32; ++i)
            printf("%02X ", memory[i]);
        printf("\n");

        if (chksum_0 != chksum_1)
            printf("Corrupt!!!");
        // задержка на 1 сек
        Sleep(1000);
    }
	delete[] memory;
	return 0;
}

uint chksum_xor(byte* data, int N)
{
    uint chk = 0;
    for (int n = 0; n < N; ++n)
        chk ^= data[n];
    return chk;
}