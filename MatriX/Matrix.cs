using System;
using System.Linq;
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
            this.matrix = MatrixUtility.zeros2d(height, width).matrix;
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
            // 列ベクトルと行ベクトル同士の演算に書き換える
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
            set { matrix[i, j] = (double)(object)value; }
            get { return matrix[i, j]; }
        }

        public double[,] this[int i = -1, uint j = 0]
        {
            get
            {
                double[,] result = new double[height, 1];
                for (int idx = 0; idx < height; idx++)
                {
                    result[idx, 0] = matrix[j, idx];
                }
                return result;
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

        // 以下はクラスの中身自体を変更する
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
                        result.matrix[i, j] += otherMatrix.matrix[i, j];
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
                    result.matrix[i, j] += value;
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
                        result.matrix[i, j] -= otherMatrix.matrix[i, j];
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
                    result.matrix[i, j] -= value;
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
                        result.matrix[i, j] *= otherMatrix.matrix[i, j];
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
                    result.matrix[i, j] *= value;
                }
            }
            return result;
        }

        public static Matrix operator /(Matrix baseMatrix, double value)
        {
            if(value == 0.0)
            {
                throw new DivideByZeroException("0で割ることはできません");
            }

            Matrix result = new Matrix(baseMatrix);
            uint i, j;

            for (i = 0; i < baseMatrix.height; i++)
            {
                for (j = 0; j < baseMatrix.width; j++)
                {
                    result.matrix[i, j] /= value;
                }
            }
            return result;
        }

        /// <summary>
        ///  行列の要素同士を足し算した結果を返す
        /// </summary>
        /// <param name="otherMatrix">足すマトリックス</param>
        /// <returns></returns>
        public Matrix add(Matrix otherMatrix)
        {
            Matrix result = new Matrix(matrix);
            result += otherMatrix;
            return result;
        }
        
        /// <summary>
        ///  行列の要素同士を引き算した結果を返す
        /// </summary>
        /// <param name="otherMatrix">引くマトリックス</param>
        /// <returns></returns>
        public Matrix subtract(Matrix otherMatrix)
        {
            Matrix result = new Matrix(matrix);
            result -= otherMatrix;
            return result;
        }

        /// <summary>
        ///  行列の要素同士を掛け合わせた結果を返す（ドット積はdot関数を利用してください）
        /// </summary>
        /// <param name="otherMatrix">かけるマトリックス</param>
        /// <returns></returns>
        public Matrix multiply(Matrix otherMatrix)
        {
            Matrix result = new Matrix(matrix);
            result *= otherMatrix;
            return result;
        }

        /// <summary>
        /// すべての要素に対して関数を適用する(実装中)
        /// </summary>
        /// <param name="applyFunc">適用する関数</param>
        /// <returns></returns>
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

        /// <summary>
        /// 行列の内容を表示する
        /// </summary>
        public void describe()
        {
            uint i, j;
            Console.Write("[[");
            for(i=0; i<height; i++)
            {
                Console.WriteLine("");
                for(j=0; j<width; j++)
                {
                    Console.Write($" {matrix[i, j]}");
                }
                Console.Write("]");
            }
            Console.WriteLine("]");
        }

        /// <summary>
        /// 配列の形状を変える
        /// </summary>
        /// <param name="size1">新しい形状の行数</param>
        /// <param name="size2">新しい形状の列数</param>
        public Matrix reshape(int size1, int size2)
        {
            if(size1*size2 != width * height)
            {
                throw new ArgumentException("指定した配列サイズは有効ではありません");
            }

            double[,] result = new double[size1, size2];
            
            for(uint i=0; i<size1*size2; i++)
            {
                result[i / size2, i % size2] = matrix[i / width, i % width];
            }

            return new Matrix(result);
        }

        /// <summary>
        /// 逆行列を計算する(実装中)
        /// </summary>
        /// <returns></returns>
        public Matrix inv()
        {
            if(width == height)
            {
                return null;
            }

            double[,] result = new double[height, width];

            return new Matrix(result);
        }

        /// <summary>
        /// すべての値の符号を+にする
        /// </summary>
        public Matrix abs()
        {
            double[,] result = (double[,])matrix.Clone();
            uint i, j;

            for(i=0; i<height; i++)
            {
                for(j=0; j<width; j++)
                {
                    result[i, j] = Math.Abs(matrix[i, j]);
                }
            }

            return new Matrix(result);
        }

        public double sum()
        {
            double result = 0.0;
            uint i, j;
            for(i=0; i<height; i++)
            {
                for(j=0; j<width; j++)
                {
                    result += matrix[i, j];
                }
            }
            return result;
        }

        public Matrix vectorize()
        {
            Matrix vector = reshape(1, width * height);
            return vector;
        }
    }

    public static class MatrixUtility
    {
        /// <summary>
        /// 行ベクトル（要素は0）を返す
        /// </summary>
        /// <param name="size">ベクトル長</param>
        /// <returns></returns>
        public static Matrix zeros1d(int size, bool isRowVector=true)
        {
            
            return isRowVector ? new Matrix(new double[1, size]) : new Matrix(new double[size, 1]);
        }

        public static Matrix oneValue1d(int size, double initValue, bool isRowVector=true)
        {
            double[,] result = isRowVector ? new double[1, size] : new double[size, 1];
            for(int i=0; i<size; i++)
            {
                if (isRowVector)
                {
                    result[0, i] = initValue;
                }
                else
                {
                    result[i, 0] = initValue;
                }
            }
            return new Matrix(result);
        }

        public static Matrix fromValue1d(params double[] value)
        {
            double[,] result = new double[1, value.Length];
            for(int i=0; i<value.Length; i++)
            {
                result[0, i] = value[i];
            }
            return new Matrix(result);
        }

        public static Matrix zeros2d(int size1, int size2)
        {
            return new Matrix(new double[size1, size2]);
        }

        public static Matrix oneValue2d(int size1, int size2, double initValue)
        {
            double[,] result = new double[size1, size2];
            int i, j;
            for(i=0; i<size1; i++)
            {
                for(j=0; j<size2; j++)
                {
                    result[i, j] = initValue;
                }
            }

            return new Matrix(result);
        }

        public static Matrix fromValue2d(params double[] value)
        {
            double[,] result = new double[1, value.Length];
            for (int i = 0; i < value.Length; i++)
            {
                result[0, i] = value[i];
            }
            return new Matrix(result);
        }

        public static Matrix unitVector2d(int size)
        {
            double[,] result = new double[size, size];
            for(int i=0; i<size; i++)
            {
                result[i, i] = 1.0;
            }
            return new Matrix(result);
        }

        public static double norm(double[,] vector, int normDim)
        {
            double result = 0.0;
            
            foreach(double item in vector)
            {
                result += Math.Pow(item, normDim);
            }
            result = Math.Pow(result, 1.0 / normDim);
            return result;
        }
        
        public static Matrix hstack(Matrix matrix1, Matrix matrix2)
        {
            if(matrix1.height != matrix2.height)
            {
                throw new ArgumentException("おなじ高さのマトリックスを指定してください");
            }

            double[,] result = new double[matrix1.height, matrix1.width + matrix2.width];
            uint w, h;

            for(w=0; w<matrix1.width; w++)
            {
                for(h=0; h<matrix1.height; h++)
                {
                    result[h, w] = matrix1[h, w];
                }
            }

            for (w = 0; w < matrix2.width; w++)
            {
                for (h = 0; h < matrix1.height; h++)
                {
                    result[h, w+matrix1.width] = matrix2[h, w];
                }
            }

            return new Matrix(result);
        }

        public static Matrix vstack(Matrix matrix1, Matrix matrix2)
        {
            if (matrix1.width != matrix2.width)
            {
                throw new ArgumentException("おなじ高さのマトリックスを指定してください");
            }

            double[,] result = new double[matrix1.height + matrix2.height, matrix1.width];
            uint w, h;

            for (w = 0; w < matrix1.width; w++)
            {
                for (h = 0; h < matrix1.height; h++)
                {
                    result[h, w] = matrix1[h, w];
                }
            }

            for (w = 0; w < matrix1.width; w++)
            {
                for (h = 0; h < matrix2.height; h++)
                {
                    result[h+matrix1.height, w] = matrix2[h, w];
                }
            }

            return new Matrix(result);
        }
    }
}
