set WORKSPACE=..
set LUBAN_DLL=Tools\Luban\Luban.dll
set CONF_ROOT=.

dotnet %LUBAN_DLL% ^
    -t all ^
    -d json ^
	-c cs-simple-json ^
    --conf %CONF_ROOT%\luban.conf ^
	-x outputCodeDir=..\Assets\Scripts\Business\LubanConfig\GenCode ^
    -x outputDataDir=..\Assets\ToBundle\Config\LubanConfigs

pause