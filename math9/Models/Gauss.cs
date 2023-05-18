

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace math9.Models
{
    public enum SolutionsCount : ushort
    {
        None,
        Single,
        Infinity
    }

    public class Gauss
    {
        public Matrix SystemMatrix { get; init; }
        public List<double> FreeVector { get; init; }
        private Matrix Extended; // расширенная матрица

        public struct RowCell // хранит только одну ячейку строки
        {
            public int Index;
            public double Value;

            public RowCell(int index, double value)
            {
                Index = index;
                Value = value;
            }

            public override string ToString()
            {
                if (Value != 0d)
                {
                    return $"{(Value > 0 ? "+" : "-")} {Math.Abs(Value)}*x{Index} ";
                }
                return "";
            }
        }

        public class EquantionRowView : IEnumerable<RowCell> // хранит всю строчку
        {
            public EquantionRowView(List<RowCell> cells, double freeValue, int index)
            {
                Cells = cells;
                FreeValue = freeValue;
                Index = index;
            }

            public EquantionRowView(double freeValue, int index)
            {
                Cells = new List<RowCell>();
                FreeValue = freeValue;
                Index = index;
            }

            public void Add(RowCell cell)
            {
                Cells.Add(cell);
            }

            public List<RowCell> Cells { get; private set; }
            public double FreeValue { get; init; } // один из элементов столбца свободных членов
            public int Index { get; init; } // индекс текущей строки

            public override string ToString()
            {
                var sb = new StringBuilder($"x{Index} = ");
                foreach (var cell in Cells)
                {
                    sb.Append(cell);
                }

                return sb.Append($"{(FreeValue > 0 ? "+" : "-")} {Math.Abs(FreeValue)}").ToString();
            }
            public IEnumerator<RowCell> GetEnumerator()
            {
                return Cells.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return Cells.GetEnumerator();
            }
        } 

        public Gauss(Matrix systemMatrix, IEnumerable<double> freeVector)
        {
            SystemMatrix = systemMatrix;
            FreeVector = new List<double>(freeVector);

            Extended = new Matrix(systemMatrix);
            Extended.AddColumn(freeVector);
        }

        private static List<double> DevideRow(IEnumerable<double> row, double divider) // делит все элементы на одно число
        {
            return row.Select(elem => elem / divider).ToList();
        }

        public bool IsSolvable() //Теорема Кронекера-Капелли
        {
            return SystemMatrix.Rank == Extended.Rank;
        }

        public SolutionsCount NumSolutions()
        {
            if (!IsSolvable())
            {
                return SolutionsCount.None;
            }

            if (SystemMatrix.Rank == SystemMatrix.ColumnsCount)
            {
                return SolutionsCount.Single;
            }

            return SolutionsCount.Infinity;
        } // Проверяет сколько решений (следствие теоремы)

        public IEnumerable<double> SolveSingle() // Для одного решения
        {
            Extended = Extended.ToUpperTriangle();
            Extended = Extended.ToLowerTriangle();

            for(int i = 0; i < Extended.RowsCount; i++)
            {
                Extended[i][^1] /= Extended[i][i];
                Extended[i][i] = 1;
            }

            return Extended.Select(x => x.Last());
        }

        public IEnumerable<EquantionRowView> SolveInfinite()
        {
            Extended = Extended.ToUpperTriangle();
            Extended = Extended.ToLowerTriangle();

            var freeValuesList =
                Enumerable.Range(0, Extended.ColumnsCount - 1) // генерирует все элементы 
                .Except(Extended.GetMainDiagonal()
                    .Select((elem, index) => new { item = elem, index = index })
                    .Where(elem => elem.item != 0)
                    .Select(elem => elem.index)
                ).ToArray();

            for(int i = 0; i < Extended.RowsCount; i++)
            {
                Extended[i] = DevideRow(Extended[i], Extended[i, i]).ToList();
                Extended[i, i] = 1d;
            }

            var result = new List<EquantionRowView>();
            for (int i = 0; i < Extended.RowsCount; i++)
            {
                if (freeValuesList.Contains(i)) // не будем формировать для незначимых переменных
                    continue;

                var infSolution = new EquantionRowView(Extended[i][^1], i);

                foreach (var freeIndex in freeValuesList)
                {
                    infSolution.Add(new RowCell(freeIndex, -1 * Extended[i, freeIndex]));
                }

                result.Add(infSolution);
            }

            return result;
        }
    }
}
