using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Matrix;

double[,] m1 = { { 11, 7 }, { 2, 6 }, { 1, 6 } };
Matrix mx1 = new Matrix(m1);
Console.WriteLine(mx1);

Console.WriteLine($"mx1: {mx1.Rows}x{mx1.Cols}\n");

Console.WriteLine($"Хэш mx1: {mx1.GetHashCode()} \n");

double[,] m2 = { { 9, 7 }, { 15, 6 } };
Matrix mx2 = new Matrix(m2);
Console.WriteLine(mx2);

double[,] m3 = { { 11, 7 }, { 2, 6 } };
Matrix mx3 = new Matrix(m3);
Console.WriteLine(mx3);

Matrix mx4 = new Matrix(mx1);
Console.WriteLine(mx4);

Console.WriteLine($"m1 идентична m3: {mx1.IsIdentical(mx4)} \n");
Console.WriteLine($"m1 идентична m2: {mx1.IsIdentical(mx2)}\n");


Matrix result1 = mx1 + mx2;
Console.WriteLine(result1);

Matrix result2 = mx2 + mx3;
Console.WriteLine(result2);

Matrix result3 = mx2 - mx3;
Console.WriteLine(result3);

Matrix result4 = mx1 * mx3;
Console.WriteLine(result4);

Matrix result5 = mx1 / 2;
Console.WriteLine(result5);

Matrix trans = mx1.Transposed;
Console.WriteLine(trans);

Console.WriteLine($"m2[0,0] = {mx2[0, 0]}\n");

Matrix mx5 = new Matrix(2, 3);
Console.WriteLine(mx5);
mx5[0, 0] = 10;
mx5[0, 1] = 20;
mx5[0, 2] = 30;
mx5[1, 0] = 40;
mx5[1, 1] = 50;
mx5[1, 2] = 60;
Console.WriteLine(mx5);

double[,] qm1 = { { 4, 7 }, { 2, 6 } };
QMatrix qmx1 = new QMatrix(qm1);
Console.WriteLine(qmx1);


double[,] qm2 = { { 4, 7, 2 }, { 2, 6, 2 } };
QMatrix qmx2 = new QMatrix(qm2);
Console.WriteLine(qmx2);

QMatrix qmx3 = new QMatrix(mx2);
Console.WriteLine(qmx3);
Console.WriteLine(qmx3);

QMatrix zeroQmx = new QMatrix(3);
Console.WriteLine(zeroQmx);

Console.WriteLine($"Определитель qmx1: {qmx1.Determinant}\n");

QMatrix qmx4 = new QMatrix(new double[,]
        {
            { 2, 5, 3, 5, 1},
            { 1, 7, 4 , 5 ,10},
            { 6, 2, 9 , 0, 8},
            { 3, 4, 5, 9, 11 },
            {0, 15, 12, 6, 9 }
        });

double det = qmx4.Determinant;
Console.WriteLine($"\n\nОпределитель qmx4: {det}\n");

QMatrix inverse = qmx1.Inverse;
Console.WriteLine(inverse);

QMatrix matrix = new QMatrix(new double[,] { { 1, 2 }, { 3, 4 } });

matrix.ComputationProgress += MyProgressHandler;

var inverse2 = matrix.Inverse;

matrix.ComputationProgress -= MyProgressHandler;

static void MyProgressHandler(string message)
{
    Console.WriteLine($"Прогресс: {message}");
}

