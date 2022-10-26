using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace MscThesis.Core.Algorithms
{
    /*
     * This code is heavily inspired from SO answer at
     * https://stackoverflow.com/questions/27427527/how-to-get-a-complete-row-or-column-from-2d-array-in-c-sharp
     */

    public static class ArrayExtension
    {
        public static double[] GetRow(this double[,] array, int row)
        {
            int cols = array.GetUpperBound(1) + 1;
            var size = Marshal.SizeOf<double>();
            var output = new double[cols];
            Buffer.BlockCopy(array, row * cols * size, output, 0, cols * size);
            return output;
        }

        public static double[] GetLastDimension(this double[,,,] array, int idx0, int idx1, int idx2)
        {
            var sizeDimension = array.GetLength(3);
            var output = new double[sizeDimension];
            for (int i = 0; i < sizeDimension; i++)
            {
                output[i] = array[idx0, idx1, idx2, i];
            }
            return output;
        }
    }
}
