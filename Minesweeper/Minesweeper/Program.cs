using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
    class Program
    {
        public int stageSize = 0;

        public void StageSizeDecision()
        {
            string input = Console.ReadLine();
            int num;
            if (int.TryParse(input, out num))
            {
                if (num >= 5 && num <= 20)
                {
                    stageSize = num;
                    return;
                }
                else
                {
                    Console.WriteLine("入力された数字が指定可能範囲ではありません。");
                    Console.WriteLine("もう一度入力しなおしてください。");
                }
            }
            else
            {
                Console.WriteLine("入力された文字は数字に変換できません。");
                Console.WriteLine("もう一度入力しなおしてください。");
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("「マインスイーパー」");

            var program = new Program();
            while(program.stageSize == 0)
            {
                Console.WriteLine("数字を入力してください。");
                Console.WriteLine("入力可能範囲は「５～２０」です。");
                program.StageSizeDecision();
            }

            //stage配列
            //左：横軸　右：縦軸
            int[,] stage = new int[program.stageSize, program.stageSize];

            do
            {
                for (int i = 0; i < program.stageSize; i++)
                {
                    for (int j = 0; j < program.stageSize; j++)
                    {
                        Console.Write(stage[j, i]);
                    }
                    Console.WriteLine();
                }
                Console.ReadKey();

            } while (true);
            

            Console.WriteLine("aaa");
        }
    }
}
