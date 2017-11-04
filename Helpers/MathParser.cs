using System;
using System.Collections.Generic;
using System.Linq;

namespace Genetic.Helpers
{
    public static class MathParser
    {
        public delegate double Operator(double x, double y);
        private static Dictionary<char, Operator> Operators = new Dictionary<char, Operator>
        {
            {'+' , (x, y) => x + y},
            {'-' , (x, y) => x - y},
            {'*' , (x, y) => x * y},
            {'/' , (x, y) => x / y},
        };

        public static double Parse(string equation)
        {
            int i = 0;
            while (i < equation.Length && !MathParser.IsNumber(equation[i]))
            {
                ++i;
            }
            if (i >= equation.Length)
            {
                return 0;
            }

            double total = equation[i++] - '0';
            Operator op = null;
            bool needOperator = true;
            for (; i < equation.Length; ++i)
            {
                if (needOperator && MathParser.IsOperator(equation[i]))
                {
                    op = MathParser.Operators[equation[i]];
                    needOperator = false;
                }
                else if (!needOperator && MathParser.IsNumber(equation[i]))
                {
                    total = op(total, equation[i] - '0');
                    needOperator = true;
                }
            }

            return total;
        }

        public static bool IsOperator(char symbol)
        {
            return Operators.ContainsKey(symbol);
        }

        public static bool IsNumber(char symbol)
        {
            return symbol >= '0' && symbol <= '9';
        }
    }
}