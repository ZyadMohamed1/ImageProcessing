using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace ImageFilters
{
    class FastMedian
    {

        private static byte[,] setBorders(byte[,] matrix)
        {
            int width = ImageOperations.GetWidth(matrix);
            int hight = ImageOperations.GetHeight(matrix);
            byte[,] newMatrix = new byte[hight + 2, width + 2];
            for (int row = 0; row < hight + 2; row++)
            {
                for (int col = 0; col < width + 2; col++)
                {
                    if (row == 0 || row == hight + 1)
                    {
                        newMatrix[row, col] = 0;
                    }
                    else if (col == 0 || col == width + 1)
                    {
                        newMatrix[row, col] = 0;
                    }
                    else
                    {
                        newMatrix[row, col] = matrix[row - 1, col - 1];
                    }
                }
            }
            return newMatrix;
        }
        private static int getCenterPosition(int matrixSize)
        {
            return ((matrixSize / 2) + 1);
        }
        private static Queue<byte> getWindowBytes(byte[,] matrix, int row, int col, int windowHight, int windowWidth, int windowSize)
        {
            Queue<byte> points = new Queue<byte>();
            int i = row;
            int j = col;
            while (i < windowHight && j < windowWidth)
            {
                points.Enqueue(matrix[i, j]);
                i++;

                if (j < windowWidth && i == windowHight)
                {
                    i = row;
                    j++;
                }
            }
            return points;
        }

        private static Queue<byte> getNextColumn(Queue<byte> points, byte[,] matrix, int row, int col, int windowHight, int windowWidth, int windowSize)
        {

            int i = row;
            int j = col + (windowSize-1);

            for(int k = 0; k < windowSize; k++)
            {
                points.Enqueue(matrix[i, j]);
                i++;
            }

            return points;
        }

        
        public static byte[,] FastFilter(byte[,] matrix, int windowSize)
        {
            int matrixWidth, matrixHight;
            int windowHight = windowSize;
            int windowWidth;
            matrix = setBorders(matrix);
            matrixWidth = ImageOperations.GetWidth(matrix);
            matrixHight = ImageOperations.GetHeight(matrix);
            int center = getCenterPosition(windowSize);

            //Deque
            Queue<byte> points = new Queue<byte>();
           
            


            for (int row = 0; windowHight <= matrixHight; row++)
            {
                windowWidth = windowSize;
                for (int col = 0; windowWidth <= matrixWidth; col++)
                {
                    
                    if(col == 0)
                    {
                        //make it empty
                        points = getWindowBytes(matrix, row, col, windowHight, windowWidth, windowSize);
                    }
                    else
                    {
                        for(int i = 0; i < windowSize; i++)
                        {
                            points.Dequeue();
                        }

                        points = getNextColumn(points, matrix, row, col, windowHight, windowWidth, windowSize);
                    }

                    byte[] sorted = points.ToArray();
                    sorted = sort.CountingSort(sorted);
                    int median = sorted[(sorted.Length/2) + 1];
                    byte MedianInByte = Convert.ToByte(median);
                    matrix[windowHight - center, windowWidth - center] = MedianInByte;


                    windowWidth++;
                }
                windowHight++;
            }

            return matrix;
        }
    }
}
