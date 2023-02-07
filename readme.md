## MscThesis

This repository contains code for Thomas Träff's Master's thesis in Computer Science & Engineering at the Technical University of Denmark (DTU).
The project ran from September 2022 to February 2023.

Title: Implementation and evaluation of multivariate
estimation-of-distribution algorithms

If you are interested in more about information about the project, the thesis *should* be publicly available at [DTU Findit](https://findit.dtu.dk).
You are also welcome to fork the code and use it for any purpose.

The main goal of the code is to implement a framework for running empirical performance tests of estimation-of-distribution algorithms (EDA) on optimization problems. 
Several EDAs and problems are also implemented alongside the framework.

## Usage

Open the `MscThesis.sln` solution file using Visual Studio or any other IDE that supports .NET development.

To run the user interface, select `MscThesis.UI` as the startup project in VS and run the project. 
The current project configuration has a preset called "Windows Machine", but since the UI uses .NET MAUI it should support running on other platforms such as Linux and Mac. 

To run the framework using the command-line interface (CLI), build the `MscThesis.CLI` project and find the outputted executable somewhere in the `bin` folder. 
Before running this executable file, the configuration file should be changed to point to a TSPLIB95 folder.
This config file is located in the same directory as the `MscThesis.CLI` executable, and on Windows it should be called `MscThesis.CLI.dll.config`.
Simply change the value of the `TSPLibDirectoryPath` parameter to the path of the `TSPLib.Net/TSPLIB95` folder from the repository root.

The CLI executable takes two parameters: an output results folder and an input specification JSON file. 
Examples of specification files can be found in the `Specifications` folder, which contains the test suite for my project. 

Have fun!