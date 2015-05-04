#!/bin/sh

MyDir=$(dirname $0)

#echo "& { $MyDir/release.ps1 $@ }"
powershell -nologo -noprofile -command "& $MyDir/release.ps1 $@"