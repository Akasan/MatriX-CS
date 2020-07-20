using System;
using System.Collections.Generic;


namespace MatriX
{
    public class Matrix
    {
        /// <summary>
        /// double型の二次元配列（行、列ベクトルは内部では二次元として扱う）
        /// </summary>
        public double[,] matrix;

        /// <summary>
        /// 行列の行数
        /// </summary>
        public int height;

        /// <summary>
        /// 行列の列数
        /// </summary>
        public int width;

        public Matrix(double[,] matrix)
        {
            this.matrix = matrix;
            height = matrix.GetLength(0);
            width = matrix.GetLength(1);
        }

        public Matrix(Matrix matrix)
        {
            this.matrix = matrix.matrix;
            this.height = matrix.height;
            this.width = matrix.width;
        }

        public Matrix(int height, int width)
        {
            this.matrix = MatrixHandler.zeros2d(height, width).matrix;
            this.height = height;
            this.width = width;
        }

        /// <summary>
        /// 転置行列を作成
        /// </summary>
        /// <returns></returns>
        public Matrix transpose()
        {
            double[,] transposedMat = new double[width, height];
            int i, j;

            for (i = 0; i < height; i++)
            {
                for (j = 0; j < width; j++)
                {
                    transposedMat[j, i] = matrix[i, j];
                }
            }
            return new Matrix(transposedMat);
        }

        /// <summary>
        /// 行列の行数、列数を取得
        /// </summary>
        /// <returns></returns>
        public int[] shape()
        {
            return new int[] { height, width };
        }

        /// <summary>
        /// 行列同士のドット積を計算する
        /// </summary>
        /// <param name="otherMatrix"></param>
        /// <returns></returns>
        public (Matrix, bool) dot(Matrix otherMatrix)
        {
            if (isProductEnable(otherMatrix))
            {
                double[,] result = new double[this.height, otherMatrix.width];

                for (uint baseI = 0; baseI < height; baseI++)
                {
                    for (uint otherJ = 0; otherJ < otherMatrix.width; otherJ++)
                    {
                        double sum = 0.0;
                        for (uint baseJ = 0; baseJ < width; baseJ++)
                        {
                            sum += matrix[baseI, baseJ] * otherMatrix[baseJ, otherJ];
                        }
                    }
                }
                return (new Matrix(result), true);
            }
            else
            {
                return (null, false);
            }
        }

        /// <summary>
        /// ドット積が計算可能か確認する
        /// </summary>
        /// <param name="otherMatrix"></param>
        /// <returns></returns>
        private bool isProductEnable(Matrix otherMatrix)
        {
            return this.width == otherMatrix.height ? true : false;
        }

        public double this[uint i, uint j]
        {
            set { matrix[i, j] = (int)(object)value; }
            get { return matrix[i, j]; }
        }

        public Matrix this[int i = -1, uint j = 0]
        {
            get
            {
                double[] item = new double[height];
                for (int idx = 0; idx < height; idx++)
                {
                    item[idx] = matrix[j, idx];
                }
                return MatrixHandler.valueToRowVector(item).transpose();
            }
        }

        public double[,] this[uint i]
        {
            get
            {
                double[,] result = new double[1, width];
                for (int idx = 0; idx < width; idx++)
                {
                    result[0, idx] = matrix[i, idx];
                }
                return result;
            }
        }

        public static Matrix operator +(Matrix baseMatrix, Matrix otherMatrix)
        {
            if (baseMatrix.isSameShape(otherMatrix))
            {
                Matrix result = new Matrix(baseMatrix);
                uint i, j;

                for(i=0; i< baseMatrix.height; i++)
                {
                    for(j=0; j< baseMatrix.width; j++)
                    {
                        result[i, j] += otherMatrix[i, j];
                    }
                }
                return result;
            }
            else
            {
                return null;
            }
        }

        public static Matrix operator +(Matrix baseMatrix, double value)
        {
            Matrix result = new Matrix(baseMatrix);
            uint i, j;

            for (i = 0; i < baseMatrix.height; i++)
            {
                for (j = 0; j < baseMatrix.width; j++)
                {
                    result[i, j] += value;
                }
            }
            return result;
        }

        public static Matrix operator -(Matrix baseMatrix, Matrix otherMatrix)
        {
            if (baseMatrix.isSameShape(otherMatrix))
            {
                Matrix result = new Matrix(baseMatrix);
                uint i, j;

                for (i = 0; i < baseMatrix.height; i++)
                {
                    for (j = 0; j < baseMatrix.width; j++)
                    {
                        result[i, j] -= otherMatrix[i, j];
                    }
                }
                return result;
            }
            else
            {
                return null;
            }
        }

        public static Matrix operator -(Matrix baseMatrix, double value)
        {
            Matrix result = new Matrix(baseMatrix);
            uint i, j;

            for (i = 0; i < baseMatrix.height; i++)
            {
                for (j = 0; j < baseMatrix.width; j++)
                {
                    result[i, j] -= value;
                }
            }
            return result;
        }

        public static Matrix operator *(Matrix baseMatrix, Matrix otherMatrix)
        {
            if (baseMatrix.isSameShape(otherMatrix))
            {
                Matrix result = new Matrix(baseMatrix);
                uint i, j;

                for (i = 0; i < baseMatrix.height; i++)
                {
                    for (j = 0; j < baseMatrix.width; j++)
                    {
                        result[i, j] *= otherMatrix[i, j];
                    }
                }
                return result;
            }
            else
            {
                return null;
            }
        }

        public static Matrix operator *(Matrix baseMatrix, double value)
        {
            Matrix result = new Matrix(baseMatrix);
            uint i, j;

            for (i = 0; i < baseMatrix.height; i++)
            {
                for (j = 0; j < baseMatrix.width; j++)
                {
                    result[i, j] *= value;
                }
            }
            return result;
        }

        public Matrix applyFunction(Func<double, double> applyFunc)
        {
            Matrix result = new Matrix(height, width);
            uint i, j;

            for(i=0; i<height; i++)
            {
                for(j=0; j<width; j++)
                {
                    result[i, j] = applyFunc(result[i, j]);
                }
            }
            return result;
        }

        /// <summary>
        /// 二つの行列が同じ形か確認する
        /// </summary>
        /// <param name="otherMatrix"></param>
        /// <returns></returns>
        public bool isSameShape(Matrix otherMatrix)
        {
            return (this.width == otherMatrix.width) && (this.height == otherMatrix.height) ? true : false;
        }
    }

    public static class MatrixHandler
    {
        /// <summary>
        /// 行ベクトル（要素は0）を返す
        /// </summary>
        /// <param name="size">ベクトル長</param>
        /// <returns></returns>
        public static dynamic zeros1d(int size)
        {
            return new Matrix(new double[1, size]);
        }

        public static Matrix zeros2d(int size1, int size2)
        {
            return new Matrix(new double[size1, size2]);
        }

        public static Matrix valueToRowVector(double[] items)
        {
            double[,] result = new double[1, items.Length];
            for(int i=0; i<items.Length; i++)
            {
                result[0, i] = items[i];
            }
            return new Matrix(result);
        }
    }
}
