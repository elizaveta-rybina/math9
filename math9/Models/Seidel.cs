

using System.Collections.Generic;
using System.Linq;

namespace math9.Models
{
    public class Seidel
    {
        public Seidel(Matrix systemMatrix, List<double> freeVector)
        {
            SystemMatrix = systemMatrix;
            FreeVector = freeVector;
			Approximation = FreeVector.Zip(SystemMatrix.GetMainDiagonal()).Select(elem => elem.First / elem.Second).ToList();
		}

		public Matrix SystemMatrix { get; init; }
        public List<double> FreeVector { get; init; }
        public List<double> Approximation { get; private set; }

		private double RowSumWithoutCur(int indexOfRow)
		{
			return SystemMatrix[indexOfRow]
				.Zip(Approximation)
				.Select(elem => elem.First * elem.Second)
				.Where((elemValue, elemIndex) => elemIndex != indexOfRow)
				.Aggregate(0d, (accumulated, current) => accumulated + current);
		}
		public IEnumerable<double> Solve(double eps)
		{
			var currentEps = 0d;
			do
			{
				var oldValue = new List<double>(Approximation);

				for (int i = 0; i < Approximation.Count; ++i)
				{
					Approximation[i] = (FreeVector[i] - RowSumWithoutCur(i)) / SystemMatrix[i, i];
				}

				currentEps = oldValue.Zip(Approximation).Select(elem => (elem.Second - elem.First) / elem.Second).Max()!;
			} while (currentEps > eps);

			return Approximation;
		}
	}
}
