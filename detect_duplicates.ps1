
$SPECS_DIR = "Specifications"

Get-ChildItem $SPECS_DIR -File -Recurse | % {

    $matches = Get-ChildItem $SPECS_DIR -Recurse -Filter $_.Name

    if (($matches | Measure-Object).Count -gt 1) {
        echo $_.FullName
    }
}
