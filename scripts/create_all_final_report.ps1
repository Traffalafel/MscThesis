
$CHARTS_DIR_REPORT_FINAL = "G:\My Drive\Dokumenter\DTU\Speciale\report\graphics\charts\final"

python scripts/render_results.py cGA_OneMax_final CpuTimeSeconds --title="CPU time" --fit="power" --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_FINAL
python scripts/render_results.py cGA_OneMax_final NumberFitnessCalls --title="Fitness evaluations" --fit="loglinear" --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_FINAL

python scripts/render_results.py MIMIC_OneMax_final CpuTimeSeconds --title="CPU time" --fit="power" --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_FINAL
python scripts/render_results.py MIMIC_OneMax_final NumberFitnessCalls --title="Fitness evaluations" --fit="linear" --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_FINAL

python scripts/render_results.py FastMIMIC_OneMax_final CpuTimeSeconds --title="CPU time" --fit="power" --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_FINAL
python scripts/render_results.py FastMIMIC_OneMax_final NumberFitnessCalls --title="Fitness evaluations" --fit="power" --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_FINAL

python scripts/render_results.py GOMEA_OneMax_final CpuTimeSeconds --title="CPU time" --fit="power" --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_FINAL
python scripts/render_results.py GOMEA_OneMax_final NumberFitnessCalls --title="Fitness evaluations" --fit="power" --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_FINAL

python scripts/render_results.py P3_OneMax_final CpuTimeSeconds --title="CPU time" --fit="power" --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_FINAL
python scripts/render_results.py P3_OneMax_final NumberFitnessCalls --title="Fitness evaluations" --fit="linear" --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_FINAL

python scripts/render_results.py cGA_LeadingOnes_final CpuTimeSeconds --title="CPU time" --fit="power" --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_FINAL
python scripts/render_results.py cGA_LeadingOnes_final NumberFitnessCalls --title="Fitness evaluations" --fit="power" --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_FINAL

python scripts/render_results.py MIMIC_LeadingOnes_final CpuTimeSeconds --title="CPU time" --fit="power" --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_FINAL
python scripts/render_results.py MIMIC_LeadingOnes_final NumberFitnessCalls --title="Fitness evaluations" --fit="power" --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_FINAL

python scripts/render_results.py FastMIMIC_LeadingOnes_final CpuTimeSeconds --title="CPU time" --fit="power" --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_FINAL
python scripts/render_results.py FastMIMIC_LeadingOnes_final NumberFitnessCalls --title="Fitness evaluations" --fit="power" --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_FINAL

python .\scripts\render_results.py GOMEA_LeadingOnes_final CpuTimeSeconds --title="CPU time" --fit="power" --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_FINAL
python .\scripts\render_results.py GOMEA_LeadingOnes_final NumberFitnessCalls --title="Fitness evaluations" --fit="power" --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_FINAL

python .\scripts\render_results.py GOMEA_LeadingOnes_final CpuTimeSeconds --title="CPU time" --fit="power" --compare="MIMIC_LeadingOnes_final,FastMIMIC_LeadingOnes_final" --compare-fit="power,power" --clean-legend --report --y-limit=200 --x-limit=340 --save --output-dir=$CHARTS_DIR_REPORT_FINAL
python .\scripts\render_results.py GOMEA_LeadingOnes_final NumberFitnessCalls --title="Fitness evaluations" --fit="power" --compare="MIMIC_LeadingOnes_final,FastMIMIC_LeadingOnes_final" --compare-fit="power,power" --clean-legend --report --y-limit="2e6" --x-limit=275 --save --output-dir=$CHARTS_DIR_REPORT_FINAL

python scripts/render_results.py P3_LeadingOnes_final CpuTimeSeconds --title="CPU time" --fit="power" --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_FINAL
python scripts/render_results.py P3_LeadingOnes_final NumberFitnessCalls --title="Fitness evaluations" --fit="power" --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_FINAL

python scripts/render_results.py cGA_JOS_final CpuTimeSeconds --title="CPU time" --fit="power" --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_FINAL
python scripts/render_results.py cGA_JOS_final NumberFitnessCalls --title="Fitness evaluations" --fit="power" --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_FINAL

python scripts/render_results.py MIMIC_JOS_final CpuTimeSeconds --title="CPU time" --fit="power" --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_FINAL
python scripts/render_results.py MIMIC_JOS_final NumberFitnessCalls --title="Fitness evaluations" --fit="linear" --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_FINAL

python scripts/render_results.py FastMIMIC_JOS_final CpuTimeSeconds --title="CPU time" --fit="power" --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_FINAL
python scripts/render_results.py FastMIMIC_JOS_final NumberFitnessCalls --title="Fitness evaluations" --fit="logsqrt" --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_FINAL

python scripts/render_results.py GOMEA_JOS_final CpuTimeSeconds --title="CPU time" --fit="power" --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_FINAL
python scripts/render_results.py GOMEA_JOS_final NumberFitnessCalls --title="Fitness evaluations" --fit="power" --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_FINAL

python scripts/render_results.py P3_JOS_final CpuTimeSeconds --title="CPU time" --fit="cubic" --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_FINAL
python scripts/render_results.py P3_JOS_final NumberFitnessCalls --title="Fitness evaluations" --fit="loglinear" --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_FINAL

python scripts/render_results.py cGA_DLB_final BestFitness --title="Best fitness" --fit="sqrt" --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_FINAL
python scripts/render_results.py cGA_DLB_final CpuTimeSeconds --title="CPU time" --fit="power" --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_FINAL
python scripts/render_results.py cGA_DLB_final NumberFitnessCalls --title="Fitness evaluations" --fit="power" --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_FINAL

python scripts/render_results.py MIMIC_DLB_final CpuTimeSeconds --title="CPU time" --fit="power" --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_FINAL
python scripts/render_results.py MIMIC_DLB_final NumberFitnessCalls --title="Fitness evaluations" --fit="cubic" --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_FINAL

python scripts/render_results.py FastMIMIC_DLB_final CpuTimeSeconds --title="CPU time" --fit="power" --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_FINAL
python scripts/render_results.py FastMIMIC_DLB_final NumberFitnessCalls --title="Fitness evaluations" --fit="power" --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_FINAL

python scripts/render_results.py GOMEA_DLB_final CpuTimeSeconds --title="CPU time" --fit="power" --report --hide-legend --save --output-dir=$CHARTS_DIR_REPORT_FINAL
python scripts/render_results.py GOMEA_DLB_final NumberFitnessCalls --title="Fitness evaluations" --fit="power" --report --hide-legend --save --output-dir=$CHARTS_DIR_REPORT_FINAL

python scripts/render_results.py GOMEA_DLB_final CpuTimeSeconds --title="CPU time" --fit="power" --report --compare="MIMIC_DLB_final,FastMIMIC_DLB_final" --compare-fit="power,power" --clean-legend --y-limit=120 --save --output-dir=$CHARTS_DIR_REPORT_FINAL
python scripts/render_results.py GOMEA_DLB_final NumberFitnessCalls --title="Fitness evaluations" --fit="power" --report --compare="MIMIC_DLB_final,FastMIMIC_DLB_final" --compare-fit="power,power" --clean-legend --y-limit=1.1e7 --save --output-dir=$CHARTS_DIR_REPORT_FINAL

python scripts/render_results.py P3_DLB_final CpuTimeSeconds --title="CPU time" --fit="power" --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_FINAL
python scripts/render_results.py P3_DLB_final NumberFitnessCalls --title="Fitness evaluations" --fit="power" --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_FINAL

python scripts/render_results.py P4_UniformTSP CpuTimeSeconds --title="CPU time" --fit="power" --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_FINAL
python scripts/render_results.py P4_UniformTSP NumberFitnessCalls --title="Fitness evaluations" --fit="power" --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_FINAL
