$dest_path = Join-Path "results" "iteration2"

Get-ChildItem "./results" -Filter "*.txt" | ForEach-Object {
    $spec_path = Join-Path "Specifications" "$($_.BaseName).json"
    
    if (Test-Path $spec_path) {
        Write-Output $_.Name
        Move-Item $_.FullName $dest_path
    }

}
