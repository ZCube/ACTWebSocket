$a = cmd /c git describe --abbrev=0
(Get-Content AssemblyInfo.cs.in) | ForEach-Object { $_ -replace "@VERSION_SHORT@", $a } | Set-Content AssemblyInfo.cs
$a = cmd /c git describe --dirty=-dev
(Get-Content AssemblyInfo.cs) | ForEach-Object { $_ -replace "@VERSION_LONG@", $a } | Set-Content AssemblyInfo.cs