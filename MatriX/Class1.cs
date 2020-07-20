using System;
using System.Collections.Generic;


namespace MatriX
{
    public static class Matrix
    {
        public static Type[] intZeros1d(int size)
        {
            Type[] zerosArray = new Type[size];
            int intTemp = 0;
            var temp = (Type)(object)intTemp;
            
            for (int i = 0; i < size; i++)
            {
                zerosArray[i] = temp;
            }
            return zerosArray;
        }

        public static Type[,] intZeros2d(int size1, int size2)
        {
            Type[,] zerosArray = new Type[size1, size2];
            int i, j;
            int intTemp = 0;
            var temp = (Type)(object)intTemp;
            for (i = 0; i < size1; i++)
            {
                for(j=0; j<size2; j++)
                {
                    zerosArray[i, j] = temp;
                }
            }
            return zerosArray;
        }

        public static Type[,] transpose(Type[,] originalMat)
        {
            Type[,] transposedMat = new Type[originalMat.GetLength(1), originalMat.GetLength(0)];
            int i, j;
            for(i=0; i<originalMat.GetLength(0); i++)
            {
                for(j=0; j<originalMat.GetLength(1); j++)
                {
                    transposedMat[j, i] = originalMat[i, j];
                }
            }
            return transposedMat;
        }

        public static (Type[,], bool) product(Type[,] baseMatrix, Type[,] compMatrix)
        {
            
            if (isProductEnable(baseMatrix, compMatrix))
            {
                Type[,] result = new Type[baseMatrix.GetLength(0), compMatrix.GetLength(1)];

                return (result, true);
            }
            else
            {
                return (new Type[0, 0], false); 
            }
        }

        public static bool isProductEnable(Type[,] baseMatrix, Type[,] compMatrix)
        {
            return baseMatrix.GetLength(1) == compMatrix.GetLength(0) ? true : false;
        }
    }

    public class Class1
    {
    }
}
