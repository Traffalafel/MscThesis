
$REGRESSION_VIEWS_DIR = "C:\Users\traff\src\repos\MscThesis\views\regression"

Get-ChildItem $REGRESSION_VIEWS_DIR -File | %{ 
    echo $_.BaseName
    python scripts/render_regression.py $_.FullName save
}
