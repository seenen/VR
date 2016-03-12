@ECHO OFF

::Variables definition
SET ProjectLocation=E:\Heros\GameEditors\Tool_AssetBundles
SET UnityLocation="D:\Program Files (x86)\Unity\Editor\Unity.exe"
SET In=E:\Heros\GameEditors\Tool_AssetBundles\Assets\2dsound
SET 
SET Out=F:\TestOut


::-logFile %cd%\log.txt
::Execution
%UnityLocation% -batchmode -projectPath %ProjectLocation% -executeMethod Main.EntryCommand -quit -CustomArgs:Language=en_US,Version=1.02,InFolder=%In%,OutFolder=%Out%

pause