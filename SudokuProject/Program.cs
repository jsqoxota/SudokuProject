using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuProject.SudokuProject
{
    class Program
    {
        static void Main(string[] args)
        {
            RandomNum randomNum = new RandomNum();
            int[] value = new int[8]{ 1, 2, 3, 4, 6, 7, 8, 9 };
            int[] randomNumArr = randomNum.GetRandomNum(value, 8);
            foreach(int num in randomNumArr)
                Console.Write(num+" ");
            Console.WriteLine();
            Console.WriteLine("Finish");
        }
    }
}
