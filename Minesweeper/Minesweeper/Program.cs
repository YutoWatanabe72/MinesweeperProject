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
        public int playerPosX = 3;
        public int playerPosY = 3;
        public int[,] stage = new int[0,0];
        public int[,] stageFlag = new int[0, 0];
        public int created = 0;

        //盤面サイズの設定
        public void StageSizeDecision()
        {
            while (stageSize == 0)
            {
                Console.WriteLine("数字を入力してください。");
                Console.WriteLine("入力可能範囲は「５～２０」です。");


                //数字の入力
                string input = Console.ReadLine();
                int num;
                if (int.TryParse(input, out num))
                {
                    if (num >= 5 && num <= 20)
                    {
                        stageSize = num;

                        //stage配列
                        //左：横軸　右：縦軸
                        stage = new int[stageSize, stageSize];
                        stageFlag = new int[stageSize, stageSize];
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
        }

        //盤面の表示
        public void StageDisp()
        {
            

            for (int i = 0; i < stageSize; i++)
            {
                for (int j = 0; j < stageSize; j++)
                {
                    if (playerPosX == j && playerPosY == i)
                    {
                        Console.Write("◎");
                    }
                    else
                    {
                        Console.Write(stage[j,i]);
                    }
                }
                Console.WriteLine();
            }
        }

        //入力されたキー情報のチェック
        public void KeyCheck()
        {
            ConsoleKeyInfo c = Console.ReadKey(true);
            switch (c.Key)
            {
                case ConsoleKey.UpArrow:
                    if(playerPosY > 0 && playerPosY <= stageSize - 1)PlayerMove(0,-1);
                    break;
                case ConsoleKey.DownArrow:
                    if (playerPosY >= 0 && playerPosY < stageSize - 1) PlayerMove(0, 1);
                    break;
                case ConsoleKey.LeftArrow:
                    if (playerPosX > 0 && playerPosX <= stageSize - 1) PlayerMove(-1, 0);
                    break;
                case ConsoleKey.RightArrow:
                    if (playerPosX >= 0 && playerPosX < stageSize - 1) PlayerMove(1, 0);
                    break;
                case ConsoleKey.Enter:
                    CreateStage();
                    stageFlag[playerPosX, playerPosY] = 1;
                    break;
                default:
                    break;
            }

        }

        //プレイヤーの移動
        public void PlayerMove(int _x, int _y)
        {
            playerPosX += _x;
            playerPosY += _y;
        }

        //ステージの生成
        public void CreateStage()
        {
            if (created != 0) return;
            int count = 0;
            while(count < stageSize)
            {
                Random rnd = new Random();
                int x = rnd.Next(0, stageSize);
                int y = rnd.Next(0, stageSize);
                if(stage[x,y] == 0 && x != playerPosX && y != playerPosY)
                {
                    stage[x, y] = 1;
                    count++;
                }
            }
            created++;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("「マインスイーパー」");

            var program = new Program();
            
            //盤面サイズの決定メソッド
            program.StageSizeDecision();

            do
            {
                Console.Clear();
                Console.WriteLine("「マインスイーパー」");
                Console.WriteLine();

                program.StageDisp();
                program.KeyCheck();

            } while (true);
            

        }
    }
}
