REM install SonarScanner for MSBuild (https://docs.sonarqube.org/latest/analysis/scan/sonarscanner-for-msbuild/)
REM create an environment variable SONAR_SCANNER with the path to SonarScanner.MSBuild.exe
%SONAR_SCANNER% begin /k:"trivident_dyndle-web" /o:trivident-bitbucket /d:sonar.verbose=true
"C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\MSBuild.exe" "D:\Code\Dyndle\dyndle-web\src\Dyndle.Modules.sln" /t:Rebuild 
%SONAR_SCANNER% end