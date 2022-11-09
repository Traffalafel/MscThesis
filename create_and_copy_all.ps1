
if ($args.Count -ne 3) {
    Write-Output "Usage: <specs_dir_path> <output_dir_path> <template_file_path>"
    return
}

$specs_dir_path = $args[0]
$scripts_dir_path = $args[1]
$template_file_path = $args[2]

# clear local scripts folder
Get-ChildItem $scripts_dir_path | ForEach-Object { $_.Delete() }

$template = Get-Content $template_file_path

Get-ChildItem $specs_dir_path -Filter "*.json" | ForEach-Object {
    $name = $_.BaseName
    $file_name = $name + ".sh"
    $file_path = Join-Path $scripts_dir_path $file_name
    $contents = $template.Replace("{name}", $name)
    $contents | Out-File $file_path
}

# delte remote scripts folder
ssh -i ~/Desktop/.ssh/gbar s210520@login.hpc.dtu.dk 'rm -r -f hpc_scripts; rm -r -f v1_MscThesis/Specifications'

scp -r -i ~/Desktop/.ssh/gbar hpc_scripts s210520@login.hpc.dtu.dk:hpc_scripts
scp -r -i ~/Desktop/.ssh/gbar $specs_dir_path s210520@login.hpc.dtu.dk:v1_MscThesis/Specifications
