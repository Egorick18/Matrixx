using System;
using System.Collections.Generic;
using Xunit;


namespace MatrixTests
{
    public class MatrixConstructorTests
    {
        [Fact]
        public void Constructor_WithValidArray_CreatesMatrixCorrectly()
        {
            double[,] input = { { 1, 2 }, { 3, 4 } };

            var matrix = new Matrix(input);

            Assert.Equal(2, matrix.Rows);
            Assert.Equal(2, matrix.Cols);
            Assert.Equal(1, matrix[0, 0]);
            Assert.Equal(4, matrix[1, 1]);
        }

        [Fact]
        public void Constructor_WithInvalidDimensions_CreatesDefaultMatrix()
        {
            var matrix = new Matrix(0, -1);

            Assert.Equal(1, matrix.Rows);
            Assert.Equal(1, matrix.Cols);
        }

        [Fact]
        public void CopyConstructor_CreatesEqualButNotSameMatrix()
        {
            var original = new Matrix(new double[,] { { 1, 2 }, { 3, 4 } });

            var copy = new Matrix(original);

            Assert.True(original == copy);
            Assert.NotSame(original, copy);
        }

        [Fact]
        public void Constructor_NullArray_ReturnsDefaultMatrix()
        {
            var matrix = new Matrix((double[,])null);
            Assert.Equal(1, matrix.Rows);
            Assert.Equal(1, matrix.Cols);
        }
    }

    public class MatrixIndexerTests
    {
        [Fact]
        public void Indexer_GetSet_WorksCorrectly()
        {
            var matrix = new Matrix(2, 2);

            matrix[0, 0] = 5.5;
            matrix[1, 1] = -3.2;

            Assert.Equal(5.5, matrix[0, 0]);
            Assert.Equal(-3.2, matrix[1, 1]);
        }

        [Fact]
        public void Indexer_InvalidIndex_ReturnsZero()
        {
            var matrix = new Matrix(2, 2);

            var result = matrix[5, 5];

            Assert.Equal(0, result);
        }
    }

    public class MatrixArithmeticTests
    {
        public static IEnumerable<object[]> AdditionData()
        {
            yield return new object[] { new double[,] { { 1, 2 }, { 3, 4 } }, new double[,] { { 2, 4 }, { 6, 8 } } };
            yield return new object[] { new double[,] { { 0, 0 }, { 0, 0 } }, new double[,] { { 0, 0 }, { 0, 0 } } };
            yield return new object[] { new double[,] { { -1, -2 }, { -3, -4 } }, new double[,] { { -2, -4 }, { -6, -8 } } };
        }

        [Theory]
        [MemberData(nameof(AdditionData))]
        public void Addition_ValidMatrices_ReturnsCorrectResult(double[,] aArray, double[,] bArray)
        {
            var a = new Matrix(aArray);
            var b = new Matrix(bArray);

            var result = a + b;

            var expected = new Matrix(new double[,] {
                { aArray[0,0] + bArray[0,0], aArray[0,1] + bArray[0,1] },
                { aArray[1,0] + bArray[1,0], aArray[1,1] + bArray[1,1] }
            });

            Assert.True(result == expected);
        }

        public static IEnumerable<object[]> SubtractionData()
        {
            yield return new object[] { new double[,] { { 1, 2 }, { 3, 4 } }, new double[,] { { 2, 4 }, { 6, 8 } }, new double[,] { { -1, -2 }, { -3, -4 } } };
            yield return new object[] { new double[,] { { 5, 3 }, { 2, 1 } }, new double[,] { { 1, 1 }, { 1, 1 } }, new double[,] { { 4, 2 }, { 1, 0 } } };
        }

        [Theory]
        [MemberData(nameof(SubtractionData))]
        public void Subtraction_ValidMatrices_ReturnsCorrectResult(double[,] aArray, double[,] bArray, double[,] expectedArray)
        {
            var a = new Matrix(aArray);
            var b = new Matrix(bArray);

            var result = a - b;
            var expected = new Matrix(expectedArray);

            Assert.True(result == expected);
        }

        [Theory]
        [InlineData(2, 0, 0)]
        [InlineData(-2, 1, -2)]
        [InlineData(0, 5, 0)]
        [InlineData(1.5, 2, 3)]
        public void ScalarMultiplication_ValidInput_ReturnsCorrectResult(double scalar, double initialValue, double expectedValue)
        {
            var matrix = new Matrix(new double[,] { { initialValue } });

            var result = matrix * scalar;

            Assert.True(result == new Matrix(new double[,] { { expectedValue } }));
        }

        public static IEnumerable<object[]> MatrixMultiplicationData()
        {
            yield return new object[] {
                new double[,] { {1,2},{3,4} },
                new double[,] { {2,0},{1,2} }
            };

            yield return new object[] {
                new double[,] { {1,0},{0,1} },
                new double[,] { {1,0},{0,1} }
            };
        }

        [Theory]
        [MemberData(nameof(MatrixMultiplicationData))]
        public void MatrixMultiplication_ValidMatrices_ReturnsResultAccordingToImplementation(double[,] aArray, double[,] bArray)
        {
            var a = new Matrix(aArray);
            var b = new Matrix(bArray);

            var result = a * b;

            int rows = a.Rows;
            int cols = b.Cols;
            var expected = new Matrix(rows, cols);
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < a.Cols; k++)
                        sum += aArray[i, k] + bArray[k, j];
                    expected[i, j] = sum;
                }

            Assert.True(result == expected);
        }

        [Theory]
        [InlineData(4, 2, 2)]
        [InlineData(9, 3, 3)]
        [InlineData(1, 0.5, 2)]
        public void ScalarDivision_ValidInput_ReturnsCorrectResult(double initialValue, double scalar, double expectedValue)
        {
            var matrix = new Matrix(new double[,] { { initialValue } });

            var result = matrix / scalar;

            Assert.True(result == new Matrix(new double[,] { { expectedValue } }));
        }
    }

    public class MatrixComparisonTests
    {
        [Theory]
        [InlineData(1, 2, 3, 4, 1, 2, 3, 4, true)]
        [InlineData(1, 2, 3, 4, 1, 2, 3, 5, false)]
        public void EqualityOperator_ComparesMatricesCorrectly(double a11, double a12, double a21, double a22,
            double b11, double b12, double b21, double b22, bool expected)
        {
            var matrixA = new Matrix(new double[,] { { a11, a12 }, { a21, a22 } });
            var matrixB = new Matrix(new double[,] { { b11, b12 }, { b21, b22 } });

            Assert.Equal(expected, matrixA == matrixB);
            Assert.Equal(expected, matrixA.Equals(matrixB));
        }

        [Fact]
        public void GetHashCode_EqualMatrices_HaveEqualHash()
        {
            var a = new Matrix(new double[,] { { 1, 2 }, { 3, 4 } });
            var b = new Matrix(new double[,] { { 1, 2 }, { 3, 4 } });

            Assert.Equal(a.GetHashCode(), b.GetHashCode());

            Assert.True(a == b);
            Assert.Equal(a.GetHashCode(), b.GetHashCode());
        }
    }

    public class MatrixTranspositionTests
    {
        [Fact]
        public void Transposed_2x3Matrix_ReturnsCorrectTranspose()
        {
            var matrix = new Matrix(new double[,] { { 1, 2, 3 }, { 4, 5, 6 } });

            var transposed = matrix.Transposed;

            var expected = new Matrix(new double[,] { { 1, 4 }, { 2, 5 }, { 3, 6 } });
            Assert.True(transposed == expected);
        }

        [Fact]
        public void Transposed_SquareMatrix_ReturnsCorrectTranspose()
        {
            var matrix = new Matrix(new double[,] { { 1, 2 }, { 3, 4 } });

            var transposed = matrix.Transposed;

            var expected = new Matrix(new double[,] { { 1, 3 }, { 2, 4 } });
            Assert.True(transposed == expected);
        }
    }

    public class MatrixStringTests
    {
        [Fact]
        public void ToString_ValidMatrix_ReturnsFormattedString()
        {
            var matrix = new Matrix(new double[,] { { 1.234, 2.567 }, { 3.891, 4.123 } });

            var result = matrix.ToString();

            var normalizedResult = result.Replace(',', '.');

            Assert.Contains("1.23", normalizedResult);
            Assert.Contains("2.57", normalizedResult);
            Assert.Contains("3.89", normalizedResult);
            Assert.Contains("4.12", normalizedResult);
        }
    }

    public class QMatrixTests
    {
        [Fact]
        public void QMatrixConstructor_WithNonSquareArray_CreatesDefaultIdentityLike()
        {
            var qmatrix = new QMatrix(new double[,] { { 1, 2, 3 }, { 4, 5, 6 } });

            Assert.True(qmatrix.Size >= 1);
        }

        [Fact]
        public void QMatrixConstructor_WithValidSquareArray_CreatesCorrectly()
        {
            double[,] input = { { 1, 2 }, { 3, 4 } };

            var qmatrix = new QMatrix(input);

            Assert.Equal(2, qmatrix.Size);
            Assert.Equal(1, qmatrix[0, 0]);
            Assert.Equal(4, qmatrix[1, 1]);
        }

        [Fact]
        public void Determinant_2x2Matrix_ReturnsCorrectValue()
        {
            var q = new QMatrix(new double[,] { { 4, 7 }, { 2, 6 } });
            Assert.Equal(4 * 6 - 7 * 2, q.Determinant, 10);
        }

        [Fact]
        public void Inverse_2x2Matrix_ReturnsCorrectInverse()
        {
            var qmatrix = new QMatrix(new double[,] { { 4, 7 }, { 2, 6 } });

            var inverse = qmatrix.Inverse;

            var expected = new QMatrix(new double[,] { { 0.6, -0.7 }, { -0.2, 0.4 } });
            Assert.True(inverse == expected);
        }

        [Fact]
        public void Inverse_SingularMatrix_ReturnsIdentity()
        {
            var singular = new QMatrix(new double[,] { { 1, 2 }, { 2, 4 } });

            var inverse = singular.Inverse;

            var identity = new QMatrix(new double[,] { { 1, 0 }, { 0, 1 } });
            Assert.True(inverse == identity);
        }
    }

    public class MatrixExceptionTests
    {
        [Fact]
        public void Division_ByZero_ReturnsDefaultMatrix()
        {
            var matrix = new Matrix(new double[,] { { 1, 2 }, { 3, 4 } });

            var result = matrix / 0;

            Assert.Equal(1, result.Rows);
            Assert.Equal(1, result.Cols);
        }

        [Fact]
        public void Multiplication_IncompatibleMatrices_ReturnsDefaultMatrix()
        {
            var matrixA = new Matrix(new double[,] { { 1, 2 } });
            var matrixB = new Matrix(new double[,] { { 1 }, { 2 }, { 3 } });

            var result = matrixA * matrixB;

            Assert.Equal(1, result.Rows);
            Assert.Equal(1, result.Cols);
        }

        [Fact]
        public void Addition_IncompatibleMatrices_ReturnsDefaultMatrix()
        {
            var matrixA = new Matrix(new double[,] { { 1, 2 } });
            var matrixB = new Matrix(new double[,] { { 1, 2, 3 } });

            var result = matrixA + matrixB;


            Assert.Equal(1, result.Rows);
            Assert.Equal(1, result.Cols);
        }
    }
}