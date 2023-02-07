
$CHARTS_DIR_REPORT_VARIABLE = "G:\My Drive\Dokumenter\DTU\Speciale\report\graphics\charts\variable"

python scripts/render_results.py cGA_OneMax BestFitness --title="Best fitness" --variable=K --x-limit=160 --report --ignore-sizes=50,150,250 --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py cGA_OneMax CpuTimeSeconds --title="CPU time" --variable=K --x-limit=160 --y-limit=0.5 --report --ignore-sizes=50,150,250 --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py cGA_OneMax NumberFitnessCalls --title="Fitness evaluations" --variable=K --x-limit=160 --y-limit=15000 --report --ignore-sizes=50,150,250 --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE

python scripts/render_results.py MIMIC_OneMax BestFitness --title="Best fitness" --variable=Pop --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py MIMIC_OneMax CpuTimeSeconds --title="CPU time" --variable=Pop --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py MIMIC_OneMax NumberFitnessCalls --title="Fitness evaluations" --variable=Pop --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE

python scripts/render_results.py FastMIMIC_OneMax BestFitness --title="Best fitness" --variable=Sampling --report --ignore-sizes=100 --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py FastMIMIC_OneMax CpuTimeSeconds --title="CPU time" --variable=Sampling --report --ignore-sizes=100 --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py FastMIMIC_OneMax NumberFitnessCalls --title="Fitness evaluations" --variable=Sampling --report --ignore-sizes=100 --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE

python scripts/render_results.py FastMIMIC_OneMax_betterSampling BestFitness --title="Best fitness" --variable=Pop --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py FastMIMIC_OneMax_betterSampling CpuTimeSeconds --title="CPU time" --variable=Pop --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py FastMIMIC_OneMax_betterSampling NumberFitnessCalls --title="Fitness evaluations" --variable=Pop --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE

python scripts/render_results.py GOMEA_OneMax BestFitness --title="Best fitness" --variable=Pop --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py GOMEA_OneMax CpuTimeSeconds --title="CPU time" --variable=Pop --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py GOMEA_OneMax NumberFitnessCalls --title="Fitness evaluations" --variable=Pop --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE

python scripts/render_results.py cGA_LeadingOnes BestFitness --title="Best fitness" --variable=K --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py cGA_LeadingOnes CpuTimeSeconds --title="CPU time" --variable=K --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py cGA_LeadingOnes NumberFitnessCalls --title="Fitness evaluations" --variable=K --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE

python scripts/render_results.py MIMIC_LeadingOnes BestFitness --title="Best fitness" --variable=Pop --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py MIMIC_LeadingOnes CpuTimeSeconds --title="CPU time" --y-limit=45 --variable=Pop --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py MIMIC_LeadingOnes NumberFitnessCalls --title="Fitness evaluations" --variable=Pop --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE

python scripts/render_results.py FastMIMIC_LeadingOnes BestFitness --title="Best fitness" --variable=Sampling --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py FastMIMIC_LeadingOnes CpuTimeSeconds --title="CPU time" --variable=Sampling --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py FastMIMIC_LeadingOnes NumberFitnessCalls --title="Fitness evaluations" --variable=Sampling --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE

python scripts/render_results.py FastMIMIC_LeadingOnes_betterSampling BestFitness --title="Best fitness" --variable=Pop --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py FastMIMIC_LeadingOnes_betterSampling CpuTimeSeconds --title="CPU time" --variable=Pop --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py FastMIMIC_LeadingOnes_betterSampling NumberFitnessCalls --title="Fitness evaluations" --variable=Pop --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE

python scripts/render_results.py GOMEA_LeadingOnes BestFitness --title="Best fitness" --ignore-sizes="20,30,40" --variable=Pop --x-limit=400 --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py GOMEA_LeadingOnes CpuTimeSeconds --title="CPU time" --ignore-sizes="20,30,40" --variable=Pop --x-limit=400 --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py GOMEA_LeadingOnes NumberFitnessCalls --title="Fitness evaluations" --ignore-sizes="20,30,40" --variable=Pop --x-limit=400 --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE

python scripts/render_results.py cGA_JOS BestFitness --title="Best fitness" --ignore-sizes="50,150,250" --variable=K --x-limit=150 --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py cGA_JOS CpuTimeSeconds --title="CPU time" --ignore-sizes="50,150,250" --variable=K --x-limit=150 --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py cGA_JOS NumberFitnessCalls --title="Fitness evaluations" --ignore-sizes="50,150,250" --variable=K --x-limit=150 --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE

python scripts/render_results.py MIMIC_JOS BestFitness --title="Best fitness" --variable=Pop --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py MIMIC_JOS CpuTimeSeconds --title="CPU time" --variable=Pop --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py MIMIC_JOS NumberFitnessCalls --title="Fitness evaluations" --variable=Pop --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE

python scripts/render_results.py FastMIMIC_JOS BestFitness --title="Best fitness" --variable=Sampling --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py FastMIMIC_JOS CpuTimeSeconds --title="CPU time" --variable=Sampling --report --y-limit=7 --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py FastMIMIC_JOS NumberFitnessCalls --title="Fitness evaluations" --variable=Sampling --report --y-limit=0.05e6 --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE

python scripts/render_results.py FastMIMIC_JOS_betterSampling BestFitness --title="Best fitness" --variable=Pop --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py FastMIMIC_JOS_betterSampling CpuTimeSeconds --title="CPU time" --variable=Pop --report --y-limit=6 --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py FastMIMIC_JOS_betterSampling NumberFitnessCalls --title="Fitness evaluations" --variable=Pop --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE

python scripts/render_results.py GOMEA_JOS BestFitness --title="Best fitness" --variable=Pop --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py GOMEA_JOS CpuTimeSeconds --title="CPU time" --variable=Pop --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py GOMEA_JOS NumberFitnessCalls --title="Fitness evaluations" --variable=Pop --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE

python .\scripts\render_results.py P3_JOS BestFitness --title="Best fitness" --variable="Gap" --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python .\scripts\render_results.py P3_JOS CpuTimeSeconds --title="CPU time" --variable="Gap" --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python .\scripts\render_results.py P3_JOS NumberFitnessCalls --title="Fitness evaluations" --variable="Gap" --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE

python scripts/render_results.py cGA_DLB BestFitness --title="Best fitness" --ignore-sizes="50,150,250" --variable=K --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py cGA_DLB CpuTimeSeconds --title="CPU time" --ignore-sizes="50,150,250" --variable=K --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py cGA_DLB NumberFitnessCalls --title="Fitness evaluations" --ignore-sizes="50,150,250" --variable=K --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE

python scripts/render_results.py MIMIC_DLB BestFitness --title="Best fitness" --variable=Pop --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py MIMIC_DLB CpuTimeSeconds --title="CPU time" --variable=Pop --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py MIMIC_DLB NumberFitnessCalls --title="Fitness evaluations" --variable=Pop --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE

python scripts/render_results.py FastMIMIC_DLB BestFitness --title="Best fitness" --variable=Sampling --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py FastMIMIC_DLB CpuTimeSeconds --title="CPU time" --variable=Sampling --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py FastMIMIC_DLB NumberFitnessCalls --title="Fitness evaluations" --variable=Sampling --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE

python scripts/render_results.py FastMIMIC_DLB_betterSampling BestFitness --title="Best fitness" --variable=Pop --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py FastMIMIC_DLB_betterSampling CpuTimeSeconds --title="CPU time" --variable=Pop --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py FastMIMIC_DLB_betterSampling NumberFitnessCalls --title="Fitness evaluations" --variable=Pop --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE

python scripts/render_results.py GOMEA_DLB BestFitness --title="Best fitness" --ignore-sizes="20,30,40" --variable=Pop --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py GOMEA_DLB CpuTimeSeconds --title="CPU time" --ignore-sizes="20,30,40" --variable=Pop --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py GOMEA_DLB NumberFitnessCalls --title="Fitness evaluations" --ignore-sizes="20,30,40" --variable=Pop --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE

python scripts/render_results.py TourMIMIC_UniformTSP BestFitness --title="Best fitness" --variable=Pop --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py TourMIMIC_UniformTSP CpuTimeSeconds --title="CPU time" --variable=Pop --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py TourMIMIC_UniformTSP NumberFitnessCalls --title="Fitness evaluations" --variable=Pop --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE

python scripts/render_results.py TourMIMIC_TSP BestFitness --title="Best fitness" --variable=Pop --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE;
python scripts/render_results.py TourMIMIC_TSP CpuTimeSeconds --title="CPU time" --variable=Pop --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE;
python scripts/render_results.py TourMIMIC_TSP NumberFitnessCalls --title="Fitness evaluations" --variable=Pop --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE;

python scripts/render_results.py TourMIMIC_TSP BestFitness --title="Best fitness" --variable=Perturbation --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE;
python scripts/render_results.py TourMIMIC_TSP CpuTimeSeconds --title="CPU time" --variable=Perturbation --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE;
python scripts/render_results.py TourMIMIC_TSP NumberFitnessCalls --title="Fitness evaluations" --variable=Perturbation --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE;

python scripts/render_results.py TourGOMEA_UniformTSP BestFitness --title="Best fitness" --variable=Pop --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py TourGOMEA_UniformTSP CpuTimeSeconds --title="CPU time" --variable=Pop --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py TourGOMEA_UniformTSP NumberFitnessCalls --title="Fitness evaluations" --variable=Pop --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE

python scripts/render_results.py TourGOMEA_TSP BestFitness --title="Best fitness" --variable=Pop --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE;
python scripts/render_results.py TourGOMEA_TSP CpuTimeSeconds --title="CPU time" --variable=Pop --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE;
python scripts/render_results.py TourGOMEA_TSP NumberFitnessCalls --title="Fitness evaluations" --variable=Pop --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE;

python scripts/render_results.py TourGOMEA_TSP BestFitness --title="Best fitness" --variable=Perturbation --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE;
python scripts/render_results.py TourGOMEA_TSP CpuTimeSeconds --title="CPU time" --variable=Perturbation --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE;
python scripts/render_results.py TourGOMEA_TSP NumberFitnessCalls --title="Fitness evaluations" --variable=Perturbation --hide-legend --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE;

python scripts/render_results.py FastTourMIMIC_UniformTSP BestFitness --title="Best fitness" --variable=Sampling --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py FastTourMIMIC_UniformTSP CpuTimeSeconds --title="CPU time" --variable=Sampling --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py FastTourMIMIC_UniformTSP NumberFitnessCalls --title="Fitness evaluations" --variable=Sampling --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE

python scripts/render_results.py FastTourMIMIC_UniformTSP BestFitness --title="Best fitness" --variable=Pop --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py FastTourMIMIC_UniformTSP CpuTimeSeconds --title="CPU time" --variable=Pop --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py FastTourMIMIC_UniformTSP NumberFitnessCalls --title="Fitness evaluations" --variable=Pop --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE

python scripts/render_results.py FastTourMIMIC_TSP BestFitness --title="Best fitness" --variable=Sampling --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py FastTourMIMIC_TSP CpuTimeSeconds --title="CPU time" --variable=Sampling --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py FastTourMIMIC_TSP NumberFitnessCalls --title="Fitness evaluations" --variable=Sampling --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE

python scripts/render_results.py FastTourMIMIC_TSP BestFitness --title="Best fitness" --variable=Pop --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py FastTourMIMIC_TSP CpuTimeSeconds --title="CPU time" --variable=Pop --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py FastTourMIMIC_TSP NumberFitnessCalls --title="Fitness evaluations" --variable=Pop --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE

python scripts/render_results.py FastP4_UniformTSP BestFitness --title="Best fitness" --variable=Shedding --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py FastP4_UniformTSP CpuTimeSeconds --title="CPU time" --variable=Shedding --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE
python scripts/render_results.py FastP4_UniformTSP NumberFitnessCalls --title="Fitness evaluations" --variable=Shedding --report --save --output-dir=$CHARTS_DIR_REPORT_VARIABLE


