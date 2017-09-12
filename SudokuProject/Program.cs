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
            Sudoku sudoku = new Sudoku();
            //RandomNum randomNum = new RandomNum();
            //int[] value = new int[8]{ 1, 2, 3, 4, 6, 7, 8, 9 };
            //int[] randomNumArr = randomNum.GetRandomNum(value, 8);
            //foreach(int num in randomNumArr)
            //    Console.Write(num+" ");
            //Console.WriteLine();

            //sudoku.SudokuCreate(1000000);

            if (args.Length != 2 || args[0] != "-c")
            {
                Console.WriteLine("error:请参考以下格式进行输入;");
                Console.WriteLine("sudoku.exe -c 20");
                return;
            }

            string N = args[1];
            Console.WriteLine("N = " + N);
            int result;
            if (int.TryParse(N, out result))
            {
                if (result > 0 && result <= 1000000)
                {
                    Console.WriteLine("Begin");
                    sudoku.SudokuCreate(int.Parse(N));
                    Console.WriteLine("Finish");
                }
                else Console.WriteLine("error:输入范围1<N<=1000000");
            }
            else Console.WriteLine("error:请输入正整数");
        }
    }
}
