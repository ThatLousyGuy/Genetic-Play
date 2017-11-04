using Genetic.Fitness;
using Genetic.Genes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Genetic
{
    class Program
    {
        private static Lazy<Random> Random = new Lazy<Random>(() => new Random());
        class ScoredGene
        {
            public double Score { get; set; }
            public MathGene Gene { get; set; }

        }
        static void Main(string[] args)
        {
            NumberFitness fitness = new NumberFitness(256);

            int generation = 0;
            int n = 200;
            double crossoverRate = 0.7;
            var genes = Enumerable
                .Range(0, n)
                .Select(_ => MathGene.Generate())
                .ToList();
            var scoredGenes = genes
                .Select(_ => fitness.GetScore(_.Decode()))
                .Zip(genes, (score, gene) => new ScoredGene { Score = score, Gene = gene })
                .ToList();
            MathGene solution = scoredGenes.FirstOrDefault(_ => _.Score == 1)?.Gene;

            while (solution == null)
            {
                // Find genes to pass on
                var selectedGenes = new List<MathGene>(n);
                genes = new List<MathGene>(n);
                while (selectedGenes.Count < n)
                {
                    var randomGene = Program.GetRouletteGene(scoredGenes);
                    if (randomGene != null)
                    {
                        selectedGenes.Add(randomGene);
                    }
                }

                // Apply crossover and mutation
                for (int i = 0; i < selectedGenes.Count - 1; i += 2)
                {
                    var firstGene = selectedGenes[i];
                    var secondGene = selectedGenes[i+1];
                    double crossoverChance = Program.Random.Value.NextDouble();
                    if (crossoverChance <= crossoverRate)
                    {
                        secondGene = firstGene.Crossover(secondGene);
                    }
                    firstGene.Mutate();
                    secondGene.Mutate();
                    genes.Add(firstGene);
                    genes.Add(secondGene);
                }
                
                scoredGenes = genes
                    .Select(_ => fitness.GetScore(_.Decode()))
                    .Zip(genes, (score, gene) => new ScoredGene { Score = score, Gene = gene })
                    .ToList();
                solution = scoredGenes.FirstOrDefault(_ => _.Score == 1)?.Gene;

                generation++;
                Console.WriteLine($"generation {generation}");
                foreach (var gene in scoredGenes)
                {
                    Console.WriteLine($"{gene.Gene.Decode().EquationString} score: {gene.Score}");
                }
            }
            
            Console.WriteLine($"{solution.Decode().EquationString} generations: {generation}");
        }

        private static MathGene GetRouletteGene(List<ScoredGene> scoredGenes)
        {
            double scoreSum = scoredGenes.Sum(_ => _.Score);
            double chance = 0;
            double chanceSelect = Program.Random.Value.NextDouble() * scoreSum;
            for (int i = 0; i < scoredGenes.Count(); ++i)
            {
                chance += scoredGenes[i].Score;
                if (chance > chanceSelect)
                {
                    return scoredGenes[i].Gene;
                }
            }
            return null;
        }
    }
}
