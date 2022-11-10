
# Script for generating HPC run-scripts from specifications and copying these to 
# the remote HPC-server using ssh

$DTU_USER_ID = "s210520"
$SSH_KEYS_DIR_PATH = "~/Desktop/.ssh/gbar"
$SPECS_DIR_PATH = "Specifications"
$SCRIPTS_DIR_PATH = "hpc_scripts"
$TEMPLATE_FILE_PATH = "run_script_template.txt"
$RESULTS_DIR_PATH = "results"

if (Test-Path $results_path) {
    # clear local scripts folder
    Get-ChildItem $SCRIPTS_DIR_PATH | ForEach-Object { $_.Delete() }
}
else {
    # create local scripts folder if missing
    New-Item $SCRIPTS_DIR_PATH -ItemType Directory
}

$template = Get-Content $TEMPLATE_FILE_PATH

Get-ChildItem $SPECS_DIR_PATH -Filter "*.json" | ForEach-Object {
    
    $name = $_.BaseName
    
    $results_file = $name + ".txt"
    $results_path = Join-Path $RESULTS_DIR_PATH $results_file
    if (Test-Path $results_path -PathType Leaf) {
        Write-Output "Skipping ${name}"
        return # skip if results file already exists
    }
    
    Write-Output "Creating script for ${name}"
    $file_name = $name + ".sh"
    $file_path = Join-Path $SCRIPTS_DIR_PATH $file_name
    $contents = $template.Replace("{name}", $name)
    $contents | Out-File $file_path
}

# delete remote scripts folder
ssh -i $SSH_KEYS_DIR_PATH "${DTU_USER_ID}@login.hpc.dtu.dk" 'rm -r -f hpc_scripts'

# copy scripts folder to remote
scp -r -i $SSH_KEYS_DIR_PATH $SCRIPTS_DIR_PATH "${DTU_USER_ID}@login.hpc.dtu.d:hpc_scripts"

# copy specifications to remote
scp -r -i $SSH_KEYS_DIR_PATH $SPECS_DIR_PATH "${DTU_USER_ID}@login.hpc.dtu.d:."
