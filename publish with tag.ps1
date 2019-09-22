$timeStamp = Get-Date -UFormat "%Y.%m.%d_%H.%M.%S"
git tag $timeStamp
git push origin
git push origin $timeStamp