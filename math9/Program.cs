using System;
using System.Collections.Generic;
using math9.Models;

namespace math9
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Метод Гаусса: ");
            var matrixSingle = new List<List<double>>();
            matrixSingle.Add(new List<double> { 3, 2, -5 });
            matrixSingle.Add(new List<double> { 2, -1, 3 });
            matrixSingle.Add(new List<double> { 1, 2, -1 });
            var freeVectorSingle = new List<double> { -1, 13, 9 };

            var matrixInf = new List<List<double>>();
            matrixInf.Add(new List<double> { 1, 3, -2, -2 });
            matrixInf.Add(new List<double> { -1, -2, 1, 2 });
            matrixInf.Add(new List<double> { -2, -1, 3, 1 });
            matrixInf.Add(new List<double> { -3, -2, 3, 3 });
            var freeVectorInf = new List<double> { -3, 2, -2, -1 };

            var matrixNone = new List<List<double>>();
            matrixNone.Add(new List<double> { 4, -3, 2, -1 });
            matrixNone.Add(new List<double> { 3, -2, 1, -3 });
            matrixNone.Add(new List<double> { 5, -3, 1, -8 });
            var freeVectorNone = new List<double> { 8, 7, 1 };

            var systemMatrix = new Matrix(matrixNone);
            var gauss = new Gauss(systemMatrix, freeVectorNone);
            switch (gauss.NumSolutions())
            {
                case SolutionsCount.None:
                    {
                        Console.WriteLine("Нет решений.");
                        break;
                    }
                case SolutionsCount.Single:
                    {
                        foreach (var solution in gauss.SolveSingle())
                        {
                            Console.Write($"{solution}\t");
                        }
                        break;
                    }
                case SolutionsCount.Infinity:
                    {
                        foreach (var solution in gauss.SolveInfinite())
                        {
                            Console.WriteLine(solution);
                        }
                        break;
                    }
            }

            Console.WriteLine("Метод Зейделя: ");
            var matrixSingleSeidel = new List<List<double>>();
            matrixSingleSeidel.Add(new List<double> { 5, -1, 3 });
            matrixSingleSeidel.Add(new List<double> { 1, -4, 2 });
            matrixSingleSeidel.Add(new List<double> { 2, -1, 5 });
            var freeVectorSingleSeidel = new List<double> { 5, 20, 10 };

            var systemMatrixSeidel = new Matrix(matrixSingleSeidel);
            var seidel = new Seidel(systemMatrixSeidel, freeVectorSingleSeidel);
            foreach (var row in systemMatrixSeidel)
            {
                foreach (var elem in row)
                {
                    Console.Write($"{elem}\t");
                }
                Console.WriteLine();
            }
            foreach (var solution in seidel.Solve(0.001))
            {
                Console.Write($"{solution} ");
            }
            Console.WriteLine();
        }
    }
}
