using static Exceptions;

public class QMatrix : Matrix
{

    public QMatrix(double[,] array) : base(array)
    {
        try
        {
            if (Rows != Cols)
                throw new NonSquareMatrixException();
        }
        catch
        {
        }
    }

    public QMatrix(QMatrix other) : base(other) { }
    public QMatrix(Matrix other) : base(other) { }
    public QMatrix(int size) : base(size, size) { }

    public int Size => Rows;

    public double Determinant
    {
        get
        {
            try
            {
                if (Rows == 1)
                    return this[0, 0];

                if (Rows == 2)
                    return this[0, 0] * this[1, 1] - this[0, 1] * this[1, 0];

                double det = 0;

                for (int j = 0; j < Cols; j++)
                {
                    double minorDet = GetMinor(0, j).Determinant;
                    det += (j % 2 == 0 ? 1 : -1) * this[0, j] * minorDet;
                }

                return det;
            }
            catch
            {
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
                double det = Determinant;

                if (Math.Abs(det) < 1e-10)
                    throw new SingularMatrixException();

                QMatrix result = new QMatrix(Size);

                for (int i = 0; i < Size; i++)
                    for (int j = 0; j < Size; j++)
                    {
                        double minorDet = GetMinor(i, j).Determinant;
                        double cofactor = ((i + j) % 2 == 0 ? 1 : -1) * minorDet;
                        result[j, i] = cofactor / det;
                    }

                return result;
            }
            catch
            {
                QMatrix identity = new QMatrix(Size);
                for (int i = 0; i < Size; i++)
                    identity[i, i] = 1;
                return identity;
            }
        }
    }

    private QMatrix GetMinor(int row, int col)
    {
        try
        {
            QMatrix minor = new QMatrix(Size - 1);
            int r = 0;

            for (int i = 0; i < Size; i++)
            {
                if (i == row) continue;
                int c = 0;

                for (int j = 0; j < Size; j++)
                {
                    if (j == col) continue;
                    minor[r, c] = this[i, j];
                    c++;
                }
                r++;
            }

            return minor;
        }
        catch
        {
            return new QMatrix(1);
        }
    }
}
