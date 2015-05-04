param(
    [Parameter(Mandatory=$true,Position=0)]
    [Alias("t,tgt")]
    [ValidateSet('Pack', 'Push')]
    [string] $Target,
    [Alias("f")]
    [switch] $Full,
    [Alias("c,cfg")]
    [ValidateSet('Debug', 'Release')]
    [string] $Configuration = 'Release',
	[Alias("v")]
    [string] $Version = '',
    [string] $id = 'DDay.iCal'
)

#region helper functions

function Invoke-BuildProject
{
    Param(
        [Parameter(Mandatory=$true)]
        [String] $proj,
        [String] $tgt = 'Build',
        [String] $v = 'm',
        [String] $props,
        [int] $m = 4
    )
    
    $msbuild = Get-MsBuild

    $opts = @("$proj", '/nologo', "/t:$tgt", "/m:$m", "/v:$v")

    if($props -ne $null -and $props -ne '')
    {
        $opts += ('/p:' + $props + '')
    }

    & $msbuild $opts
    if(!$?)
    {
        throw "Build error! See above."
    }
}

function Get-MsBuild
{
    $msbuilds = @('12.0', '4.0')

    foreach($ver in $msbuilds)
    {
        $r = ('HKLM:\SOFTWARE\Microsoft\MSBuild\ToolsVersions\{0}' -f $ver)
        if(Test-Path $r)
        {
            $p = $r | Get-ItemProperty -Name 'MSBuildToolsPath' | Select-Object -ExpandProperty 'MSBuildToolsPath'
            return "$p\msbuild.exe"
        }
    }

    throw "MsBuild not found"
}
#endregion

$dir = Split-Path -Parent -Path $MyInvocation.MyCommand.Path

if ($Version -eq '' -or $Version -eq $null) {
    $Version = Get-Content (Join-Path $dir Version.txt)
}

if ($Full) {
    $Configuration = 'Release'
} else {
    $Configuration = 'Debug'
    $Version += '-dev-' + (Get-Date (git show -s --format=%ai)).ToUniversalTime().ToString('yyyyMMddHHmmss')
}

switch($Target)
{    
    'Push'
    {
        & nuget push ((Join-Path $dir bin) + "\*.$Version.nupkg")
        & git push origin v$Version
    }
    'Pack'
    {
        Invoke-BuildProject -proj build.proj -tgt $Target -props "Configuration=$Configuration;Version=$Version"
        & nuget pack DDay.iCal.nuspec -Version $Version -Properties "id=$id;Configuration=$Configuration" -OutputDirectory (Join-Path $dir bin)
        & git tag v$Version
    }
}

