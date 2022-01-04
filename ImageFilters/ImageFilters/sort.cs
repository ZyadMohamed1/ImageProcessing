using System;
using System.Collections.Generic;
using System.Text;

namespace ImageFilters
{
    class sort
    {
        public static byte getMin(byte[] matrix) // -----> o(n)
        {
            byte min = matrix[0];

            for (int i = 0; i < matrix.Length; i++)
            {
                byte number = matrix[i];

                if (number < min)
                {
                    min = number;
                }
            }

            return min;
        }

        public static byte getMax(byte[] matrix)
        {
            byte max = matrix[0];

            for (int i = 0; i < matrix.Length; i++)
            {
                byte number = matrix[i];

                if (number > max)
                {
                    max = number;
                }
            }

            return max;
        }
        
        public static int partition(byte[] Array, int pivot, int right)
        {
            byte rightValue = Array[right];
            byte Temp;
            int pivotValue = pivot;
            for (int j = pivot; j < right; j++)
            {
                if (Array[j] <= rightValue)
                {
                    Temp = Array[j];
                    Array[j] = Array[pivotValue];
                    Array[pivotValue++] = Temp;
                }
            }
            Temp = Array[pivotValue];
            Array[pivotValue] = Array[right];
            Array[right] = Temp;
            return pivotValue;
        }

        public static byte[] quickSort(byte[] Array, int pivot, int right)
        {           
            if (pivot < right)
            {
                int q = partition(Array, pivot, right);
                quickSort(Array, pivot, q - 1);
                quickSort(Array, q + 1, right);
            }
            return Array;
        }

        public static byte[] CountingSort(byte[] arr)
        {
            byte minimum = getMin(arr);
            byte maximum = getMax(arr);

            byte[] count = new byte[maximum - minimum + 1];
            int itr = 0;

            for (int i = 0; i < count.Length; i++)
            {
                count[i] = 0;
            }

            for (int i = 0; i < arr.Length; i++)
            {
                count[arr[i] - minimum]++;
            }

            for (int i = minimum; i <= maximum; i++)
            {
                while (count[i - minimum]-- > 0)
                {
                    arr[itr] = (byte)i;
                    itr++;
                }
            }

            return arr;
        }

        public static int kthSmallest(byte[] Array, int left, int right, int k)
        {

            if (k > 0 && k <= right - left + 1)
            {

                int pos = partition(Array, left, right);

                if (pos - left == k - 1)
                    return Array[pos];


                if (pos - left > k - 1)
                    return kthSmallest(Array, left, pos - 1, k);

                return kthSmallest(Array, pos + 1, right,
                                   k - pos + left - 1);
            }

            return int.MaxValue;
        }

        public static int kthLargest(byte[] arr, int k)
        {
            k = arr.Length - k;
            int low = 0;
            int high = arr.Length - 1;
            int res = 0;
            while (low <= high)
            {
                res = partition(arr, low, high);
                if (res == k)
                {
                    return arr[res];
                }
                else if (res > k)
                {
                    high = res - 1;
                }
                else
                {
                    low = res + 1;
                }
            }
            return res;
        }

        public static byte[] selectK(byte[] arr, int k) 
        {
            byte[] tmp1 = new byte[arr.Length - 1];
            byte[] tmp2 = new byte[tmp1.Length - 1];
       

            bool firstElement = false;
            bool secondElement = false;

            int small = kthSmallest(arr, 0, arr.Length - 1, k);

            int cnt1 = 0;
            int cnt2 = 0;
            for (int i = 0; i < arr.Length; i++)
            {

                if (arr[i] == small && firstElement == false)
                {
                    firstElement = true;
                    continue;
                }
                else
                {
                    tmp1[cnt1] = arr[i];
                    cnt1++;
                }

            }
            int large = kthLargest(tmp1, k);

            for (int i = 0; i < tmp1.Length; i++)
            {

                if (tmp1[i] == large && secondElement == false)
                {
                    secondElement = true;
                    continue;
                }
                else
                {
                    tmp2[cnt2] = tmp1[i];
                    cnt2++;
                }

            }
            return tmp2;
        }
    }
}
