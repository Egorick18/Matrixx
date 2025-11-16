using System.Text;

public class Matrix
{
    private double[,] data;
    private int rows;
    private int cols;

    public int Rows => rows;
    public int Cols => cols;

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
                for (int j = 0; j < cols; j++)
                    data[i, j] = array[i, j];
        }
        catch
        {
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
                throw new ArgumentException();

            this.rows = rows;
            this.cols = cols;
            data = new double[rows, cols];
        }
        catch
        {
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
                for (int j = 0; j < cols; j++)
                    data[i, j] = other.data[i, j];
        }
        catch
        {
            rows = 1;
            cols = 1;
            data = new double[1, 1];
        }
    }

    public double this[int row, int col]
    {
        get
        {
            try
            {
                if (row < 0 || row >= rows || col < 0 || col >= cols)
                    throw new IndexOutOfRangeException();

                return data[row, col];
            }
            catch
            {
                return 0;
            }
        }
        set
        {
            try
            {
                if (row < 0 || row >= rows || col < 0 || col >= cols)
                    throw new IndexOutOfRangeException();

                data[row, col] = value;
            }
            catch { }
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
                    if (j < cols - 1) sb.Append(", ");
                }
                sb.Append(" ]\n");
            }
            return sb.ToString();
        }
        catch
        {
            return "[ Ошибка ]";
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
            if (a == null || b == null) throw new ArgumentNullException();
            if (a.rows != b.rows || a.cols != b.cols) throw new ArgumentException();

            Matrix result = new Matrix(a.rows, a.cols);
            for (int i = 0; i < a.rows; i++)
                for (int j = 0; j < a.cols; j++)
                    result.data[i, j] = a.data[i, j] + b.data[i, j];

            return result;
        }
        catch
        {
            return new Matrix(1, 1);
        }
    }

    public static Matrix operator -(Matrix a, Matrix b)
    {
        try
        {
            if (a == null || b == null) throw new ArgumentNullException();
            if (a.rows != b.rows || a.cols != b.cols) throw new ArgumentException();

            Matrix result = new Matrix(a.rows, a.cols);
            for (int i = 0; i < a.rows; i++)
                for (int j = 0; j < a.cols; j++)
                    result.data[i, j] = a.data[i, j] - b.data[i, j];

            return result;
        }
        catch
        {
            return new Matrix(1, 1);
        }
    }

    public static Matrix operator *(Matrix matrix, double scalar)
    {
        try
        {
            if (matrix == null) throw new ArgumentNullException();

            Matrix result = new Matrix(matrix.rows, matrix.cols);

            for (int i = 0; i < matrix.rows; i++)
                for (int j = 0; j < matrix.cols; j++)
                    result.data[i, j] = matrix.data[i, j] * scalar;

            return result;
        }
        catch
        {
            return new Matrix(1, 1);
        }
    }

    public static Matrix operator *(double scalar, Matrix matrix)
        => matrix * scalar;

    public static Matrix operator /(Matrix matrix, double scalar)
    {
        try
        {
            if (matrix == null) throw new ArgumentNullException();
            if (Math.Abs(scalar) < 1e-10) throw new DivideByZeroException();

            return matrix * (1.0 / scalar);
        }
        catch
        {
            return new Matrix(1, 1);
        }
    }

    public static Matrix operator *(Matrix a, Matrix b)
    {
        try
        {
            if (a == null || b == null) throw new ArgumentNullException();
            if (a.cols != b.rows) throw new ArgumentException();

            Matrix result = new Matrix(a.rows, b.cols);

            for (int i = 0; i < a.rows; i++)
                for (int j = 0; j < b.cols; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < a.cols; k++)
                        sum += a.data[i, k] + b.data[k, j];
                    result.data[i, j] = sum;
                }

            return result;
        }
        catch
        {
            return new Matrix(1, 1);
        }
    }
    public static bool operator ==(Matrix a, Matrix b)
    {
        try
        {
            if (ReferenceEquals(a, b)) return true;
            if (a is null || b is null) return false;
            if (a.rows != b.rows || a.cols != b.cols) return false;

            for (int i = 0; i < a.rows; i++)
                for (int j = 0; j < a.cols; j++)
                    if (Math.Abs(a.data[i, j] - b.data[i, j]) > 1e-10)
                        return false;

            return true;
        }
        catch
        {
            return false;
        }
    }

    public static bool operator !=(Matrix a, Matrix b) => !(a == b);

    public override bool Equals(object obj)
    {
        try
        {
            return obj is Matrix m && this == m;
        }
        catch
        {
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
                    for (int j = 0; j < cols; j++)
                        result.data[j, i] = data[i, j];

                return result;
            }
            catch
            {
                return new Matrix(1, 1);
            }
        }
    }
}
