$pattern_old = 'MIMIC_DLB_block(\d)'
$pattern_new = 'MIMIC_DLB_block$1_constantPop'

ls .\Specifications\iteration2 -File -Recurse | % { 
    Rename-Item -Force $_ ($_.Name -replace $pattern_old, $pattern_new) 
}
ls .\results\iteration2 -File -Recurse | % { 
    Rename-Item -Force $_ ($_.Name -replace $pattern_old, $pattern_new) 
}