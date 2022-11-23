$test = Get-ChildItem "results" -Recurse -Filter "*.txt" | %{ @{Name=$_.BaseName} }

$test | ForEach-Object {
    echo $_.Name
}