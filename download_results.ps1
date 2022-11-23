
# Script for downloading results files from HPC cluster and moving them
# to the proper folders

$DTU_USER_ID = "s210520"
$SSH_KEYS_DIR_PATH = "~/Desktop/.ssh/gbar"
$SPECS_DIR_PATH = "Specifications"
$RESULTS_DIR_PATH = "results"
$RESULTS_TMP_DIR_PATH = "results_tmp"

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
    $dir_name = $spec.Directory

    $path_new = Join-Path $RESULTS_DIR_PATH $dir_name $_.Name
    Move-Item $_.FullName $path_new
}

Remove-Item $RESULTS_TMP_DIR_PATH