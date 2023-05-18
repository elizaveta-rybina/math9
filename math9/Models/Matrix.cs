
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace math9.Models
{
    public class Matrix : IEnumerable<List<double>>
	{
		public Matrix()
		{
			Data = new List<List<double>>();
		}
		public Matrix(IEnumerable<IEnumerable<double>> matrix)
        {
			ValidateInputArguments(matrix);
			Data = matrix.Select(item => item.ToList()).ToList();
        }

		public int ColumnsCount => Data[0].Count();

		public int RowsCount => Data.Count();

		public List<List<double>> Data { get; init; } // храним данные тут 

		public int Rank
		{
			get
			{
				var _data = ToUpperTriangle(Data);

				int result = 0;
				foreach (var row in _data)
				{
					result += isNotNullRow(row) ? 1 : 0;
				}

				return result;
			}
		}

		private static void ValidateInputArguments(IEnumerable<IEnumerable<double>> matrix)
		{
			if (matrix == null)
			{
				throw new ArgumentNullException("Ошибка! Матрица пустая.");
			}

			var size = matrix.ElementAt(0).Count(); // смотрим количество элементов в первой строчке матрицы
			foreach (var line in matrix)
			{
				if (line.Count() != size)
				{
					throw new ArgumentException("Ошибка! Входная матрица ступенчатая.");
				}
			}
		}

		private static Matrix ToUpperTriangleUnchecked(IEnumerable<IEnumerable<double>> matrix) // возвращает матрицу в верхнетреугольном виде (ступенчатая)
		{
			var _data = matrix.Select(item => item.ToList()).ToList();

			for (int i = 1; i < _data.Count(); ++i)
			{
				for (int k = 0; k < i; ++k)
				{
					if (_data[k][k] != 0)
					{
						var multiplier = _data[i][k] / _data[k][k];
						for(int j = 0; j < _data[0].Count; j++)
						{
							_data[i][j] -= _data[k][j] * multiplier;
						}
					}
				}
			}
			return new Matrix(_data);
		}

		private static Matrix ToLowerTriangleUnchecked(IEnumerable<IEnumerable<double>> matrix) // возвращает матрицу в нижнетреугольном виде (ступенчатая)
		{
			var _data = matrix.Select(item => item.ToList()).ToList();

			for (int i = _data.Count - 2; i >= 0; --i)
			{
				for (int k = _data.Count - 1; k > i; --k)
				{
					if (_data[k][k] != 0)
					{
						var multiplier = _data[i][k] / _data[k][k];
						for (int j = 0; j < _data[0].Count; j++)
						{
							_data[i][j] -= _data[k][j] * multiplier;
						}
					}
				}
			}

			return new Matrix(_data);
		}

		public static Matrix ToUpperTriangle(IEnumerable<IEnumerable<double>> matrix)
		{
			ValidateInputArguments(matrix);

			return ToUpperTriangleUnchecked(matrix);
		}

		public static Matrix ToLowerTriangle(IEnumerable<IEnumerable<double>> matrix)
		{
			ValidateInputArguments(matrix);

			return ToLowerTriangleUnchecked(matrix);
		}

		public Matrix ToUpperTriangle()
		{
			return ToUpperTriangleUnchecked(Data);
		}

		public Matrix ToLowerTriangle()
		{
			return ToLowerTriangleUnchecked(Data);
		}

		private static IEnumerable<double> GetMainDiagonalUnchecked(IEnumerable<IEnumerable<double>> matrix)
		{
			for (int i = 0; i < matrix.Count(); ++i)
			{
				yield return matrix.ElementAt(i).ElementAt(i);
			}
		}
		public IEnumerable<double> GetMainDiagonal()
		{
			return GetMainDiagonalUnchecked(Data);
		}

		public static IEnumerable<double> GetMainDiagonal(IEnumerable<IEnumerable<double>> matrix)
		{
			ValidateInputArguments(matrix);

			return GetMainDiagonalUnchecked(matrix);
		}

		private static bool isNotNullRow(IEnumerable<double> row)
		{
			foreach (var elem in row)
			{
				if (elem != 0)
				{
					return true;
				}
			}

			return false;
		}

		public void AddColumn(IEnumerable<double> column)
		{
			if (column.Count() != Data.Count())
			{
				throw new ArgumentException("The size of the input column must match the size of the matrix columns.");
			}

			for (int i = 0; i < Data.Count(); ++i)
			{
				Data[i].Add(column.ElementAt(i));
			}
		}

        public IEnumerator<List<double>> GetEnumerator()
        {
            return Data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
			return Data.GetEnumerator();
        }

        
        public List<double> this[int index]
		{
			get => Data[index];
			set => Data[index] = value;
		}
		public double this[int row, int column]
		{
			get => Data[row][column];
			set => Data[row][column] = value;
		}
	}
}
