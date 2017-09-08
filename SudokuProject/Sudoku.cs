using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SudokuProject.SudokuProject
{
    class Sudoku
    {
        //数据存储
        List<List<int>> X = new List<List<int>>();//垂直
        List<List<int>> Y = new List<List<int>>();//水平
        List<int> Z;//小九宫格
        List<int> exceptYNum;//除去水平方向 剩余的数
        List<int> remainNum;//未使用的数

        //小九宫格 首格坐标 6个
        //int[] XLocation = new int[6] { 3, 6, 0, 6, 0, 3 };//水平方向
        //int[] YLocation = new int[6] { 0, 0, 3, 3, 6, 6 };//垂直方向 

        //小九宫格 首格坐标 8个
        int[] XLocation = new int[8] { 3, 6, 0, 3, 6, 0, 3, 6 };//水平方向
        int[] YLocation = new int[8] { 0, 0, 3, 3, 3, 6, 6, 6 };//垂直方向 

        //需填充小九宫格数量
        int littleSudokuNum = 8;
        
        //数独 9*9
        int[,] sudoku = new int[9, 9];

        #region Test
        //{ {5, 6, 7, 0, 0, 0, 0, 0, 0},
        //  {3, 2, 4, 0, 0, 0, 0, 0, 0},
        //  {1, 8, 9, 0, 0, 0, 0, 0, 0},
        //  {0, 0, 0, 4, 2, 5, 0, 0, 0},
        //  {0, 0, 0, 1, 7, 8, 0, 0, 0},
        //  {0, 0, 0, 6, 3, 9, 0, 0, 0},
        //  {0, 0, 0, 0, 0, 0, 4, 7, 8},
        //  {0, 0, 0, 0, 0, 0, 9, 2, 1},
        //  {0, 0, 0, 0, 0, 0, 3, 5, 6} };
        #endregion

        //XLocation和YLocation数组的指针
        int location = 0;
        //小九宫格位置上限
        int maxXL;
        int maxYL;
        //小九宫格完成数量
        int count = 0;
        //目标数量
        int N;
        //完成数量
        int n = 0;

        FileStream f;
        StreamWriter sw;

        /// <summary>
        /// 产生终盘
        /// </summary>
        /// <param name="n"></param>
        public void SudokuCreate(int s)
        {
            N = s;
            for (int i = 0; i < 9; i++)
            {
                X.Add(new List<int>());
                Y.Add(new List<int>());
            }
            AddZ1Num();
            //AddZNum();

            for (int i = 0; i < 81; i++)
            {
                if (sudoku[i % 9, i / 9] != 0)
                {
                    X[i % 9].Add(sudoku[i / 9, i % 9]);
                    Y[i / 9].Add(sudoku[i / 9, i % 9]);
                }
            }

            f = new FileStream("sudoku.txt", FileMode.Create, FileAccess.Write);
            sw = new StreamWriter(f);
            AddNineNum();
            sw.Close();
            f.Close();
        }

        /// <summary>
        /// 添加Z5和Z9的数据
        /// </summary>
        /// <param name="sudoku"></param>
        private void AddZNum()
        {
            RandomNum randomNum = new RandomNum();
            int[] value = new int[9] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            int[] randomNumArr = randomNum.GetRandomNum(value, 9);
            for (int i = 5, z = 8; i >= 3; i--)
                for (int j = 5; j >= 3 && z >= 0; j--, z--)
                    sudoku[j, i] = randomNumArr[z];
            Console.ReadLine();
            value = new int[9] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            randomNumArr = randomNum.GetRandomNum(value, 9);
            for (int i = 8, z = 8; i >= 6; i--)
                for (int j = 8; j >= 6 && z >= 0; j--, z--)
                    sudoku[j, i] = randomNumArr[z];
        }

        /// <summary>
        /// 添加Z1方块的数据
        /// </summary>
        /// <param name="sudoku"></param>
        public void AddZ1Num()
        {
            sudoku[0, 0] = 5;
            X[0].Add(5);
            Y[0].Add(5);
            RandomNum randomNum = new RandomNum();
            int[] value = new int[8] { 1, 2, 3, 4, 6, 7, 8, 9 };
            int[] randomNumArr = randomNum.GetRandomNum(value, 8);
            for (int i = 2, z = 7; i >= 0; i--)
            {
                for (int j = 2; j >= 0 && z >= 0; j--, z--)
                    sudoku[j, i] = randomNumArr[z];
            }
        }

        /// <summary>
        /// 填满小九宫格&&更换小九宫格
        /// </summary>
        public void AddNineNum()
        {
            if (n >= N) return;
            if (location == littleSudokuNum)
            {
                n++;
                PrintResultFile();
                return;
            }
            //PrintResult();
            List<int> temp;
            int a, b;
            a = maxYL;
            b = maxXL;
            maxYL = YLocation[location] + 3;
            maxXL = XLocation[location] + 3;
            temp = Z;
            Z = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            exceptYNum = Z.Except(Y[YLocation[location]]).ToList();
            AddOneNum(XLocation[location], YLocation[location], exceptYNum);
            Z = temp;
            maxYL = a;
            maxXL = b;
        }

        /// <summary>
        /// 单格填数
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="exceptYNum"></param>
        public void AddOneNum(int x, int y,List<int> exceptYNum)
        {
            if (n >= N) return;
            remainNum = exceptYNum.Except(X[x]).ToList();
            foreach(int i in remainNum)
            {
                count++;
                sudoku[y, x] = i;
                exceptYNum.Remove(i);
                X[x].Add(i);
                Y[y].Add(i);
                Z.Remove(i);
                if (x + 1 < maxXL) AddOneNum(x + 1, y, exceptYNum);
                else if (y + 1 < maxYL && Z.Except(Y[y + 1]).ToList().Count >= 3)
                    AddOneNum(x - 2, y + 1, Z.Except(Y[y + 1]).ToList());
                if (count == 9)
                {
                    count = 0;
                    location++;
                    AddNineNum();
                    location--;
                    count = 9;
                }
                Z.Add(i);
                X[x].Remove(i);
                Y[y].Remove(i);
                exceptYNum.Add(i);
                sudoku[y, x] = 0;
                count--;
            }
        }

        /// <summary>
        /// Console输出
        /// </summary>
        public void PrintResultConsole()
        {
            Console.WriteLine();
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                    Console.Write(sudoku[i, j] + " ");
                Console.WriteLine();
            }
        }

        /// <summary>
        /// File输出
        /// </summary>
        public void PrintResultFile()
        {
            try
            {
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                        sw.Write(sudoku[i, j] + " ");
                    sw.WriteLine();
                }
                sw.WriteLine();
                sw.Flush();
            }
            catch (IOException e)
            {
                throw e;
            }
        }
    }
}