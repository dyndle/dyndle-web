"%SONAR_SCANNER%" begin /k:"trivident_dyndle-web" /o:trivident-bitbucket /d:sonar.verbose=true /d:sonar.login=%SONAR_LOGIN% /d:sonar.host.url=https://sonarcloud.io
"%MSBUILD%" ..\src\Dyndle.Modules.sln /t:Rebuild 
"%SONAR_SCANNER%" end /d:sonar.login=%SONAR_LOGIN% 