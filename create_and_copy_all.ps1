
# Script for generating HPC run-scripts from specifications and copying these to 
# the remote HPC-server using ssh

$DTU_USER_ID = "s210520"
$SSH_KEYS_DIR_PATH = "~/Desktop/.ssh/gbar"
$SPECS_DIR_PATH = "Specifications"
$SPECS_DIR_TMP_PATH = "specs_tmp"
$SCRIPTS_DIR_PATH = "hpc_scripts"
$TEMPLATE_FILE_PATH = "run_script_template.txt"
$RESULTS_DIR_PATH = "results"

if (Test-Path $SCRIPTS_DIR_PATH) {
    Remove-Item $SCRIPTS_DIR_PATH -Recurse -Force
}
if (Test-Path $SPECS_DIR_TMP_PATH) {
    Remove-Item $SPECS_DIR_TMP_PATH -Recurse -Force
}
mkdir $SCRIPTS_DIR_PATH
mkdir $SPECS_DIR_TMP_PATH

$template = Get-Content $TEMPLATE_FILE_PATH
$results_names = Get-ChildItem $RESULTS_DIR_PATH -Recurse -Filter "*.txt" | %{ @{Name=$_.BaseName} }

Get-ChildItem $SPECS_DIR_PATH -Recurse -Filter "*.json" | ForEach-Object {
    
    $name = $_.BaseName
    
    if ($results_names.Where({ $_.Name -eq $name }, 'First').Count -gt 0) {
        echo $_.BaseName
        return
    }

    Write-Output "Creating script for ${name}"
    $file_name = $name + ".sh"
    $file_path = Join-Path $SCRIPTS_DIR_PATH $file_name
    $contents = $template.Replace("{name}", $name)
    $contents | Out-File $file_path

    Copy-Item $_ $SPECS_DIR_TMP_PATH
}

# delete remote scripts folder
ssh -i $SSH_KEYS_DIR_PATH "${DTU_USER_ID}@login.hpc.dtu.dk" "rm -r -f hpc_scripts"

# copy scripts folder to remote
scp -r -i $SSH_KEYS_DIR_PATH $SCRIPTS_DIR_PATH "${DTU_USER_ID}@login.hpc.dtu.dk:hpc_scripts"

# copy specifications to remote
scp -i $SSH_KEYS_DIR_PATH $SPECS_DIR_TMP_PATH/*.json "${DTU_USER_ID}@login.hpc.dtu.dk:Specifications"
