@echo ConfData Pack Start ......
set NAMESPACE=DongciDaci
set PACKAGE_NAME=com.trinitigames.server.conf.auto

set EXCLE_PATH=..\excel
set DATA_OUTPUT=..\..\Data\TableData
set CSHARP_OUTPUT=..\..\Assets\Scripts\Proto\ProtoDefine

set SERVER_DATAPATH=..\output\server\data
set SERVER_OUTPUT=..\output\server\class


set TEMP_PROTOFILE=..\output\_temp\proto_files
set SERVER_PROTOFILE=..\output\server\proto
set SERVER_LANGUGE=java

rem build script and data
python ..\src\python\excel_to_protobuf.py -i %EXCLE_PATH% -n %NAMESPACE% -d %DATA_OUTPUT% -c %CSHARP_OUTPUT% -s %SERVER_OUTPUT% -l %SERVER_LANGUGE% -p %PACKAGE_NAME%
rem copy to server work folder
xcopy /s/y/i "%DATA_OUTPUT%\*.bin" "%SERVER_DATAPATH%" 

xcopy /s/y/i "%TEMP_PROTOFILE%\*.proto" "%SERVER_PROTOFILE%"

@echo ======== ConfData Pack End ========
pause