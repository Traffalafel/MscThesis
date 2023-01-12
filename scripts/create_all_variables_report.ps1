
$CHARTS_DIR_REPORT_VARIABLE = "G:\My Drive\Dokumenter\DTU\Speciale\report\graphics\charts\variable"

python scripts/render_results.py GOMEA_LeadingOnes BestFitness --variable=Pop --x-limit=400 --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py GOMEA_LeadingOnes CpuTimeSeconds --variable=Pop --x-limit=400 --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE

python scripts/render_results.py cGA_OneMax CpuTimeSeconds --variable=K --x-limit=160 --y-limit=0.5 --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py cGA_OneMax NumberFitnessCalls --variable=K --x-limit=160 --y-limit=15000 --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE

python scripts/render_results.py FastMIMIC_OneMax CpuTimeSeconds --variable=Sampling --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py FastMIMIC_OneMax NumberFitnessCalls --variable=Sampling --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE

python scripts/render_results.py MIMIC_OneMax CpuTimeSeconds --variable=Pop --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py MIMIC_OneMax NumberFitnessCalls --variable=Pop --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE

python scripts/render_results.py MIMIC_LeadingOnes CpuTimeSeconds --y-limit=45 --variable=Pop --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py MIMIC_LeadingOnes NumberFitnessCalls --variable=Pop --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE

python scripts/render_results.py FastMIMIC_LeadingOnes BestFitness --variable=Sampling --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py FastMIMIC_LeadingOnes CpuTimeSeconds --variable=Sampling --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE

python scripts/render_results.py cGA_JOS CpuTimeSeconds --variable=K --x-limit=150 --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py cGA_JOS NumberFitnessCalls --variable=K --x-limit=150 --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE

python .\scripts\render_results.py P3_JOS BestFitness --variable="Gap" --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python .\scripts\render_results.py P3_JOS NumberFitnessCalls --variable="Gap" --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE

python scripts/render_results.py FastMIMIC_DLB BestFitness --variable=Sampling --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py FastMIMIC_DLB CpuTimeSeconds --variable=Sampling --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE

python scripts/render_results.py TourMIMIC_UniformTSP BestFitness --variable=Pop --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py TourMIMIC_UniformTSP NumberFitnessCalls --variable=Pop --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE

python scripts/render_results.py TourGOMEA_UniformTSP BestFitness --variable=Pop --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py TourGOMEA_UniformTSP NumberFitnessCalls --variable=Pop --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE

python scripts/render_results.py FastTourMIMIC_UniformTSP BestFitness --variable=Sampling --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py FastTourMIMIC_UniformTSP NumberFitnessCalls --variable=Sampling --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE

python scripts/render_results.py FastP4_UniformTSP BestFitness --variable=Shedding --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py FastP4_UniformTSP NumberFitnessCalls --variable=Shedding --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE

python scripts/render_results.py TourMIMIC_TSP BestFitness --variable=Perturbation --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE;
python scripts/render_results.py TourMIMIC_TSP CpuTimeSeconds --variable=Perturbation --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE;
python scripts/render_results.py TourGOMEA_TSP BestFitness --variable=Perturbation --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE;
python scripts/render_results.py TourGOMEA_TSP CpuTimeSeconds --variable=Perturbation --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE;

