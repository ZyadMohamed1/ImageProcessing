using System;
using System.Collections.Generic;
using System.Text;

namespace ImageFilters
{
    class AlphaTrim
    {

        private static byte[,] setBorders(byte[,] matrix, int windowSize) // O(N^2)
        {
            int width = matrix.GetLength(1);
            int hight = matrix.GetLength(0);
            byte[,] newMatrix = new byte[hight + (windowSize - 1), width + (windowSize - 1)];


            for (int row = 0; row < newMatrix.GetLength(0); row++) // O(N)
            {
                for (int col = 0; col < newMatrix.GetLength(1); col++) // O(N)
                {
                    if (row < (((windowSize - 2) / 2) + 1) || row >= newMatrix.GetLength(0) - (((windowSize - 2) / 2) + 1))
                    {
                        newMatrix[row, col] = 0;
                    }
                    else if (col < (((windowSize - 2) / 2) + 1) || col >= newMatrix.GetLength(1) - (((windowSize - 2) / 2) + 1))
                    {
                        newMatrix[row, col] = 0;
                    }
                    else
                    {
                        newMatrix[row, col] = matrix[row - (windowSize / 2), col - (windowSize / 2)];
                    }

                }
            }
            return newMatrix;
        }

        private static byte[] getWindowBytes(byte[,] matrix, int row, int col, int windowHight, int windowWidth, int windowSize) //O(N^2)
        {
            int arrayCounter = 0;
            byte[] points = new byte[windowSize * windowSize];
            for (int i = row; i < windowHight; i++)
            {
                for (int j = col; j < windowWidth; j++)
                {
                    points[arrayCounter] = matrix[i, j];
                    arrayCounter++;
                }
            }
            return points;
        }

        private static int getCenterPosition(int matrixSize) //O(1)
        {
            return ((matrixSize / 2) + 1);

        }

        public static byte[,] AlphaFilter(byte[,] matrix, int windowSize, int alpha, int sortType) //O(N^4)
        {
            //Alpha trim var
            int start = alpha;
            int end = (windowSize * windowSize) - alpha;
            int matrixWidth, matrixHight;
            int windowHight = windowSize;
            int windowWidth;
            int center = getCenterPosition(windowSize); //O(1)

            //For select k
            int Kelement = alpha;
            matrix = setBorders(matrix, windowSize);


            matrixWidth = matrix.GetLength(1);
            matrixHight = matrix.GetLength(0);

           
            for (int row = 0; windowHight <= matrixHight; row++) // O(N)
            {

                windowWidth = windowSize;
                for (int col = 0; windowWidth <= matrixWidth; col++)  // O(N)
                {
                    

                    byte[] points = getWindowBytes(matrix, row, col, windowHight, windowWidth, windowSize); //O(N^2)
                    int sum = 0;

                    switch (sortType)
                    {
                        case 1:
                            points = sort.quickSort(points, 0, points.Length - 1); //O(N^2)
                            break;
                        case 2:
                            points = sort.CountingSort(points); //O(N+K)
                            break;
                        case 3:
                            {
                                points = sort.selectK(points, Kelement); //O(N)
                            }
                            break;
                        default:
                            points = sort.CountingSort(points);
                            break;
                    }

                    if (sortType == 3)
                    {
                        for (int i = 0; i < points.Length; i++) // O(N)
                        {
                            sum += points[i];
                        }

                        int Mean = (sum / points.Length);
                        byte MeanInByte = Convert.ToByte(Mean);
                        matrix[windowHight - center, windowWidth - center] = MeanInByte;

                          windowWidth++;
                    }
                    else
                    {
                        for (int i = start; i < end; i++) // O(N)
                        {
                            sum += points[i];
                        }
                        int Mean = (sum / ((windowSize * windowSize) - (alpha + alpha)));
                        byte MeanInByte = Convert.ToByte(Mean);
                        matrix[windowHight - center, windowWidth - center] = MeanInByte;

                        windowWidth++;
                    }

                }
                windowHight++;
            }

           
            return matrix;
        }

    }
}
