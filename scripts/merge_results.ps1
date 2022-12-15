# Merge results 
$to_merge = Get-ChildItem "results" -Recurse -File -Filter "*_pt*.txt" | %{ @{Directory=$_.DirectoryName;Name=($_.BaseName -replace "_pt\d+", "")} } | Sort-Object -Property {$_.Name} -Unique
$to_merge | %{
    echo $_.Name
    python scripts/merge_results.py $_.Name $_.Directory
}
