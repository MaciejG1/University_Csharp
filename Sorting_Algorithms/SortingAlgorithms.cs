using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class Program
{
    public static void Main()
    {
        //variables declaration
        string textToSorting;
        char[] Letters;
        char[] Letters1;
        char[] Letters2;
        char[] Letters3;
        char[] Letters4;
        char[] Letters5;
        //Input Data to Sort
        Console.Write("Podaj tekst do posortowania: ");
        textToSorting = Console.ReadLine();
        textToSorting = textToSorting.ToUpper();
        Letters = textToSorting.ToCharArray();
        Letters1 = textToSorting.ToCharArray();
        Letters2 = textToSorting.ToCharArray();
        Letters3 = textToSorting.ToCharArray();
        Letters4 = textToSorting.ToCharArray();
        Letters5 = textToSorting.ToCharArray();
        //Output of sorted Data 
        Console.WriteLine("\nSortowanie przez proste wstawianie: " + Sorting.BasicInsertionSorting(Letters)); 
        Console.WriteLine("Sortowanie przez proste wybieranie: " + Sorting.SelectSorting(Letters1));
        Console.WriteLine("Sortowanie bąbelkowe: " + Sorting.BubbleSorting(Letters2));
        Console.WriteLine("Sortowanie koktajlowe: " + Sorting.ShakeSorting(Letters3));
        Console.WriteLine("Sortowanie szybkie: " + Sorting.QuickSorting(Letters4, 0, Letters4.Length - 1));
        Console.WriteLine("Sortowanie stogowe: " + Sorting.HeapSorting(Letters5));
    }
}



////////////////////////////////////////////////////////////////////////////////////////////////////


public class Sorting
{
    //Sorting functions
    public static string BasicInsertionSorting(char[] array)
    {

        for (int i = 1; i < array.Length; i++)
        {
            char key = array[i];

            while (i - 1 >= 0 && Convert.ToInt16(array[i - 1]) > Convert.ToInt16(key))
            {

                array[i] = array[i - 1];
                array[i - 1] = key;
                i -= 1;
            }

        }
        string sortedWord = new string(array);
        return sortedWord;
    }
    public static string SelectSorting(char[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            int tempMinNumber = i;
            for (int j = i + 1; j < array.Length; j++)
            {
                if (Convert.ToInt16(array[j]) < Convert.ToInt16(array[tempMinNumber]))
                    tempMinNumber = j;
            }
            swap(array, i, tempMinNumber);
        }
        string sortedWord = new string(array);
        return sortedWord;
    }
    public static string BubbleSorting(char[] array)
    {
        int n = array.Length;
        for (int j = 0; j < n - 1; j++)
        {
            for (int i = 0; i < n - 1 - j; i++)
            {
                if (Convert.ToInt16(array[i]) > Convert.ToInt16(array[i + 1]))
                {
                    swap(array, i, i + 1);
                }
            }

        }
        string sortedWord = new string(array);
        return sortedWord;
    }
    public static string ShakeSorting(char[] array)
    {
        int bottom = 0;
        int top = array.Length - 1;
        bool replace = true;

        while (replace)
        {
            replace = false;

            for (int i = bottom; i < top; i++)
            {
                if (Convert.ToInt16(array[i]) > Convert.ToInt16(array[i + 1]))
                {
                    swap(array, i, i + 1);
                    replace = true;
                }
            }


            top--;
            for (int i = top; i > bottom; i--)
            {
                if (Convert.ToInt16(array[i]) < Convert.ToInt16(array[i - 1]))
                {
                    swap(array, i, i - 1);
                    replace = true;
                }
            }

            bottom++;
        }
        string sortedWord = new string(array);
        return sortedWord;
    }
    public static string QuickSorting(char[] array, int poczatek, int koniec)
    {
        int i, j; 
        char pivot;
        i = (poczatek + koniec) / 2;
        pivot = array[i];
        array[i] = array[koniec];
        j = 0;
        for (j = i = poczatek; i < koniec; i++)
        {
            if (Convert.ToInt16(array[i]) < Convert.ToInt16(pivot))
            {
                swap(array,i,j);
                j++;
            }
        }
        array[koniec]= array[j];
        array[j] = pivot;
        if(poczatek<j-1) QuickSorting(array,poczatek,j-1);
        if (j + 1 < koniec) QuickSorting(array, j + 1, koniec);
       
        string sortedWord = new string(array);
        return sortedWord;
    }
    public static string HeapSorting(char[] array)
    {
     
        int n = array.Length;
        for (int i = n / 2 - 1; i >= 0; i--)
        {
            ValidateMaxHeap(array, n, i);
        }
        for (int i = n - 1; i > 0; i--)
        {
            swap(array, 0, i);
           ValidateMaxHeap(array, --n, 0);
        }
        string sortedWord = new string(array);
        return sortedWord;
      

    }
    static void ValidateMaxHeap(char[] array, int heapSize, int parentIndex)
    {
        int maxIndex = parentIndex;
        int leftChild = parentIndex * 2 + 1;
        int rightChild = parentIndex * 2 + 2;
        if (leftChild < heapSize && Convert.ToInt16(array[leftChild]) > Convert.ToInt16(array[maxIndex]))
            maxIndex = leftChild;
        if (rightChild < heapSize && Convert.ToInt16(array[rightChild]) > Convert.ToInt16(array[maxIndex]))
            maxIndex = rightChild;
        if (maxIndex != parentIndex)
        {
            swap(array,parentIndex,maxIndex);
            ValidateMaxHeap(array,heapSize,maxIndex);
        }
    }
    static void swap(char[] array, int a, int b)
    {
        char temp = array[a];
        array[a] = array[b];
        array[b] = temp;
    }
}
