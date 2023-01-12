
$CHARTS_DIR_REPORT_FINAL = "G:\My Drive\Dokumenter\DTU\Speciale\report\graphics\charts\final"

python scripts/render_results.py cGA_OneMax_final CpuTimeSeconds --fit="power" --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_FINAL
python scripts/render_results.py cGA_OneMax_final NumberFitnessCalls --fit="loglinear" --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_FINAL

python scripts/render_results.py FastMIMIC_OneMax_final CpuTimeSeconds --fit="power" --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_FINAL
python scripts/render_results.py FastMIMIC_OneMax_final NumberFitnessCalls --fit="logpower" --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_FINAL

python scripts/render_results.py P3_OneMax_final CpuTimeSeconds --fit="power" --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_FINAL
python scripts/render_results.py P3_OneMax_final NumberFitnessCalls --fit="linear" --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_FINAL

python scripts/render_results.py MIMIC_LeadingOnes_final CpuTimeSeconds --fit="power" --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_FINAL
python scripts/render_results.py MIMIC_LeadingOnes_final NumberFitnessCalls --fit="power" --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_FINAL

python scripts/render_results.py P3_LeadingOnes_final CpuTimeSeconds --fit="power" --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_FINAL
python scripts/render_results.py P3_LeadingOnes_final NumberFitnessCalls --fit="power" --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_FINAL

python scripts/render_results.py FastMIMIC_JOS_final CpuTimeSeconds --fit="power" --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_FINAL
python scripts/render_results.py FastMIMIC_JOS_final NumberFitnessCalls --fit="logsqrt" --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_FINAL

python scripts/render_results.py GOMEA_JOS_final CpuTimeSeconds --fit="power" --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_FINAL
python scripts/render_results.py GOMEA_JOS_final NumberFitnessCalls --fit="power" --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_FINAL

python scripts/render_results.py cGA_DLB_final BestFitness --fit="sqrt" --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_FINAL
python scripts/render_results.py cGA_DLB_final CpuTimeSeconds --fit="power" --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_FINAL

python scripts/render_results.py FastMIMIC_DLB_final CpuTimeSeconds --fit="power" --report --compare="MIMIC_DLB_final,GOMEA_DLB_final" --compare-fit="power,power" --compare-own-x --clean-legend --save --output-dir=$CHARTS_DIR_REPORT_FINAL
python scripts/render_results.py FastMIMIC_DLB_final NumberFitnessCalls --fit="power" --report --compare="MIMIC_DLB_final,GOMEA_DLB_final" --compare-fit="power,power" --clean-legend --save --output-dir=$CHARTS_DIR_REPORT_FINAL

python .\scripts\render_results.py FastMIMIC_LeadingOnes_final CpuTimeSeconds --fit="power" --compare="MIMIC_LeadingOnes_final,GOMEA_LeadingOnes_final" --compare-fit="power,power" --clean-legend --report --compare-own-x --save --output-dir=$CHARTS_DIR_REPORT_FINAL
python .\scripts\render_results.py FastMIMIC_LeadingOnes_final NumberFitnessCalls --fit="power" --compare="MIMIC_LeadingOnes_final,GOMEA_LeadingOnes_final" --compare-fit="power,power" --clean-legend --report --compare-own-x --y-limit="2e6" --x-limit=250 --save --output-dir=$CHARTS_DIR_REPORT_FINAL