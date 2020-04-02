$gitbranch = git rev-parse --abbrev-ref HEAD
$whoami = whoami
$env:SonarScannerVersion = $env:username+":"+$gitbranch
Write-Host $env:SonarScannerVersion
#cmd /c "sonar-scanner.bat"