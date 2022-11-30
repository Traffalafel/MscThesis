$file_path = $args[0]
$PREV_VAL = "0.01"

$new_vals = $("1", "0.75", "0.5", "0.25", "0.1", "0.0001", "0.001")

$file_name = Split-Path $file_path -Leaf

$new_vals | ForEach-Object {

    $file_name_new = $file_name.Replace($PREV_VAL, $_)
    $file_path_new = Join-Path "Specifications" "iteration2" $file_name_new
    
    Copy-Item -Force $file_path $file_path_new
    
    $content = Get-Content $file_path_new -Raw
    $content = $content.Replace($PREV_VAL, $_)
    $content | Set-Content $file_path_new

}
