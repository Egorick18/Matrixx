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
            yield return new object[] {
                new double[,] { { 1, 2 }, { 3, 4 } },
                new double[,] { { 5, 6 }, { 7, 8 } },
                new double[,] { { 6, 8 }, { 10, 12 } }
            };
            yield return new object[] {
                new double[,] { { 0, 0 }, { 0, 0 } },
                new double[,] { { 0, 0 }, { 0, 0 } },
                new double[,] { { 0, 0 }, { 0, 0 } }
            };
            yield return new object[] {
                new double[,] { { -1, -2 }, { -3, -4 } },
                new double[,] { { -5, -6 }, { -7, -8 } },
                new double[,] { { -6, -8 }, { -10, -12 } }
            };
        }

        [Theory]
        [MemberData(nameof(AdditionData))]
        public void Addition_ValidMatrices_ReturnsCorrectResult(double[,] aArray, double[,] bArray, double[,] expectedArray)
        {
            var a = new Matrix(aArray);
            var b = new Matrix(bArray);
            var expected = new Matrix(expectedArray);

            var result = a + b;

            Assert.True(result == expected);
        }

        public static IEnumerable<object[]> SubtractionData()
        {
            yield return new object[] {
                new double[,] { { 1, 2 }, { 3, 4 } },
                new double[,] { { 2, 4 }, { 6, 8 } },
                new double[,] { { -1, -2 }, { -3, -4 } }
            };
            yield return new object[] {
                new double[,] { { 5, 3 }, { 2, 1 } },
                new double[,] { { 1, 1 }, { 1, 1 } },
                new double[,] { { 4, 2 }, { 1, 0 } }
            };
        }

        [Theory]
        [MemberData(nameof(SubtractionData))]
        public void Subtraction_ValidMatrices_ReturnsCorrectResult(double[,] aArray, double[,] bArray, double[,] expectedArray)
        {
            var a = new Matrix(aArray);
            var b = new Matrix(bArray);
            var expected = new Matrix(expectedArray);

            var result = a - b;

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
            var expected = new Matrix(new double[,] { { expectedValue } });

            var result = matrix * scalar;

            Assert.True(result == expected);
        }

        public static IEnumerable<object[]> MatrixMultiplicationData()
        {
            yield return new object[] {
                new double[,] { {1,2},{3,4} },
                new double[,] { {2,0},{1,2} },
                new double[,] { {4,4},{10,8} }
            };

            yield return new object[] {
                new double[,] { {1,0},{0,1} },
                new double[,] { {1,0},{0,1} },
                new double[,] { {1,0},{0,1} }
            };

            yield return new object[] {
                new double[,] { {2,0},{0,2} },
                new double[,] { {3,1},{1,3} },
                new double[,] { {6,2},{2,6} }
            };
        }

        [Theory]
        [MemberData(nameof(MatrixMultiplicationData))]
        public void MatrixMultiplication_ValidMatrices_ReturnsResultAccordingToImplementation(double[,] aArray, double[,] bArray, double[,] expectedArray)
        {
            var a = new Matrix(aArray);
            var b = new Matrix(bArray);
            var expected = new Matrix(expectedArray);

            var result = a * b;

            Assert.True(result == expected);
        }

        [Theory]
        [InlineData(4, 2, 2)]
        [InlineData(9, 3, 3)]
        [InlineData(1, 0.5, 2)]
        public void ScalarDivision_ValidInput_ReturnsCorrectResult(double initialValue, double scalar, double expectedValue)
        {
            var matrix = new Matrix(new double[,] { { initialValue } });
            var expected = new Matrix(new double[,] { { expectedValue } });

            var result = matrix / scalar;

            Assert.True(result == expected);
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
        }

        [Fact]
        public void GetHashCode_DifferentMatrices_ShouldHaveDifferentHashCodes()
        {
            var matrix1 = new Matrix(new double[,] { { 1, 2 }, { 3, 4 } });
            var matrix2 = new Matrix(new double[,] { { 1, 2, 3 }, { 4, 5, 6 } });
            var matrix3 = new Matrix(new double[,] { { 1 }, { 2 }, { 3 } });

            Assert.NotEqual(matrix1.GetHashCode(), matrix2.GetHashCode());
            Assert.NotEqual(matrix1.GetHashCode(), matrix3.GetHashCode());
            Assert.NotEqual(matrix2.GetHashCode(), matrix3.GetHashCode());
        }

        [Fact]
        public void GetHashCode_SameDimensionsDifferentValues_HaveSameHashDueToImplementation()
        {
            var matrix1 = new Matrix(new double[,] { { 1, 2 }, { 3, 4 } });
            var matrix2 = new Matrix(new double[,] { { 5, 6 }, { 7, 8 } });

            Assert.Equal(matrix1.GetHashCode(), matrix2.GetHashCode());
        }

        [Fact]
        public void GetHashCode_DifferentDimensions_HaveDifferentHashCodes()
        {
            var matrix1 = new Matrix(new double[,] { { 1, 2 }, { 3, 4 } });
            var matrix2 = new Matrix(new double[,] { { 1, 2, 3 }, { 4, 5, 6 } });
            var matrix3 = new Matrix(new double[,] { { 1 }, { 2 } });

            Assert.NotEqual(matrix1.GetHashCode(), matrix2.GetHashCode());
            Assert.NotEqual(matrix1.GetHashCode(), matrix3.GetHashCode());
            Assert.NotEqual(matrix2.GetHashCode(), matrix3.GetHashCode());
        }
    }

    public class MatrixTranspositionTests
    {
        [Fact]
        public void Transposed_2x3Matrix_ReturnsCorrectTranspose()
        {
            var matrix = new Matrix(new double[,] { { 1, 2, 3 }, { 4, 5, 6 } });
            var expected = new Matrix(new double[,] { { 1, 4 }, { 2, 5 }, { 3, 6 } });

            var transposed = matrix.Transposed;

            Assert.True(transposed == expected);
        }

        [Fact]
        public void Transposed_SquareMatrix_ReturnsCorrectTranspose()
        {
            var matrix = new Matrix(new double[,] { { 1, 2 }, { 3, 4 } });
            var expected = new Matrix(new double[,] { { 1, 3 }, { 2, 4 } });

            var transposed = matrix.Transposed;

            Assert.True(transposed == expected);
        }

        [Fact]
        public void Transposed_TransposeTwice_ReturnsOriginal()
        {
            var original = new Matrix(new double[,] { { 1, 2, 3 }, { 4, 5, 6 } });

            var transposedOnce = original.Transposed;
            var transposedTwice = transposedOnce.Transposed;

            Assert.True(transposedTwice == original);
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
            var expected = new Matrix(input);

            var qmatrix = new QMatrix(input);

            Assert.Equal(2, qmatrix.Size);
            Assert.True(qmatrix == expected);
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
            var expected = new QMatrix(new double[,] { { 0.6, -0.7 }, { -0.2, 0.4 } });

            var inverse = qmatrix.Inverse;

            Assert.True(inverse == expected);
        }

        [Fact]
        public void Inverse_SingularMatrix_ReturnsIdentity()
        {
            var singular = new QMatrix(new double[,] { { 1, 2 }, { 2, 4 } });
            var identity = new QMatrix(new double[,] { { 1, 0 }, { 0, 1 } });

            var inverse = singular.Inverse;

            Assert.True(inverse == identity);
        }

        [Fact]
        public void Inverse_MultipliedByOriginal_ReturnsIdentity()
        {
            var original = new QMatrix(new double[,] { { 3, 0, 2 }, { 2, 0, -2 }, { 0, 1, 1 } });

            var inverse = original.Inverse;
            var product = original * inverse;

            var identity = new QMatrix(new double[,] { { 1, 0, 0 }, { 0, 1, 0 }, { 0, 0, 1 } });

            Assert.True(IsMatrixEqual(product, identity, 1e-10));
        }

        private bool IsMatrixEqual(Matrix a, Matrix b, double tolerance)
        {
            if (a.Rows != b.Rows || a.Cols != b.Cols)
                return false;

            for (int i = 0; i < a.Rows; i++)
                for (int j = 0; j < a.Cols; j++)
                    if (Math.Abs(a[i, j] - b[i, j]) > tolerance)
                        return false;

            return true;
        }
    }

    public class MatrixExceptionTests
    {
        [Fact]
        public void Division_ByZero_ReturnsDefaultMatrix()
        {
            var matrix = new Matrix(new double[,] { { 1, 2 }, { 3, 4 } });
            var defaultMatrix = new Matrix(1, 1);

            var result = matrix / 0;

            Assert.True(result == defaultMatrix);
        }

        [Fact]
        public void Multiplication_IncompatibleMatrices_ReturnsDefaultMatrix()
        {
            var matrixA = new Matrix(new double[,] { { 1, 2 } });
            var matrixB = new Matrix(new double[,] { { 1 }, { 2 }, { 3 } });
            var defaultMatrix = new Matrix(1, 1);

            var result = matrixA * matrixB;

            Assert.True(result == defaultMatrix);
        }

        [Fact]
        public void Addition_IncompatibleMatrices_ReturnsDefaultMatrix()
        {
            var matrixA = new Matrix(new double[,] { { 1, 2 } });
            var matrixB = new Matrix(new double[,] { { 1, 2, 3 } });
            var defaultMatrix = new Matrix(1, 1);

            var result = matrixA + matrixB;

            Assert.True(result == defaultMatrix);
        }

        [Fact]
        public void Constructor_NullMatrix_CreatesDefaultMatrix()
        {
            var defaultMatrix = new Matrix(1, 1);

            var result = new Matrix((Matrix)null);

            Assert.True(result == defaultMatrix);
        }

        [Fact]
        public void Indexer_SetInvalidIndex_DoesNotThrow()
        {
            var matrix = new Matrix(2, 2);

            matrix[-1, 0] = 5;
            matrix[0, -1] = 5;
            matrix[10, 0] = 5;
            matrix[0, 10] = 5;

            var expected = new Matrix(2, 2);
            Assert.True(matrix == expected);
        }

        [Fact]
        public void Operations_WithNullMatrices_ReturnDefaultMatrix()
        {
            var matrix = new Matrix(new double[,] { { 1, 2 } });
            var defaultMatrix = new Matrix(1, 1);

            var result1 = matrix + null;
            var result2 = matrix - null;
            var result3 = matrix * (Matrix)null;
            var result4 = (Matrix)null * matrix;

            Assert.True(result1 == defaultMatrix);
            Assert.True(result2 == defaultMatrix);
            Assert.True(result3 == defaultMatrix);
            Assert.True(result4 == defaultMatrix);
        }

        [Fact]
        public void ScalarOperations_WithNullMatrix_ReturnDefaultMatrix()
        {
            var defaultMatrix = new Matrix(1, 1);

            var result1 = (Matrix)null * 2.0;
            var result2 = 2.0 * (Matrix)null;
            var result3 = (Matrix)null / 2.0;

            Assert.True(result1 == defaultMatrix);
            Assert.True(result2 == defaultMatrix);
            Assert.True(result3 == defaultMatrix);
        }
    }

    public class MatrixEdgeCaseTests
    {
        [Fact]
        public void EmptyLikeMatrix_CreatedWithDefaultConstructor()
        {
            var matrix = new Matrix();

            Assert.Equal(1, matrix.Rows);
            Assert.Equal(1, matrix.Cols);
            Assert.Equal(0, matrix[0, 0]);
        }

        [Fact]
        public void LargeMatrix_OperationsWorkCorrectly()
        {
            var largeMatrix = new Matrix(100, 100);

            for (int i = 0; i < 100; i++)
            {
                largeMatrix[i, i] = 1;
            }

            var transposed = largeMatrix.Transposed;

            Assert.True(largeMatrix == transposed);
        }

        [Fact]
        public void MatrixWithExtremeValues_HandlesCorrectly()
        {
            var matrix = new Matrix(new double[,] {
                { double.MaxValue, double.MinValue },
                { double.Epsilon, -double.Epsilon }
            });

            var result = matrix + matrix;

            var expected = new Matrix(new double[,] {
                { double.MaxValue * 2, double.MinValue * 2 },
                { double.Epsilon * 2, -double.Epsilon * 2 }
            });

            Assert.True(result == expected || result == new Matrix(1, 1));
        }
    }
}
