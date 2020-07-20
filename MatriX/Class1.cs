using System;
using System.Collections.Generic;


namespace MatriX
{
    public class Matrix
    {
        public Type[,] matrix;
        public int height, width;
        public Type type;

        public Matrix(Type[,] matrix)
        {
            this.matrix = matrix;
            type = matrix[0, 0].GetType();
            height = matrix.GetLength(0);
            width = matrix.GetLength(1);
        }

        public Matrix transpose()
        {
            Type[,] transposedMat = new Type[width, height];
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

        public int[] shape()
        {
            return new int[] { height, width};
        }

        public (Matrix, bool) dot(Matrix otherMatrix)
        {
            if (isProductEnable(otherMatrix))
            {
                Type[,] result = new Type[this.height, otherMatrix.width];

                return (new Matrix(result), true);

            }
            else
            {
                return (null, false);
            }
        }

        private bool isProductEnable(Matrix otherMatrix)
        {
            return this.width == otherMatrix.height ? true : false;
        }

        public Type this[uint i, uint j]
        {
            set { matrix[i, j] = (Type)(object)value; }
            get { return matrix[i, j]; }
        }

        public Matrix this[int i =-1, uint j=0]
        {
            get {
                Type[] item = new Type[height];
                for(int idx=0; idx<height; idx++) {
                    item[idx] = matrix[j, idx];
                }
                return MatrixHandler.valueToMatrix(item).transpose();
            }
        }

        public Type[,] this[uint i]
        {
            get {
                Type[,] result = new Type[1, width];
                for(int idx=0; idx<width; idx++)
                {
                    result[0, idx] = matrix[i, idx];
                }
                return result;
            }
        }
    }

    public static class MatrixHandler
    {
        /// <summary>
        /// 行ベクトル（要素は0）を返す
        /// </summary>
        /// <param name="size">ベクトル長</param>
        /// <returns></returns>
        public static Matrix intZeros1d(int size)
        {
            return new Matrix(new Type[1, size]);
        }

        public static Matrix intZeros2d(int size1, int size2)
        {
            return new Matrix(new Type[size1, size2]);
        }

        public static Matrix valueToMatrix(Type[] items)
        {
            Type[,] result = new Type[1, items.Length];
            for(int i=0; i<items.Length; i++)
            {
                result[0, i] = items[i];
            }
            return new Matrix(result);
        }

    }

    public class Class1
    {
    }
}
