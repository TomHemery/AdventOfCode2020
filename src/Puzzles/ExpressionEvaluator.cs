using System;
using System.Text.RegularExpressions;
using System.IO;
namespace AdventOfCode2020
{
    public static class ExpressionEvaluator
    {
        public static void Evaluate(string expressionFile)
        {
            ulong total = 0;
            foreach(string line in File.ReadLines(expressionFile))
            {
                var root = new Expr(Regex.Replace(line, @"\s+", ""));
                ulong val = root.Evaluate();
                Console.WriteLine("{0} = {1}", line, val);
                total = checked(total + val);
            }
            Console.WriteLine("Total: {0}", total);
        }

        private class Expr
        {
            public Sum left = null;
            public Expr right = null;

            public Expr(string expr){
                int bracketCount = 0;

                for(int i = 0; i < expr.Length; i ++)
                {
                    char symbol = expr[i];
                    switch(symbol)
                    {
                        case '(':
                            bracketCount++;
                            break;
                        case ')':
                            bracketCount--;
                            break;
                        case '*':
                            if(bracketCount == 0){
                                left = new Sum(expr.Substring(0, i));
                                right = new Expr(expr.Substring(i + 1));
                                return;
                            }
                            break;

                    }
                }
                left = new Sum(expr);
            }

            public ulong Evaluate()
            {
                if(right == null){
                    return left.Evaluate();
                } else {
                    return left.Evaluate() * right.Evaluate();
                }
            }
        }

        private class Sum
        {
            public Value left;
            public Sum right;

            public Sum(string expr){
                int bracketCount = 0;

                for(int i = 0; i < expr.Length; i ++)
                {
                    char symbol = expr[i];
                    switch(symbol)
                    {
                        case '(':
                            bracketCount++;
                            break;
                        case ')':
                            bracketCount--;
                            break;
                        case '+':
                            if(bracketCount == 0){
                                left = new Value(expr.Substring(0, i));
                                right = new Sum(expr.Substring(i + 1));
                                return;
                            }
                            break;
                    }
                }
                left = new Value(expr);
            }

            public ulong Evaluate()
            {
                if(right == null){
                    return left.Evaluate();
                } else {
                    return left.Evaluate() + right.Evaluate();
                }
            }
        }

        private class Value
        {
            public ulong val;
            public Expr bracketedExpr = null;

            public Value(string expr)
            {
                if(!ulong.TryParse(expr, out val)){
                    bracketedExpr = new Expr(expr.Substring(1, expr.Length - 2));
                }
            }

            public ulong Evaluate()
            {
                if(bracketedExpr == null){
                    return val;
                } else {
                    return bracketedExpr.Evaluate();
                }
            }
        }
    }
}