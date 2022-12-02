$pattern_old = '_DLB_(\d)'
$pattern_new = '_DLB_block$1'

ls .\Specifications\ -File -Recurse | % { 
    Rename-Item -Force $_ ($_.Name -replace $pattern_old, $pattern_new) 
}
ls .\results\ -File -Recurse | % { 
    Rename-Item -Force $_ ($_.Name -replace $pattern_old, $pattern_new) 
}