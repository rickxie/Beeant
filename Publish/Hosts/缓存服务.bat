@echo off
if not "%~1"=="p" start /min cmd.exe /c %0 p&exit
start ICacheService\Beeant.Distributed.Service.Host.exe
