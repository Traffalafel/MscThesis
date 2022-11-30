
# Script for downloading results files from HPC cluster and moving them
# to the proper folders

$DTU_USER_ID = "s210520"
$SSH_KEYS_DIR_PATH = "~/Desktop/.ssh/gbar"
$SPECS_DIR_PATH = "Specifications"
$RESULTS_DIR_PATH = "results"
$RESULTS_TMP_DIR_PATH = "results_tmp"

if (Test-Path $RESULTS_TMP_DIR_PATH) {
    Write-Output "Cannot continue, directory already exists: $($RESULTS_TMP_DIR_PATH)"
    return
}

mkdir $RESULTS_TMP_DIR_PATH

scp -r -i $SSH_KEYS_DIR_PATH "${DTU_USER_ID}@login.hpc.dtu.dk:results/*.txt" $RESULTS_TMP_DIR_PATH

$specs = Get-ChildItem $SPECS_DIR_PATH -Recurse -Filter "*.json" | %{ @{Directory=$_.DirectoryName;Name=$_.BaseName} }

Get-ChildItem $RESULTS_TMP_DIR_PATH -Filter "*.txt" | ForEach-Object {

    $name = $_.BaseName
    $specs_match = $specs.Where({ $_.Name -eq $name }, 'First')

    if ($specs_match.Count -eq 0) {
        Write-Output "Could not find match for $($name)"
        return
    }

    $spec = $specs_match[0]
    $dir_name = $spec.Directory | Split-Path -Leaf
    $target_dir = Join-Path $RESULTS_DIR_PATH $dir_name
    if (-Not (Test-Path $target_dir)) {
        mkdir $target_dir
    }

    $path_new = Join-Path $target_dir $_.Name

    if (-Not (Test-Path $path_new)) {
        Move-Item $_.FullName $path_new
    }
}

Remove-Item -Recurse -Force $RESULTS_TMP_DIR_PATH