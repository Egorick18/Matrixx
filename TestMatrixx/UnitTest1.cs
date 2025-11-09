using System;
using Xunit;
using Assert = Xunit.Assert;
using TheoryAttribute = Xunit.TheoryAttribute;

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
        public void CopyConstructor_CreatesIdenticalMatrix()
        {
            var original = new Matrix(new double[,] { { 1, 2 }, { 3, 4 } });

            var copy = new Matrix(original);

            Assert.True(original.IsIdentical(copy));
            Assert.NotSame(original, copy);
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
        [Theory]
        [InlineData(1, 2, 3, 4, 2, 4, 6, 8)]
        [InlineData(0, 0, 0, 0, 0, 0, 0, 0)]
        [InlineData(-1, -2, -3, -4, -2, -4, -6, -8)]
        public void Addition_ValidMatrices_ReturnsCorrectResult(
            double a11, double a12, double a21, double a22,
            double b11, double b12, double b21, double b22)
        {
            var matrixA = new Matrix(new double[,] { { a11, a12 }, { a21, a22 } });
            var matrixB = new Matrix(new double[,] { { b11, b12 }, { b21, b22 } });

            var result = matrixA + matrixB;

            Assert.Equal(a11 + b11, result[0, 0]);
            Assert.Equal(a12 + b12, result[0, 1]);
            Assert.Equal(a21 + b21, result[1, 0]);
            Assert.Equal(a22 + b22, result[1, 1]);
        }

        [Theory]
        [InlineData(1, 2, 3, 4, 2, 4, 6, 8, -1, -2, -3, -4)]
        [InlineData(5, 3, 2, 1, 1, 1, 1, 1, 4, 2, 1, 0)]
        public void Subtraction_ValidMatrices_ReturnsCorrectResult(
            double a11, double a12, double a21, double a22,
            double b11, double b12, double b21, double b22,
            double r11, double r12, double r21, double r22)
        {
            var matrixA = new Matrix(new double[,] { { a11, a12 }, { a21, a22 } });
            var matrixB = new Matrix(new double[,] { { b11, b12 }, { b21, b22 } });

            var result = matrixA - matrixB;

            Assert.Equal(r11, result[0, 0]);
            Assert.Equal(r12, result[0, 1]);
            Assert.Equal(r21, result[1, 0]);
            Assert.Equal(r22, result[1, 1]);
        }

        [Theory]
        [InlineData(2, 0, 0)]
        [InlineData(-2, 1, -2)]
        [InlineData(0, 5, 0)]
        [InlineData(1.5, 2, 3)]
        public void ScalarMultiplication_ValidInput_ReturnsCorrectResult(
    double scalar, double initialValue, double expectedValue)
        {
            var matrix = new Matrix(new double[,] { { initialValue } });

            var result = matrix * scalar;

            Assert.Equal(expectedValue, result[0, 0]);
        }

        [Theory]
        [InlineData(1, 2, 3, 4, 2, 0, 1, 2, 4, 4, 10, 8)]
        [InlineData(1, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 1)]
        public void MatrixMultiplication_ValidMatrices_ReturnsCorrectResult(
    double a11, double a12, double a21, double a22,
    double b11, double b12, double b21, double b22,
    double r11, double r12, double r21, double r22)
        {
            var matrixA = new Matrix(new double[,] { { a11, a12 }, { a21, a22 } });
            var matrixB = new Matrix(new double[,] { { b11, b12 }, { b21, b22 } });

            var result = matrixA * matrixB;

            Assert.Equal(r11, result[0, 0], 10);
            Assert.Equal(r12, result[0, 1], 10);
            Assert.Equal(r21, result[1, 0], 10);
            Assert.Equal(r22, result[1, 1], 10);
        }

        [Theory]
        [InlineData(4, 2, 2)]
        [InlineData(9, 3, 3)]
        [InlineData(1, 0.5, 2)]
        public void ScalarDivision_ValidInput_ReturnsCorrectResult(
            double initialValue, double scalar, double expectedValue)
        {
            var matrix = new Matrix(new double[,] { { initialValue } });

            var result = matrix / scalar;

            Assert.Equal(expectedValue, result[0, 0], 10);
        }
    }

    public class MatrixComparisonTests
    {
        [Theory]
        [InlineData(1, 2, 3, 4, 1, 2, 3, 4, true)]
        [InlineData(1, 2, 3, 4, 1, 2, 3, 5, false)]
        public void IsIdentical_ComparesMatricesCorrectly(
    double a11, double a12, double a21, double a22,
    double b11, double b12, double b21, double b22,
    bool expected)
        {
            var matrixA = new Matrix(new double[,] { { a11, a12 }, { a21, a22 } });
            var matrixB = new Matrix(new double[,] { { b11, b12 }, { b21, b22 } });

            var result = matrixA.IsIdentical(matrixB);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(1, 2, 3, 4, 1, 2, 3, 4, true)]
        [InlineData(1, 2, 3, 4, 1, 2, 3, 5, false)]
        public void EqualityOperator_ComparesMatricesCorrectly(
            double a11, double a12, double a21, double a22,
            double b11, double b12, double b21, double b22,
            bool expected)
        {
            var matrixA = new Matrix(new double[,] { { a11, a12 }, { a21, a22 } });
            var matrixB = new Matrix(new double[,] { { b11, b12 }, { b21, b22 } });

            Assert.Equal(expected, matrixA == matrixB);
        }
    }
    public class MatrixTranspositionTests
    {
        [Fact]
        public void Transposed_2x3Matrix_ReturnsCorrectTranspose()
        {
            var matrix = new Matrix(new double[,] { { 1, 2, 3 }, { 4, 5, 6 } });

            var transposed = matrix.Transposed;

            Assert.Equal(3, transposed.Rows);
            Assert.Equal(2, transposed.Cols);
            Assert.Equal(1, transposed[0, 0]);
            Assert.Equal(4, transposed[0, 1]);
            Assert.Equal(2, transposed[1, 0]);
            Assert.Equal(5, transposed[1, 1]);
            Assert.Equal(3, transposed[2, 0]);
            Assert.Equal(6, transposed[2, 1]);
        }

        [Fact]
        public void Transposed_SquareMatrix_ReturnsCorrectTranspose()
        {
            var matrix = new Matrix(new double[,] { { 1, 2 }, { 3, 4 } });

            var transposed = matrix.Transposed;

            Assert.Equal(1, transposed[0, 0]);
            Assert.Equal(3, transposed[0, 1]);
            Assert.Equal(2, transposed[1, 0]);
            Assert.Equal(4, transposed[1, 1]);
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

        public class QMatrixConstructorTests
        {
            [Fact]
            public void QMatrixConstructor_WithNonSquareArray_CreatesDefaultMatrix()
            {
                var qmatrix = new QMatrix(new double[,] { { 1, 2, 3 }, { 4, 5, 6 } });

                Assert.Equal(2, qmatrix.Size);
                Assert.True(qmatrix.IsIdentical(new QMatrix(new double[,] { { 1, 0 }, { 0, 1 } })));
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
            public void QMatrixCopyConstructor_CreatesIdenticalMatrix()
            {
                var original = new QMatrix(new double[,] { { 1, 2 }, { 3, 4 } });

                var copy = new QMatrix(original);

                Assert.True(original.IsIdentical(copy));
            }
        }

        public class QMatrixDeterminantTests
        {
            [Theory]
            [InlineData(1, 0, 0, 1, 1)]
            [InlineData(2, 1, 1, 2, 3)]
            [InlineData(3, 0, 0, 3, 9)]
            [InlineData(4, 7, 2, 6, 10)]
            public void Determinant_2x2Matrix_ReturnsCorrectValue(
                double a11, double a12, double a21, double a22, double expectedDet)
            {
                var qmatrix = new QMatrix(new double[,] { { a11, a12 }, { a21, a22 } });

                var determinant = qmatrix.Determinant;

                Assert.Equal(expectedDet, determinant, 10);
            }

            [Fact]
            public void Determinant_3x3Matrix_ReturnsCorrectValue()
            {
                var qmatrix = new QMatrix(new double[,] {
                { 1, 2, 3 },
                { 4, 5, 6 },
                { 7, 8, 9 }
            });

                var determinant = qmatrix.Determinant;

                Assert.Equal(0, determinant, 10);
            }

            [Fact]
            public void Determinant_1x1Matrix_ReturnsCorrectValue()
            {
                var qmatrix = new QMatrix(new double[,] { { 5 } });

                var determinant = qmatrix.Determinant;

                Assert.Equal(5, determinant);
            }
        }

        public class QMatrixInverseTests
        {
            [Fact]
            public void Inverse_2x2Matrix_ReturnsCorrectInverse()
            {
                var qmatrix = new QMatrix(new double[,] { { 4, 7 }, { 2, 6 } });

                var inverse = qmatrix.Inverse;

                var expected = new QMatrix(new double[,] { { 0.6, -0.7 }, { -0.2, 0.4 } });
                Assert.True(inverse.IsIdentical(expected));
            }

            [Fact]
            public void Inverse_IdentityMatrix_ReturnsIdentity()
            {
                var identity = new QMatrix(new double[,] { { 1, 0 }, { 0, 1 } });

                var inverse = identity.Inverse;

                Assert.True(identity.IsIdentical(inverse));
            }

            [Fact]
            public void Inverse_SingularMatrix_ReturnsIdentity()
            {
                var singular = new QMatrix(new double[,] { { 1, 2 }, { 2, 4 } });

                var inverse = singular.Inverse;

                var identity = new QMatrix(new double[,] { { 1, 0 }, { 0, 1 } });
                Assert.True(inverse.IsIdentical(identity));
            }
        }

        public class QMatrixEventTests
        {
            [Fact]
            public void ComputationProgress_Event_IsRaisedDuringDeterminantCalculation()
            {
                var qmatrix = new QMatrix(new double[,] { { 1, 2 }, { 3, 4 } });
                var eventMessages = new System.Collections.Generic.List<string>();
                qmatrix.ComputationProgress += message => eventMessages.Add(message);

                var det = qmatrix.Determinant;

                Assert.NotEmpty(eventMessages);
                Assert.Contains(eventMessages, msg => msg.Contains("¬ычисление определител€"));
            }

            [Fact]
            public void ComputationProgress_Event_IsRaisedDuringInverseCalculation()
            {
                var qmatrix = new QMatrix(new double[,] { { 4, 7 }, { 2, 6 } });
                var eventMessages = new System.Collections.Generic.List<string>();
                qmatrix.ComputationProgress += message => eventMessages.Add(message);

                var inverse = qmatrix.Inverse;

                Assert.NotEmpty(eventMessages);
                Assert.Contains(eventMessages, msg => msg.Contains("обратной матрицы"));
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
}