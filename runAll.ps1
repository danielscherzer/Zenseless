$exeList = Get-ChildItem -Path $PSScriptRoot -Filter *.exe -Recurse
$exeList = $exeList | Select-Object FullName
$exeList = $exeList | Where-Object {$_.FullName -like '*\bin\*'} #| Out-GridView
foreach($exe in $exeList)
{
	& $exe.FullName
}
Write-Output "*********** finished **************"
