using Genetic.Lifeforms;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Genetic.Genes
{
    public class MathGene
    {
        public const double MUTATION_RATE = 0.001;
        private const int GENE_LENGTH = 20;
        private static Lazy<Random> Random = new Lazy<Random>(() => new Random());
        private string Gene { get; set; }

        public MathGene(string gene)
        {
            this.Gene = gene;
        }

        public static MathGene Generate()
        {
            var bits = Enumerable
                .Range(0, GENE_LENGTH)
                .Select(_ => (MathGene.Random.Value.NextDouble() >= 0.5) ? '1' : '0').ToArray();
            return new MathGene(new String(bits));
        }

        public Equation Decode()
        {
            return new Equation(this.Gene);
        }

        public string Encode()
        {
            return this.Gene;
        }

        public MathGene Clone()
        {
            return new MathGene(this.Gene);
        }

        public MathGene Crossover(MathGene other)
        {
            int crossPoint = MathGene.Random.Value.Next(0, MathGene.GENE_LENGTH);
            string myLeft = this.Gene.Substring(0, crossPoint);
            string myRight = this.Gene.Substring(crossPoint, MathGene.GENE_LENGTH - crossPoint);
            string otherLeft = other.Gene.Substring(0, crossPoint);
            string otherRight = other.Gene.Substring(crossPoint, MathGene.GENE_LENGTH - crossPoint);

            this.Gene = String.Concat(myLeft, otherRight);
            return new MathGene(String.Concat(otherLeft, myRight));
        }

        public void Mutate(double rate = MUTATION_RATE)
        {
            var bits = this.Gene.ToCharArray();
            for (int i = 0; i < bits.Length; ++i)
            {
                if (MathGene.Random.Value.NextDouble() <= MathGene.MUTATION_RATE)
                {
                    // Flip between 0 and 1
                    bits[i] = (char)(('1' - bits[i]) + '0');
                }
            }
            this.Gene = new String(bits);
        }
    }
}