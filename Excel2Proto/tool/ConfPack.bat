@echo ConfData Pack Start ......
set NAMESPACE=ConfigData
set PACKAGE_NAME=com.trinitigames.server.conf.auto

set EXCLE_PAHT=.\excel
set DATA_OUTPUT=.\client\data
set CSHARP_OUTPUT=.\client\scripts
set SERVER_DATAPATH=.\server\data
set SERVER_OUTPUT=.\server\class


set TEMP_PROTOFILE=.\_temp\proto_files
set SERVER_PROTOFILE=.\server\proto
set SERVER_LANGUGE=java

rem build script and data
python ..\src\python\excel_to_protobuf.py -i %EXCLE_PAHT% -n %NAMESPACE% -d %DATA_OUTPUT% -c %CSHARP_OUTPUT% -s %SERVER_OUTPUT% -l %SERVER_LANGUGE% -p %PACKAGE_NAME%
rem copy to server work folder
xcopy /s/y/i "%DATA_OUTPUT%\*.bin" "%SERVER_DATAPATH%" 

xcopy /s/y/i "%TEMP_PROTOFILE%\*.proto" "%SERVER_PROTOFILE%"

@echo ======== ConfData Pack End ========
pause