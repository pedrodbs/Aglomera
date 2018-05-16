ECHO OFF
REM automatically converts html export of README.md to README.aml that can then be edited as the main page of the API documentation
REM has to have ConvertHtmlToMaml.exe from "Sandcastle Help File Builder" accessible from PATH
SET mpath=%~dp0
RMDIR "%mpath%..\..\docs\" /s /q
ConvertHtmlToMaml "%mpath%..\.." "%mpath%Content"
PAUSE