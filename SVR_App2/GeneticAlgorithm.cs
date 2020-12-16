using System;
using System.Diagnostics;
using Accord;
using Accord.Genetic;
using Accord.Math.Random;
using Accord.Statistics.Distributions.Univariate;

namespace SupportVectorRegression
{

public class GeneticAlgorithm
{
      // genetic population
        private Population population;
        // size of population
        private int populationSize;

        // number of weight in the search space dimension
        private int numberOfNetworksWeights;

        // generator for newly generated neurons
        private IRandomNumberGenerator<double> chromosomeGenerator;
        // mutation generators
        private IRandomNumberGenerator<double> mutationMultiplierGenerator;
        private IRandomNumberGenerator<double> mutationAdditionGenerator;

        // selection method for chromosomes in population
        private ISelectionMethod selectionMethod;

        // crossover probability in genetic population
        private double crossOverRate;
        // mutation probability in genetic population
        private double mutationRate;
        // probability to add newly generated chromosome to population
        private double randomSelectionRate;

   public GeneticAlgorithm(int populationSize, int numberOfWeights,
            IRandomNumberGenerator<double> chromosomeGenerator,
            IRandomNumberGenerator<double> mutationMultiplierGenerator,
            IRandomNumberGenerator<double> mutationAdditionGenerator,
            ISelectionMethod selectionMethod,
            double crossOverRate, double mutationRate, double randomSelectionRate)
        {
           
            this.numberOfNetworksWeights =numberOfWeights;

            // population parameters
            this.populationSize = populationSize;
            this.chromosomeGenerator = chromosomeGenerator;
            this.mutationMultiplierGenerator = mutationMultiplierGenerator;
            this.mutationAdditionGenerator = mutationAdditionGenerator;
            this.selectionMethod = selectionMethod;
            this.crossOverRate = crossOverRate;
            this.mutationRate = mutationRate;
            this.randomSelectionRate = randomSelectionRate;
        }


             public double RunEpoch(double[][] input, double[][] output)
        {
            Debug.Assert(input.Length > 0);
            Debug.Assert(output.Length > 0);
            Debug.Assert(input.Length == output.Length);
            
             // check if it is a first run and create population if so
            if (population == null)
            {
                // sample chromosome
                DoubleArrayChromosome chromosomeExample = new DoubleArrayChromosome(
                    chromosomeGenerator, mutationMultiplierGenerator, mutationAdditionGenerator,
                    numberOfNetworksWeights);

                // create population ...
               // population = new Population(populationSize, chromosomeExample, new EvolutionaryFitness(network, input, output), selectionMethod);
                // ... and configure it
                population.CrossoverRate = crossOverRate;
                population.MutationRate = mutationRate;
                population.RandomSelectionPortion = randomSelectionRate;
            }

            // run genetic epoch
            population.RunEpoch();

            // get best chromosome of the population
            DoubleArrayChromosome chromosome = (DoubleArrayChromosome)population.BestChromosome;
            double[] chromosomeGenes = chromosome.Value;

            // put best chromosome's value into neural network's weights
            int v = 0;

           
            return 1.0 / chromosome.Fitness;
        }

    
}


}