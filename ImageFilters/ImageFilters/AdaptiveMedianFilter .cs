using System;
using System.Collections.Generic;
using System.Text;

namespace ImageFilters
{
    class AdaptiveMedianFilter
    {
        private static byte[,] setBorders(byte[,] matrix) // -----> o(n^2)
        {
            int width = ImageOperations.GetWidth(matrix);       // -----> o(1)
            int hight = ImageOperations.GetHeight(matrix);      // -----> o(1)
            byte[,] newMatrix = new byte[hight + 2, width + 2];  
            for (int row = 0; row < hight + 2; row++)              // -----> o(n) ----> o(N)*o(N) = O(N^2)
            {
                for (int col = 0; col < width + 2; col++)          // -----> o(n)
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
        private static int getCenterPosition(int matrixSize) // O(1)
        {
            return ((matrixSize / 2) + 1);
        }
        private static byte[] getWindowBytes(byte[,] matrix, int row, int col, int windowHight, int windowWidth, int windowSize)// O(N^2)
        {
            int arrayCounter = 0;
            byte[] points = new byte[windowSize * windowSize];
            int i = row;
            int j = col;
            while (i < windowHight && j < windowWidth)
            {
                points[arrayCounter] = matrix[i, j];
                arrayCounter++;
                i++;

                if (j < windowWidth && i == windowHight)
                {
                    i = row;
                    j++;
                }
            }
            return points;
        }
        private static byte[] getMed(byte[] points , int sortType) // -----> O(N^2) slowest
        {
            byte[] arr = new byte[4];
            byte[] sorted;
            switch (sortType)
            {
                case 1:
                    sorted = sort.quickSort(points, 0, points.Length - 1);
                    break;
                case 2:
                    sorted = sort.CountingSort(points);
                    break;
                default:
                    sorted = sort.quickSort(points, 0, points.Length - 1);
                    break;
            }
            arr[2] = sorted[points.Length - 1];
            arr[1] = sorted[points.Length / 2];
            arr[0] = sorted[0];
            int A1 = arr[1] - arr[0];
            int A2 = arr[2] - arr[1];
            if (A1 > 0 && A2 > 0)
            {
                arr[3] = 1;
                return arr;
            }
            else
            {
                arr[3] = 0;
                return arr;
            }
        }
        public static byte[,] adaptiveFilter(byte[,] matrix, int windowSize, int maxWindowSize, int sortType) // -----> O(N^4)
        {
            int matrixWidth, matrixHight;
            int windowHight = windowSize;
            int windowWidth;
            int maxWidth = maxWindowSize;
            int maxHight = maxWindowSize;
            matrix = setBorders(matrix); //--->O(N^2)
            matrixWidth = ImageOperations.GetWidth(matrix);
            matrixHight = ImageOperations.GetHeight(matrix);

            for (int row = 0; windowHight <= matrixHight; row++) // -----> O(N) * O(N^3) = O(N^4)
            {
                windowWidth = windowSize;
                for (int col = 0; windowWidth <= matrixWidth; col++) //----> O(N) * O(N^2) = O(N^3)
                {
                    byte[] points = getWindowBytes(matrix, row, col, windowHight, windowWidth, windowSize); // ---> O(N^2)
                    byte[] arr = getMed(points, sortType); // -----> O(N^2)
                    int center = getCenterPosition(windowSize);  // O(1)
                    if (arr[3] == 1)
                    {
                        byte max = arr[2];
                        byte min = arr[0];
                        byte windowCenter;
                        windowCenter = matrix[windowHight - center
                           , windowWidth - center];
                        int B1 = windowCenter - min;
                        int B2 = max - windowCenter;
                 
                        if (!(B1 > 0 && B2 > 0))
                        {
                            matrix[windowHight - center
                            , windowWidth - center] = arr[1];
                        }
                 
                        windowWidth++;
                    }
                    else
                    {
                        if (windowSize <= maxHight && windowSize <= maxWidth)
                        {

                            col = col - 1;
                            windowSize += 2;
                            windowWidth += 2;
                            windowHight += 2;
                            matrix = setBorders(matrix);
                            matrixWidth = ImageOperations.GetWidth(matrix);
                            matrixHight = ImageOperations.GetHeight(matrix);
                        }
                        else
                        {
                            
                            byte max = arr[2];
                            byte min = arr[0];
                            byte windowCenter;
                            windowCenter = matrix[windowHight - center
                               , windowWidth - center];
                            int B1 = windowCenter - min;
                            int B2 = max - windowCenter;
                            if (!(B1 > 0 && B2 > 0))
                            {
                                matrix[windowHight - center
                                  , windowWidth - center] = arr[1];
                            }
                            windowWidth++;
                        }
                    }
                }
                windowHight++;
            }

            return matrix;
        }
        }
    }
