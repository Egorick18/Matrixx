public class Exceptions
{
    public class MatrixException : Exception
    {
        public MatrixException(string message) : base(message) { }
    }

    public class NonSquareMatrixException : MatrixException
    {
        public NonSquareMatrixException() : base("Матрица должна быть квадратной") { }
    }

    public class SingularMatrixException : MatrixException
    {
        public SingularMatrixException() : base("Матрица вырождена, обратной матрицы не существует") { }
    }
}