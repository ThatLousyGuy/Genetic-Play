using Genetic.Lifeforms;
using System;

namespace Genetic.Fitness
{
    public class NumberFitness
    {
        public double BaseNum { get; set; }
        
        public NumberFitness(double num)
        {
            this.BaseNum = num;
        }

        public double GetScore(Equation equation)
        {
            double score = 0;
            try
            {
                double value = (double)(equation.Evaluate(null));
                double dist = Math.Abs(this.BaseNum - value);
                score = 1 / (dist + 1);
            } catch (DivideByZeroException) { score = 0; }
            return (!score.Equals(Double.NaN)) ? score : 0;
        }
    }
}