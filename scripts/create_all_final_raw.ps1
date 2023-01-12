
$RESULTS_FINAL_DIR = "results/final"
$FINAL_CHARTS_DIR = "G:\My Drive\Dokumenter\DTU\Speciale\raw charts\final"

Get-ChildItem $RESULTS_FINAL_DIR -File | %{

    python scripts/render_results.py $_.BaseName BestFitness --line --hide-legend --save --output-dir=$FINAL_CHARTS_DIR
    python scripts/render_results.py $_.BaseName CpuTimeSeconds --line --hide-legend --save --output-dir=$FINAL_CHARTS_DIR
    python scripts/render_results.py $_.BaseName NumberFitnessCalls --line --hide-legend --save --output-dir=$FINAL_CHARTS_DIR

}