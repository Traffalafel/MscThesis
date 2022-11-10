
# Script for generating HPC run-scripts from specifications and copying these to 
# the remote HPC-server using ssh

$SPECS_DIR_PATH = "Specifications"
$SCRIPTS_DIR_PATH = "hpc_scripts"
$TEMPLATE_FILE_PATH = "run_script_template.txt"
$RESULTS_DIR_PATH = "results"

# clear local scripts folder
Get-ChildItem $SCRIPTS_DIR_PATH | ForEach-Object { $_.Delete() }

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
ssh -i ~/Desktop/.ssh/gbar s210520@login.hpc.dtu.dk 'rm -r -f hpc_scripts'

# copy scripts folder to remote
scp -r -i ~/Desktop/.ssh/gbar hpc_scripts s210520@login.hpc.dtu.dk:hpc_scripts

# copy specifications to remote
scp -r -i ~/Desktop/.ssh/gbar $SPECS_DIR_PATH s210520@login.hpc.dtu.dk:.
