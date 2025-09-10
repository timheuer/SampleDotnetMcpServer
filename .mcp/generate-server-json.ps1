param(
    [string]$Template,
    [string]$Output,
    [string]$Version
)

Get-Content $Template -Raw |
ForEach-Object { $_ -replace '__VERSION__', $Version } |
Set-Content $Output -Encoding utf8
