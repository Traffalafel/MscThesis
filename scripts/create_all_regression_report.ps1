
$REGRESSION_VIEWS_DIR = "C:\Users\traff\src\repos\MscThesis\views\regression"
$DIR = "G:\My Drive\Dokumenter\DTU\Speciale\report\graphics\charts\regression"

Get-ChildItem $REGRESSION_VIEWS_DIR -File | %{ 
    echo $_.BaseName
    python scripts/render_regression.py $_.FullName $DIR
}
