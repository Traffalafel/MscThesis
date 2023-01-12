
$REGRESSION_VIEWS_DIR = "C:\Users\traff\src\repos\MscThesis\views\regression"
$REGRESSION_CHARTS_DIR = "G:\My Drive\Dokumenter\DTU\Speciale\raw charts\regression"

Get-ChildItem $REGRESSION_VIEWS_DIR -File | %{ 
    echo $_.BaseName
    python scripts/render_regression.py $_.FullName $REGRESSION_CHARTS_DIR
}
