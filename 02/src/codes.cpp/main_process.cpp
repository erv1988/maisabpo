#include <Windows.h>
#include <tlhelp32.h>
#include <random>
#include <string>
#include <iostream>

void EnableDebugPriv();

int main()
{
    SIZE_T bytesRW = 0;
    int N = 64 * 1024 * 1024;
	byte* buffer = new byte[N];
	time_t t = time(nullptr);
	srand(t);

	printf("Ptr=");
	std::string str;
	std::getline(std::cin, str);
    LPVOID lpAddress = (LPVOID)strtoull(str.c_str(), NULL, 16);
     
    EnableDebugPriv();

    PROCESSENTRY32 entry;
    entry.dwSize = sizeof(PROCESSENTRY32);

    HANDLE snapshot = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, NULL);

    if (Process32First(snapshot, &entry) == TRUE)
    {
        while (Process32Next(snapshot, &entry) == TRUE)
        {
            if (_stricmp(entry.szExeFile, "loop.exe") == 0)
            {
                HANDLE processHandle = OpenProcess(PROCESS_ALL_ACCESS, FALSE, entry.th32ProcessID);

                do
                {
                    ReadProcessMemory(processHandle, lpAddress, buffer, N, &bytesRW);
                    printf("Read %lld from %08llX\n", bytesRW, (unsigned long long)lpAddress);
                    for (int i = 0; i < 32; ++i)
                        printf("%02X ", buffer[i]);
                    printf("\n");

                    int M = (unsigned)rand() % N;
                    buffer[0] = (byte)~buffer[M];
                    printf("Corrupt %dth byte\n", M);
                    for (int i = 0; i < 32; ++i)
                        printf("%02X ", buffer[i]);
                    printf("\n");

                    WriteProcessMemory(processHandle, lpAddress, buffer, N, &bytesRW);
                    printf("Wrote %lld to %08llX\n", bytesRW, (unsigned long long)lpAddress);
                    printf("Press Enter to continue");
                    getchar();
                } while (true);

                CloseHandle(processHandle);
            }
        }
    }

    CloseHandle(snapshot);

}

void EnableDebugPriv()
{
    HANDLE hToken;
    LUID luid;
    TOKEN_PRIVILEGES tkp;

    OpenProcessToken(GetCurrentProcess(), TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, &hToken);

    LookupPrivilegeValue(NULL, SE_DEBUG_NAME, &luid);

    tkp.PrivilegeCount = 1;
    tkp.Privileges[0].Luid = luid;
    tkp.Privileges[0].Attributes = SE_PRIVILEGE_ENABLED;

    AdjustTokenPrivileges(hToken, false, &tkp, sizeof(tkp), NULL, NULL);

    CloseHandle(hToken);
}