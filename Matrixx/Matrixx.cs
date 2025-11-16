using System;
using System.Text;

public class Matrix
{
    protected double[,] data;
    protected int rows;
    protected int cols;

    public Matrix(double[,] array)
    {
        try
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            rows = array.GetLength(0);
            cols = array.GetLength(1);
            data = new double[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    data[i, j] = array[i, j];
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при создании матрицы: {ex.Message}");
            rows = 1;
            cols = 1;
            data = new double[1, 1];
        }
    }
    public Matrix(int rows, int cols)
    {
        try
        {
            if (rows <= 0 || cols <= 0)
                throw new ArgumentException("Размеры матрицы должны быть положительными числами");

            this.rows = rows;
            this.cols = cols;
            data = new double[rows, cols];
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при создании матрицы: {ex.Message}");
            this.rows = 1;
            this.cols = 1;
            data = new double[1, 1];
        }
    }
    public Matrix(Matrix other)
    {
        try
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));
            rows = other.rows;
            cols = other.cols;
            data = new double[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    data[i, j] = other.data[i, j];
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при копировании матрицы: {ex.Message}");
            rows = 1;
            cols = 1;
            data = new double[1, 1];
        }
    }
    public int Rows => rows;
    public int Cols => cols;
    public double this[int row, int col]
    {
        get
        {
            try
            {
                if (row < 0 || row >= rows || col < 0 || col >= cols)
                    throw new IndexOutOfRangeException("Индекс находится вне границ матрицы");
                return data[row, col];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при доступе к элементу матрицы: {ex.Message}");
                return 0;
            }
        }
        set
        {
            try
            {
                if (row < 0 || row >= rows || col < 0 || col >= cols)
                    throw new IndexOutOfRangeException("Индекс находится вне границ матрицы");
                data[row, col] = value;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при установке элемента матрицы: {ex.Message}");
            }
        }
    }
    public override string ToString()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < rows; i++)
            {
                sb.Append("[ ");
                for (int j = 0; j < cols; j++)
                {
                    sb.Append(data[i, j].ToString("F2"));
                    if (j < cols - 1)
                        sb.Append(", ");
                }
                sb.Append(" ]\n");
            }
            return sb.ToString();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при преобразовании матрицы в строку: {ex.Message}");
            return "[ Ошибка ]";
        }
    }
    public bool IsIdentical(Matrix other)
    {
        try
        {
            if (other == null || rows != other.rows || cols != other.cols)
                return false;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (Math.Abs(data[i, j] - other.data[i, j]) > 1e-10)
                        return false;
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при сравнении матриц: {ex.Message}");
            return false;
        }
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(rows, cols);
    }
    public static Matrix operator +(Matrix a, Matrix b)
    {
        try
        {
            if (a == null || b == null)
                throw new ArgumentNullException("Матрицы не могут быть null");
            if (a.rows != b.rows || a.cols != b.cols)
                throw new ArgumentException("Размеры матриц должны совпадать для сложения");
            Matrix result = new Matrix(a.rows, a.cols);
            for (int i = 0; i < a.rows; i++)
            {
                for (int j = 0; j < a.cols; j++)
                {
                    result.data[i, j] = a.data[i, j] + b.data[i, j];
                }
            }
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при сложении матриц: {ex.Message}");
            return new Matrix(1, 1);
        }
    }
    public static Matrix operator -(Matrix a, Matrix b)
    {
        try
        {
            if (a == null || b == null)
                throw new ArgumentNullException("Матрицы не могут быть null");
            if (a.rows != b.rows || a.cols != b.cols)
                throw new ArgumentException("Размеры матриц должны совпадать для вычитания");
            Matrix result = new Matrix(a.rows, a.cols);
            for (int i = 0; i < a.rows; i++)
            {
                for (int j = 0; j < a.cols; j++)
                {
                    result.data[i, j] = a.data[i, j] - b.data[i, j];
                }
            }
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при вычитании матриц: {ex.Message}");
            return new Matrix(1, 1);
        }
    }
    public static Matrix operator *(Matrix matrix, double scalar)
    {
        try
        {
            if (matrix == null)
                throw new ArgumentNullException(nameof(matrix));
            Matrix result = new Matrix(matrix.rows, matrix.cols);
            for (int i = 0; i < matrix.rows; i++)
            {
                for (int j = 0; j < matrix.cols; j++)
                {
                    result.data[i, j] = matrix.data[i, j] * scalar;
                }
            }
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при умножении матрицы на скаляр: {ex.Message}");
            return new Matrix(1, 1);
        }
    }
    public static Matrix operator *(double scalar, Matrix matrix)
    {
        try
        {
            return matrix * scalar;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при умножении скаляра на матрицу: {ex.Message}");
            return new Matrix(1, 1);
        }
    }
    public static Matrix operator /(Matrix matrix, double scalar)
    {
        try
        {
            if (matrix == null)
                throw new ArgumentNullException(nameof(matrix));
            if (Math.Abs(scalar) < 1e-10)
                throw new DivideByZeroException("Деление на ноль");

            return matrix * (1.0 / scalar);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при делении матрицы на скаляр: {ex.Message}");
            return new Matrix(1, 1);
        }
    }

    public static Matrix operator *(Matrix a, Matrix b)
    {
        try
        {
            if (a == null || b == null)
                throw new ArgumentNullException("Матрицы не могут быть null");
            if (a.cols != b.rows)
                throw new ArgumentException("Количество столбцов первой матрицы должно совпадать с количеством строк второй матрицы");

            Matrix result = new Matrix(a.rows, b.cols);
            for (int i = 0; i < a.rows; i++)
            {
                for (int j = 0; j < b.cols; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < a.cols; k++)
                    {
                        sum += a.data[i, k] * b.data[k, j];
                    }
                    result.data[i, j] = sum;
                }
            }
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при умножении матриц: {ex.Message}");
            return new Matrix(1, 1);
        }
    }

    public static bool operator ==(Matrix a, Matrix b)
    {
        try
        {
            if (a is null || b is null) return false;
            return a.IsIdentical(b);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при сравнении матриц: {ex.Message}");
            return false;
        }
    }

    public static bool operator !=(Matrix a, Matrix b)
    {
        try
        {
            return !(a == b);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при сравнении матриц: {ex.Message}");
            return true;
        }
    }

    public override bool Equals(object obj)
    {
        try
        {
            return obj is Matrix matrix && this == matrix;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при сравнении матриц: {ex.Message}");
            return false;
        }
    }

    public Matrix Transposed
    {
        get
        {
            try
            {
                Matrix result = new Matrix(cols, rows);
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        result.data[j, i] = data[i, j];
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при транспонировании матрицы: {ex.Message}");
                return new Matrix(1, 1);
            }
        }
    }
}

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

public class QMatrix : Matrix
{
    public event Action<string> ComputationProgress;

    public QMatrix(double[,] array) : base(array)
    {
        try
        {
            if (rows != cols)
                throw new NonSquareMatrixException();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при создании QMatrix: {ex.Message}");
            rows = 2;
            cols = 2;
            data = new double[2, 2] { { 1, 0 }, { 0, 1 } };
        }
    }

    public QMatrix(QMatrix other) : base(other) { }

    public QMatrix(Matrix other) : base(other)
    {
        try
        {
            if (rows != cols)
                throw new NonSquareMatrixException();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при создании QMatrix: {ex.Message}");
            rows = 2;
            cols = 2;
            data = new double[2, 2] { { 1, 0 }, { 0, 1 } };
        }
    }

    public QMatrix(int size) : base(size, size) { }

    public int Size => rows;

    public double Determinant
    {
        get
        {
            try
            {
                OnComputationProgress("Вычисление определителя начато");

                if (rows == 1)
                {
                    OnComputationProgress("Определитель матрицы 1x1 вычислен");
                    return data[0, 0];
                }

                if (rows == 2)
                {
                    double det = data[0, 0] * data[1, 1] - data[0, 1] * data[1, 0];
                    OnComputationProgress("Определитель матрицы 2x2 вычислен");
                    return det;
                }

                double determinant = 0;
                for (int j = 0; j < cols; j++)
                {
                    double minorDet = GetMinor(0, j).Determinant;
                    determinant += (j % 2 == 0 ? 1 : -1) * data[0, j] * minorDet;
                    OnComputationProgress($"Вычислен минор для элемента [0,{j}]");
                }

                OnComputationProgress("Определитель вычислен полностью");
                return determinant;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при вычислении определителя: {ex.Message}");
                OnComputationProgress($"Ошибка при вычислении определителя: {ex.Message}");
                return 0;
            }
        }
    }

    public QMatrix Inverse
    {
        get
        {
            try
            {
                OnComputationProgress("Начало вычисления обратной матрицы");

                double det = Determinant;
                OnComputationProgress($"Определитель: {det}");

                if (Math.Abs(det) < 1e-10)
                    throw new SingularMatrixException();

                QMatrix result = new QMatrix(rows);

                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        double minorDet = GetMinor(i, j).Determinant;
                        double cofactor = ((i + j) % 2 == 0 ? 1 : -1) * minorDet;
                        result.data[j, i] = cofactor / det;
                        OnComputationProgress($"Вычислен элемент [{j},{i}] обратной матрицы");
                    }
                }

                OnComputationProgress("Обратная матрица вычислена полностью");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при вычислении обратной матрицы: {ex.Message}");
                OnComputationProgress($"Ошибка при вычислении обратной матрицы: {ex.Message}");
                QMatrix identity = new QMatrix(rows);
                for (int i = 0; i < rows; i++)
                    identity.data[i, i] = 1;
                return identity;
            }
        }
    }

    private QMatrix GetMinor(int row, int col)
    {
        try
        {
            QMatrix minor = new QMatrix(rows - 1);
            int minorRow = 0;

            for (int i = 0; i < rows; i++)
            {
                if (i == row) continue;

                int minorCol = 0;
                for (int j = 0; j < cols; j++)
                {
                    if (j == col) continue;

                    minor.data[minorRow, minorCol] = data[i, j];
                    minorCol++;
                }
                minorRow++;
            }
            return minor;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при вычислении минора: {ex.Message}");
            return new QMatrix(1);
        }
    }

    protected virtual void OnComputationProgress(string message)
    {
        try
        {
            ComputationProgress?.Invoke(message);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка в обработчике события ComputationProgress: {ex.Message}");
        }
    }
}

