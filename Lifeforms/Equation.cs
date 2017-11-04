using Genetic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Genetic.Lifeforms
{
    public class Equation
    {
        public string EquationString { get; private set; }
        private static Dictionary<string, string> demux;
        public Dictionary<string, string> Demux
        {
            get { return demux; }
            set { demux = value; }
        }

        public Equation(string gene)
        {
            this.Initialize();
            this.Decode(gene);
        }
        
        public object Evaluate(object obj)
        {
            return MathParser.Parse(this.EquationString);
        }
        
        private void Initialize()
        {
            if (this.Demux == null)
            {
                this.Demux = this.Codify(new List<string> {
                    "0",
                    "1",
                    "2",
                    "3",
                    "4",
                    "5",
                    "6",
                    "7",
                    "8",
                    "9",
                    "+",
                    "-",
                    "*",
                    "/",
                });
            }
        }

        private void Decode(string gene)
        {
            int sectionLength = 4;
            var parts = new List<string>();
            for (int i = 0; i + sectionLength <= gene.Length; i += sectionLength)
            {
                string section = gene.Substring(i, sectionLength);
                parts.Add(this.ProcessSection(section));
            }
            this.EquationString = String.Concat(parts);
        }

        private string ProcessSection(string section)
        {
            if (this.Demux.ContainsKey(section))
            {
                return this.Demux[section];
            };
            return String.Empty;
        }

        private Dictionary<string, string> Codify(IEnumerable<string> elements)
        {
            int count = elements.Count();
            int bitsRequired = 0;
            do
            {
                count >>= 1;
                ++bitsRequired;
            } while (count > 0);

            return Enumerable
                .Range(0, elements.Count())
                .Select(i => Convert.ToString(i, 2).PadLeft(bitsRequired, '0'))
                .Zip(elements, (k, v) => new { k, v })
                .ToDictionary(_ => _.k, _ => _.v);

        }
    }
}