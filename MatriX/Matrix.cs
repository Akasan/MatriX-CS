using System;
using System.Linq;
using System.Collections.Generic;


namespace MatriX
{
    public class DMatrix
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

        public DMatrix(double[,] matrix)
        {
            Mat = matrix;
            Height = matrix.GetLength(0);
            Width = matrix.GetLength(1);
        }

        public DMatrix(int height, int width)
        {
            this.Mat = DMatrixUtility.Zeros2d(height, width).Mat;
            this.Height = height;
            this.Width = width;
        }

        public DMatrix Clone()
        {
            return new DMatrix((double[,])Mat.Clone());
        }

        /// <summary>
        /// 転置行列を作成
        /// </summary>
        /// <returns></returns>
        public DMatrix Transpose()
        {
            DMatrix transposedMat = new DMatrix(Width, Height);
            int i, j;

            for (i = 0; i < Height; i++)
            {
                for (j = 0; j < Width; j++)
                {
                    transposedMat[j, i] = Mat[i, j];
                }
            }
            return transposedMat;
        }

        /// <summary>
        /// 行列の行数、列数を取得
        /// </summary>
        /// <returns></returns>
        public (int, int) Shape()
        {
            return (Height, Width);
        }

        /// <summary>
        /// 行列同士のドット積を計算する
        /// </summary>
        /// <param name="otherDMatrix"></param>
        /// <returns></returns>
        public DMatrix Dot(DMatrix otherDMatrix)
        {
            // 列ベクトルと行ベクトル同士の演算に書き換える
            if (IsProductEnable(otherDMatrix))
            {
                DMatrix result = new DMatrix(this.Height, otherDMatrix.Width);
                int baseH, otherW, baseJ;
                double sum;

                for (baseH = 0; baseH < Height; baseH++)
                {
                    for (otherW = 0; otherW < otherDMatrix.Width; otherW++)
                    {
                        sum = 0.0;
                        for (baseJ = 0; baseJ < Width; baseJ++)
                        {
                            sum += Mat[baseH, baseJ] * otherDMatrix[baseJ, otherW];
                        }
                        result[baseH, otherW] = sum;
                    }
                }
                return result;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// ドット積が計算可能か確認する
        /// </summary>
        /// <param name="otherDMatrix"></param>
        /// <returns></returns>
        private bool IsProductEnable(DMatrix otherDMatrix)
        {
            return this.Width == otherDMatrix.Height ? true : false;
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

        public double? this[int i]
        {
            set {
                if(Height == 1)
                {
                    Mat[0, i] = (double)(object)value;
                }
                else if(Width == 1)
                {
                    Mat[i, 0] = (double)(object)value;
                }
                else { }
            }
            get {
                if (Height == 1)
                {
                    return Mat[0, i];
                }
                else if (Width == 1)
                {
                    return Mat[i, 0];
                }
                else {
                    return null;
                }
            }
        }

        // 以下はクラスの中身自体を変更する
        public static DMatrix operator +(DMatrix baseDMatrix, DMatrix otherDMatrix)
        {
            if (baseDMatrix.IsSameShape(otherDMatrix))
            {
                DMatrix result = baseDMatrix.Clone();
                int i, j;

                for(i=0; i< baseDMatrix.Height; i++)
                {
                    for(j=0; j< baseDMatrix.Width; j++)
                    {
                        result.Mat[i, j] += otherDMatrix.Mat[i, j];
                    }
                }
                return result;
            }
            else
            {
                return null;
            }
        }

        public static DMatrix operator +(DMatrix baseDMatrix, double value)
        {
            DMatrix result = baseDMatrix.Clone();
            int i, j;

            for (i = 0; i < baseDMatrix.Height; i++)
            {
                for (j = 0; j < baseDMatrix.Width; j++)
                {
                    result.Mat[i, j] += value;
                }
            }
            return result;
        }

        public static DMatrix operator -(DMatrix baseDMatrix, DMatrix otherDMatrix)
        {
            if (baseDMatrix.IsSameShape(otherDMatrix))
            {
                DMatrix result = baseDMatrix.Clone();
                int i, j;

                for (i = 0; i < baseDMatrix.Height; i++)
                {
                    for (j = 0; j < baseDMatrix.Width; j++)
                    {
                        result.Mat[i, j] -= otherDMatrix.Mat[i, j];
                    }
                }
                return result;
            }
            else
            {
                return null;
            }
        }

        public static DMatrix operator -(DMatrix baseDMatrix, double value)
        {
            DMatrix result = baseDMatrix.Clone();
            int i, j;

            for (i = 0; i < baseDMatrix.Height; i++)
            {
                for (j = 0; j < baseDMatrix.Width; j++)
                {
                    result.Mat[i, j] -= value;
                }
            }
            return result;
        }

        public static DMatrix operator *(DMatrix baseDMatrix, DMatrix otherDMatrix)
        {
            if (baseDMatrix.IsSameShape(otherDMatrix))
            {
                DMatrix result = baseDMatrix.Clone();
                int i, j;

                for (i = 0; i < baseDMatrix.Height; i++)
                {
                    for (j = 0; j < baseDMatrix.Width; j++)
                    {
                        result.Mat[i, j] *= otherDMatrix.Mat[i, j];
                    }
                }
                return result;
            }
            else
            {
                return null;
            }
        }

        public static DMatrix operator *(DMatrix baseDMatrix, double value)
        {
            DMatrix result = baseDMatrix.Clone();
            int i, j;

            for (i = 0; i < baseDMatrix.Height; i++)
            {
                for (j = 0; j < baseDMatrix.Width; j++)
                {
                    result.Mat[i, j] *= value;
                }
            }
            return result;
        }

        public static DMatrix operator /(DMatrix baseDMatrix, double value)
        {
            if(value == 0.0)
            {
                throw new DivideByZeroException("0で割ることはできません");
            }

            DMatrix result = baseDMatrix.Clone();
            int i, j;

            for (i = 0; i < baseDMatrix.Height; i++)
            {
                for (j = 0; j < baseDMatrix.Width; j++)
                {
                    result.Mat[i, j] /= value;
                }
            }
            return result;
        }

        /// <summary>
        ///  行列の要素同士を足し算した結果を返す
        /// </summary>
        /// <param name="otherDMatrix">足すマトリックス</param>
        /// <returns></returns>
        public DMatrix Add(DMatrix otherDMatrix)
        {
            DMatrix result = Clone();
            result += otherDMatrix;
            return result;
        }
        
        /// <summary>
        ///  行列の要素同士を引き算した結果を返す
        /// </summary>
        /// <param name="otherDMatrix">引くマトリックス</param>
        /// <returns></returns>
        public DMatrix Subtract(DMatrix otherDMatrix)
        {
            DMatrix result = Clone();
            result -= otherDMatrix;
            return result;
        }

        /// <summary>
        ///  行列の要素同士を掛け合わせた結果を返す（ドット積はdot関数を利用してください）
        /// </summary>
        /// <param name="otherDMatrix">かけるマトリックス</param>
        /// <returns></returns>
        public DMatrix Multiply(DMatrix otherDMatrix)
        {
            DMatrix result = Clone();
            result *= otherDMatrix;
            return result;
        }

        /// <summary>
        /// すべての要素に対して関数を適用する(実装中)
        /// </summary>
        /// <param name="applyFunc">適用する関数</param>
        /// <returns></returns>
        public DMatrix ApplyFunction(Func<double, double> applyFunc)
        {
            DMatrix result = Clone();
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
        /// <param name="otherDMatrix"></param>
        /// <returns></returns>
        public bool IsSameShape(DMatrix otherDMatrix)
        {
            return (this.Width == otherDMatrix.Width) && (this.Height == otherDMatrix.Height) ? true : false;
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
        public DMatrix Reshape(int size1, int size2)
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

            return new DMatrix(result);
        }

        /// <summary>
        /// 逆行列を計算する。
        /// 実装は以下のページを参考（https://qiita.com/sekky0816/items/8c73a7ec32fd9b040127）
        /// TODO https://docs.microsoft.com/ja-jp/archive/msdn-magazine/2012/december/csharp-matrix-decomposition#%E8%A1%8C%E5%88%97%E5%BC%8F　も参考にして組み込む
        /// </summary>
        /// <returns></returns>
        public DMatrix Inv()
        {
            if(Width != Height)
            {
                return null;
            }

            DMatrix result = DMatrixUtility.UnitVector2d(Height);
            DMatrix clone = Clone();

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

            for(j=0; j<Height; j++)
            {
                for (i = 0; i < Width; i++)
                {
                    if(double.IsNaN(result[j, i]))
                    {
                        return null;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// すべての値の符号を+にする
        /// </summary>
        public DMatrix Abs()
        {
            DMatrix result = Clone();
            int i, j;

            for(i=0; i<Height; i++)
            {
                for(j=0; j<Width; j++)
                {
                    result[i, j] = Math.Abs(Mat[i, j]);
                }
            }

            return result;
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
        /// 各行および列ごとの総和を計算
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public DMatrix Sum(int axis = 0)
        {
            DMatrix result;
            // axis 0: 行方向　　1: 列方向
            if (axis == 0)
            {
                result = new DMatrix(Height, 1);
                for (int i = 0; i < Height; i++)
                {
                    DMatrix temp = this.GetRowVector(i);
                    result[i, 0] = temp.Sum();
                }
            }
            else
            {
                result = new DMatrix(1, Width);
                for (int i = 0; i < Width; i++)
                {
                    DMatrix temp = this.GetColumnVector(i);
                    result[0, i] = temp.Sum();
                }
            }

            return result;
        }

        /// <summary>
        /// 指定した行ベクトルを取得
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        public DMatrix GetRowVector(int rowIndex)
        {
            DMatrix result = new DMatrix(1, Width);

            for(int i=0; i<Width; i++)
            {
                result[0, i] = Mat[rowIndex, i];
            }
            return result;
        }

        /// <summary>
        /// 指定した行ベクトルを取得
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        public DMatrix GetColumnVector(int columnIndex)
        {
            DMatrix result = new DMatrix(Height, 1);
            for (int i = 0; i < Height; i++)
            {
                result[i, 0] = Mat[i, columnIndex];
            }
            return result;
        }

        /// <summary>
        /// 全要素の平均をとる
        /// </summary>
        /// <returns></returns>
        public double Average()
        {
            return Sum() / (Width * Height);
        }

        /// <summary>
        /// 各行および列ごとの平均を計算
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public DMatrix Average(int axis = 0)
        {
            DMatrix result, temp;
            // axis 0: 行方向　　1: 列方向
            if (axis == 0)
            {
                result = new DMatrix(Height, 1);
                for (int i = 0; i < Height; i++)
                {
                    temp = this.GetRowVector(i);
                    result[i, 0] = temp.Average();
                }
            }
            else
            {
                result = new DMatrix(1, Width);
                for (int i = 0; i < Width; i++)
                {
                    temp = this.GetColumnVector(i);
                    result[0, i] = temp.Average();
                }
            }

            return result;
        }

        /// <summary>
        /// 一次元ベクトルに変換
        /// </summary>
        /// <returns></returns>
        public DMatrix Flatten()
        {
            return Reshape(1, Width * Height);
        }

        public double Median()
        {
            List<double> list = new List<double>();
            int length, i, j;
            DMatrix cloneFlattened = Clone().Flatten();

            for(i=0; i<Width*Height; i++)
            {
                list.Add(Mat[0, i]);
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

        public DMatrix Median(int axis=0)
        {
            DMatrix result, temp;
            // axis 0: 行方向　　1: 列方向
            if (axis == 0)
            {
                result = new DMatrix(Height, 1);
                for (int i = 0; i < Height; i++)
                {
                    temp = this.GetRowVector(i);
                    result[i, 0] = temp.Median();
                }
            }
            else
            {
                result = new DMatrix(1, Width);
                for (int i = 0; i < Width; i++)
                {
                    temp = this.GetColumnVector(i);
                    result[0, i] = temp.Median();
                }
            }

            return result;
        }

        public double Std()
        {
            List<double> list = new List<double>();
            int i, j;
            double average = 0.0, std = 0.0;
            DMatrix cloneFlattened = Clone().Flatten();

            for (i = 0; i < Width * Height; i++)
            {
                average += Mat[0, i];
            }

            average /= Width * Height;

            for (i = 0; i < Width * Height; i++)
            {
                std += Math.Pow(Mat[0, i] - average, 2.0);
            }

            return Math.Sqrt(std / (Width * Height));
        }

        public DMatrix Std(int axis = 0)
        {
            DMatrix result, temp;
            // axis 0: 行方向　　1: 列方向
            if (axis == 0)
            {
                result = new DMatrix(Height, 1);
                for (int i = 0; i < Height; i++)
                {
                    temp = this.GetRowVector(i);
                    result[i, 0] = temp.Std();
                }
            }
            else
            {
                result = new DMatrix(1, Width);
                for (int i = 0; i < Width; i++)
                {
                    temp = this.GetColumnVector(i);
                    result[0, i] = temp.Std();
                }
            }

            return result;
        }

        public DMatrix Pow(double coef)
        {
            DMatrix clone = Clone();
            return clone.ApplyFunction(delegate(double item) { return Math.Pow(item, coef); });
        }

        public double Determinant()
        {
            if (Width != Height)
            {
                return 0.0;
            }

            DMatrix result = DMatrixUtility.UnitVector2d(Height);
            DMatrix clone = Clone();

            int max;
            double tmp = 0.0;
            int i, j, k;

            for (k = 0; k < Height; k++)
            {
                max = k;
                for (j = k + 1; j < Height; j++)
                {
                    if (Math.Abs(clone[j, k]) > Math.Abs(clone[max, k]))
                    {
                        max = j;
                    }
                }

                if (max != k)
                {
                    for (i = 0; i < Width; i++)
                    {
                        (clone[max, i], clone[k, i]) = (clone[k, i], clone[max, i]);
                        (result[max, i], result[k, i]) = (result[k, i], result[max, i]);
                    }
                }

                tmp = clone[k, k];
            }

            return tmp;
        }

        public DMatrix Sqrt()
        {
            return Clone().Pow(0.5);
        }
    }

    public static class DMatrixUtility
    {
        /// <summary>
        /// 行ベクトル（要素は0）を返す
        /// </summary>
        /// <param name="size">ベクトル長</param>
        /// <returns></returns>
        public static DMatrix Zeros1d(int size, bool isRowVector=true)
        {
            
            return isRowVector ? new DMatrix(new double[1, size]) : new DMatrix(new double[size, 1]);
        }

        public static DMatrix OneValue1d(int size, double initValue, bool isRowVector=true)
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
            return new DMatrix(result);
        }

        public static DMatrix FromValue1d(params double[] value)
        {
            double[,] result = new double[1, value.Length];
            for(int i=0; i<value.Length; i++)
            {
                result[0, i] = value[i];
            }
            return new DMatrix(result);
        }

        public static DMatrix Zeros2d(int size1, int size2)
        {
            return new DMatrix(new double[size1, size2]);
        }

        public static DMatrix OneValue2d(int size1, int size2, double initValue)
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

            return new DMatrix(result);
        }

        public static DMatrix FromValue2d(params double[] value)
        {
            double[,] result = new double[1, value.Length];
            for (int i = 0; i < value.Length; i++)
            {
                result[0, i] = value[i];
            }
            return new DMatrix(result);
        }

        /// <summary>
        /// 任意のサイズの単位ベクトルを生成する
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static DMatrix UnitVector2d(int size)
        {
            double[,] result = new double[size, size];
            for(int i=0; i<size; i++)
            {
                result[i, i] = 1.0;
            }
            return new DMatrix(result);
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
        public static DMatrix HStack(DMatrix matrix1, DMatrix matrix2)
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

            return new DMatrix(result);
        }

        /// <summary>
        /// 行列を垂直方向に結合する
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns></returns>
        public static DMatrix VStack(DMatrix matrix1, DMatrix matrix2)
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

            return new DMatrix(result);
        }

        public static DMatrix Arange(double start, double end, double interval)
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
            return new DMatrix(result);
        }

        /*public static DMatrix random(int size1, int size2 = -1)
        {

        }*/
    }
}
