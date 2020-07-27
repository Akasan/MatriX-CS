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
        public double[,] Mat;

        /// <summary>
        /// 行列の行数
        /// </summary>
        public int Height;

        /// <summary>
        /// 行列の列数
        /// </summary>
        public int Width;

        public Matrix(double[,] matrix)
        {
            this.Mat = matrix;
            Height = matrix.GetLength(0);
            Width = matrix.GetLength(1);
        }

        public Matrix(Matrix matrix)
        {
            this.Mat = matrix.Mat;
            this.Height = matrix.Height;
            this.Width = matrix.Width;
        }

        public Matrix(int height, int width)
        {
            this.Mat = MatrixUtility.Zeros2d(Height, Width).Mat;
            this.Height = Height;
            this.Width = Width;
        }

        public Matrix Clone()
        {
            return new Matrix(Mat);
        }

        /// <summary>
        /// 転置行列を作成
        /// </summary>
        /// <returns></returns>
        public Matrix Transpose()
        {
            double[,] transposedMat = new double[Width, Height];
            int i, j;

            for (i = 0; i < Height; i++)
            {
                for (j = 0; j < Width; j++)
                {
                    transposedMat[j, i] = Mat[i, j];
                }
            }
            return new Matrix(transposedMat);
        }

        /// <summary>
        /// 行列の行数、列数を取得
        /// </summary>
        /// <returns></returns>
        public int[] Shape()
        {
            return new int[] { Height, Width };
        }

        /// <summary>
        /// 行列同士のドット積を計算する
        /// </summary>
        /// <param name="otherMatrix"></param>
        /// <returns></returns>
        public Matrix Dot(Matrix otherMatrix)
        {
            // 列ベクトルと行ベクトル同士の演算に書き換える
            if (IsProductEnable(otherMatrix))
            {
                double[,] result = new double[this.Height, otherMatrix.Width];
                int baseH, otherW, baseJ;
                double sum;

                for (baseH = 0; baseH < Height; baseH++)
                {
                    for (otherW = 0; otherW < otherMatrix.Width; otherW++)
                    {
                        sum = 0.0;
                        for (baseJ = 0; baseJ < Width; baseJ++)
                        {
                            sum += Mat[baseH, baseJ] * otherMatrix[baseJ, otherW];
                        }
                        result[baseH, otherW] = sum;
                    }
                }
                return new Matrix(result);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// ドット積が計算可能か確認する
        /// </summary>
        /// <param name="otherMatrix"></param>
        /// <returns></returns>
        private bool IsProductEnable(Matrix otherMatrix)
        {
            return this.Width == otherMatrix.Height ? true : false;
        }

        /// <summary>
        /// 特定の要素を抽出する
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public double this[int i, int j]
        {
            set { Mat[i, j] = (double)(object)value; }
            get {
                return Mat[i, j]; }
        }

        // 以下はクラスの中身自体を変更する
        public static Matrix operator +(Matrix baseMatrix, Matrix otherMatrix)
        {
            if (baseMatrix.IsSameShape(otherMatrix))
            {
                Matrix result = new Matrix(baseMatrix);
                int i, j;

                for(i=0; i< baseMatrix.Height; i++)
                {
                    for(j=0; j< baseMatrix.Width; j++)
                    {
                        result.Mat[i, j] += otherMatrix.Mat[i, j];
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
            int i, j;

            for (i = 0; i < baseMatrix.Height; i++)
            {
                for (j = 0; j < baseMatrix.Width; j++)
                {
                    result.Mat[i, j] += value;
                }
            }
            return result;
        }

        public static Matrix operator -(Matrix baseMatrix, Matrix otherMatrix)
        {
            if (baseMatrix.IsSameShape(otherMatrix))
            {
                Matrix result = new Matrix(baseMatrix);
                int i, j;

                for (i = 0; i < baseMatrix.Height; i++)
                {
                    for (j = 0; j < baseMatrix.Width; j++)
                    {
                        result.Mat[i, j] -= otherMatrix.Mat[i, j];
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
            int i, j;

            for (i = 0; i < baseMatrix.Height; i++)
            {
                for (j = 0; j < baseMatrix.Width; j++)
                {
                    result.Mat[i, j] -= value;
                }
            }
            return result;
        }

        public static Matrix operator *(Matrix baseMatrix, Matrix otherMatrix)
        {
            if (baseMatrix.IsSameShape(otherMatrix))
            {
                Matrix result = new Matrix(baseMatrix);
                int i, j;

                for (i = 0; i < baseMatrix.Height; i++)
                {
                    for (j = 0; j < baseMatrix.Width; j++)
                    {
                        result.Mat[i, j] *= otherMatrix.Mat[i, j];
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
            int i, j;

            for (i = 0; i < baseMatrix.Height; i++)
            {
                for (j = 0; j < baseMatrix.Width; j++)
                {
                    result.Mat[i, j] *= value;
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
            int i, j;

            for (i = 0; i < baseMatrix.Height; i++)
            {
                for (j = 0; j < baseMatrix.Width; j++)
                {
                    result.Mat[i, j] /= value;
                }
            }
            return result;
        }

        /// <summary>
        ///  行列の要素同士を足し算した結果を返す
        /// </summary>
        /// <param name="otherMatrix">足すマトリックス</param>
        /// <returns></returns>
        public Matrix Add(Matrix otherMatrix)
        {
            Matrix result = new Matrix(Mat);
            result += otherMatrix;
            return result;
        }
        
        /// <summary>
        ///  行列の要素同士を引き算した結果を返す
        /// </summary>
        /// <param name="otherMatrix">引くマトリックス</param>
        /// <returns></returns>
        public Matrix Subtract(Matrix otherMatrix)
        {
            Matrix result = new Matrix(Mat);
            result -= otherMatrix;
            return result;
        }

        /// <summary>
        ///  行列の要素同士を掛け合わせた結果を返す（ドット積はdot関数を利用してください）
        /// </summary>
        /// <param name="otherMatrix">かけるマトリックス</param>
        /// <returns></returns>
        public Matrix Multiply(Matrix otherMatrix)
        {
            Matrix result = new Matrix(Mat);
            result *= otherMatrix;
            return result;
        }

        /// <summary>
        /// すべての要素に対して関数を適用する(実装中)
        /// </summary>
        /// <param name="applyFunc">適用する関数</param>
        /// <returns></returns>
        public Matrix ApplyFunction(Func<double, double> applyFunc)
        {
            Matrix result = new Matrix(Height, Width);
            int i, j;

            for(i=0; i< Height; i++)
            {
                for(j=0; j< Width; j++)
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
        public bool IsSameShape(Matrix otherMatrix)
        {
            return (this.Width == otherMatrix.Width) && (this.Height == otherMatrix.Height) ? true : false;
        }

        /// <summary>
        /// 行列の内容を表示する
        /// </summary>
        public void Describe()
        {
            int i, j;
            Console.Write("[[");
            for(i=0; i<Height; i++)
            {
                Console.WriteLine("");
                for(j=0; j<Width; j++)
                {
                    Console.Write($" {Mat[i, j]}");
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
        public Matrix Reshape(int size1, int size2)
        {
            if(size1*size2 != Width * Height)
            {
                throw new ArgumentException("指定した配列サイズは有効ではありません");
            }

            double[,] result = new double[size1, size2];
            
            for(int i=0; i<size1*size2; i++)
            {
                result[i / size2, i % size2] = Mat[i / Width, i % Width];
            }

            return new Matrix(result);
        }

        /// <summary>
        /// 逆行列を計算する。
        /// 実装は以下のページを参考（https://qiita.com/sekky0816/items/8c73a7ec32fd9b040127）
        /// </summary>
        /// <returns></returns>
        public Matrix Inv()
        {
            if(Width != Height)
            {
                return null;
            }

            Matrix result = MatrixUtility.UnitVector2d(Height);
            Matrix clone = Clone();

            int max;
            double tmp;
            int i, j, k;

            for(k=0; k<Height; k++)
            {
                max = k;
                for(j=k+1; j<Height; j++)
                {
                    if(Math.Abs(clone[j, k]) > Math.Abs(clone[max, k]))
                    {
                        max = j;
                    }
                }

                if(max != k)
                {
                    for(i=0; i<Width; i++)
                    {
                        (clone[max, i], clone[k, i]) = (clone[k, i], clone[max, i]);
                        (result[max, i], result[k, i]) = (result[k, i], result[max, i]);
                    }
                }

                tmp = clone[k, k];

                for (i=0; i< Width; i++)
                {
                    clone[k, i] /= tmp;
                    result[k, i] /= tmp;
                }

                for(j=0; j<Height; j++)
                {
                    if(j != k)
                    {
                        tmp = clone[j, k] / clone[k, k];
                        for (i = 0; i< Width; i++)
                        {
                            clone[j, i] -= clone[k, i] * tmp;
                            result[j, i] -= result[k, i] * tmp;
                        }
                    }
                }
            }


            return result;
        }

        /// <summary>
        /// すべての値の符号を+にする
        /// </summary>
        public Matrix Abs()
        {
            double[,] result = (double[,])Mat.Clone();
            int i, j;

            for(i=0; i<Height; i++)
            {
                for(j=0; j<Width; j++)
                {
                    result[i, j] = Math.Abs(Mat[i, j]);
                }
            }

            return new Matrix(result);
        }

        /// <summary>
        /// すべての要素の和を計算
        /// </summary>
        /// <returns></returns>
        public double Sum()
        {
            double result = 0.0;
            int i, j;
            for(i=0; i<Height; i++)
            {
                for(j=0; j<Width; j++)
                {
                    result += Mat[i, j];
                }
            }
            return result;
        }

        /// <summary>
        /// 指定した行ベクトルを取得
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        public Matrix GetRowVector(int rowIndex)
        {
            double[,] result = new double[1, Width];
            for(int i=0; i<Width; i++)
            {
                result[0, i] = Mat[rowIndex, i];
            }
            return new Matrix(result);
        }

        /// <summary>
        /// 指定した行ベクトルを取得
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        public Matrix GetColumnVector(int columnIndex)
        {
            double[,] result = new double[Height, 1];
            for (int i = 0; i < Height; i++)
            {
                result[i, 0] = Mat[i, columnIndex];
            }
            return new Matrix(result);
        }

        /// <summary>
        /// 各行および列ごとの総和を計算
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public Matrix Sum(int axis = 0)
        {
            // axis 0: 行方向　　1: 列方向
            if (axis == 0)
            {
                double[,] result = new double[Height, 1];
                for(int i=0; i< Height; i++)
                {
                    Matrix temp = this.GetRowVector(i);
                    result[i, 0] = temp.Sum();
                }
                return new Matrix(result);
            }
            else
            {
                double[,] result = new double[1, Width];
                for (int i = 0; i < Width; i++)
                {
                    Matrix temp = this.GetColumnVector(i);
                    result[0, i] = temp.Sum();
                }
                return new Matrix(result);
            }
        }

        /// <summary>
        /// 各行および列ごとの平均を計算
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public Matrix Average(int axis = 0)
        {
            // axis 0: 行方向　　1: 列方向
            if (axis == 0)
            {
                double[,] result = new double[Height, 1];
                for (int i = 0; i < Height; i++)
                {
                    Matrix temp = this.GetRowVector(i);
                    result[i, 0] = temp.Average();
                }
                return new Matrix(result);
            }
            else
            {
                double[,] result = new double[1, Width];
                for (int i = 0; i < Width; i++)
                {
                    Matrix temp = this.GetColumnVector(i);
                    result[0, i] = temp.Average();
                }
                return new Matrix(result);
            }
        }

        /// <summary>
        /// 一次元ベクトルに変換
        /// </summary>
        /// <returns></returns>
        public Matrix Flatten()
        {
            Matrix vector = Reshape(1, Width * Height);
            return vector;
        }

        /// <summary>
        /// 全要素の平均をとる
        /// </summary>
        /// <returns></returns>
        public double Average()
        {
            return Sum() / (Width * Height);
        }

        public double Median()
        {
            List<double> list = new List<double>();
            int length;

            for (int i=0; i<Height; i++)
            {
                for(int j=0; j<Width; j++)
                {
                    list.Add(Mat[i, j]);
                }
            }
            list.Sort();
            length = list.Count();

            if(length % 2 == 0)
            {
                return (list[length / 2 - 1] + list[length / 2]) / 2;
            }
            else
            {
                return list[length / 2];
            }
        }
    }

    public static class MatrixUtility
    {
        /// <summary>
        /// 行ベクトル（要素は0）を返す
        /// </summary>
        /// <param name="size">ベクトル長</param>
        /// <returns></returns>
        public static Matrix Zeros1d(int size, bool isRowVector=true)
        {
            
            return isRowVector ? new Matrix(new double[1, size]) : new Matrix(new double[size, 1]);
        }

        public static Matrix OneValue1d(int size, double initValue, bool isRowVector=true)
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

        public static Matrix FromValue1d(params double[] value)
        {
            double[,] result = new double[1, value.Length];
            for(int i=0; i<value.Length; i++)
            {
                result[0, i] = value[i];
            }
            return new Matrix(result);
        }

        public static Matrix Zeros2d(int size1, int size2)
        {
            return new Matrix(new double[size1, size2]);
        }

        public static Matrix OneValue2d(int size1, int size2, double initValue)
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

        public static Matrix FromValue2d(params double[] value)
        {
            double[,] result = new double[1, value.Length];
            for (int i = 0; i < value.Length; i++)
            {
                result[0, i] = value[i];
            }
            return new Matrix(result);
        }

        /// <summary>
        /// 任意のサイズの単位ベクトルを生成する
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Matrix UnitVector2d(int size)
        {
            double[,] result = new double[size, size];
            for(int i=0; i<size; i++)
            {
                result[i, i] = 1.0;
            }
            return new Matrix(result);
        }

        /// <summary>
        /// ベクトルのノルムを計算する
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="normDim"></param>
        /// <returns></returns>
        public static double Norm(double[,] vector, int normDim)
        {
            double result = 0.0;
            
            foreach(double item in vector)
            {
                result += Math.Pow(item, normDim);
            }
            result = Math.Pow(result, 1.0 / normDim);
            return result;
        }
        
        /// <summary>
        /// 行列を水平方向に結合する
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns></returns>
        public static Matrix HStack(Matrix matrix1, Matrix matrix2)
        {
            if(matrix1.Height != matrix2.Height)
            {
                throw new ArgumentException("おなじ高さのマトリックスを指定してください");
            }

            double[,] result = new double[matrix1.Height, matrix1.Width + matrix2.Width];
            int w, h;

            for(w=0; w<matrix1.Width; w++)
            {
                for(h=0; h<matrix1.Height; h++)
                {
                    result[h, w] = matrix1[h, w];
                }
            }

            for (w = 0; w < matrix2.Width; w++)
            {
                for (h = 0; h < matrix1.Height; h++)
                {
                    result[h, w+matrix1.Width] = matrix2[h, w];
                }
            }

            return new Matrix(result);
        }

        /// <summary>
        /// 行列を垂直方向に結合する
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns></returns>
        public static Matrix VStack(Matrix matrix1, Matrix matrix2)
        {
            if (matrix1.Width != matrix2.Width)
            {
                throw new ArgumentException("おなじ高さのマトリックスを指定してください");
            }

            double[,] result = new double[matrix1.Height + matrix2.Height, matrix1.Width];
            int w, h;

            for (w = 0; w < matrix1.Width; w++)
            {
                for (h = 0; h < matrix1.Height; h++)
                {
                    result[h, w] = matrix1[h, w];
                }
            }

            for (w = 0; w < matrix1.Width; w++)
            {
                for (h = 0; h < matrix2.Height; h++)
                {
                    result[h+matrix1.Height, w] = matrix2[h, w];
                }
            }

            return new Matrix(result);
        }

        public static Matrix Arange(double start, double end, double interval)
        {
            List<double> tempList = new List<double>();
            int count = 0;
            double tmp;
            while (true)
            {
                tmp = start + interval * count;
                if(tmp<= end + Math.Pow(10.0, -10))
                {
                    tempList.Add(tmp);
                    count += 1;
                }
                else
                {
                    break;
                }
            }
            double[,] result = new double[1, tempList.Count()];
            for(int i=0; i<tempList.Count(); i++)
            {
                result[0, i] = tempList[i];
            }
            return new Matrix(result);
        }

        /*public static Matrix random(int size1, int size2 = -1)
        {

        }*/
    }
}
