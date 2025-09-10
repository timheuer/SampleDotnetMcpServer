param(
    [string]$Template,
    [string]$Output,
    [string]$Version
)

# If $Version is not provided, get it from environment variable MCP_VERSION
if (-not $Version) {
    $Version = $env:MCP_VERSION
    if (-not $Version) {
        Write-Error "No version specified. Pass -Version or set MCP_VERSION env var."
        exit 1
    }
}

Get-Content $Template -Raw |
ForEach-Object { $_ -replace '__VERSION__', $Version } |
Set-Content $Output -Encoding utf8
