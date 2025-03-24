function SafeRemoveDir {
    param ([Parameter(ValueFromPipeline=$true)][string]$path)
    process {
        if (Test-Path $path) {
            Remove-Item -Recurse -Force $path
        }
    }
}