﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
    class Program
    {
        public int stageSize = 0;//盤面のサイズ
        public int bomCount = 0;
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
                        bomCount = stageSize;

                        //stage配列
                        //左：横軸　右：縦軸
                        stage = new int[stageSize, stageSize];
                        stageFlag = new int[stageSize, stageSize];
                        created = 0;
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
                        if (stageFlag[j, i] == 1)
                        {
                            int n = stage[j, i];
                            char num = '０';
                            n += (int)num;
                            Console.Write((char)n);
                        }
                        else
                        {
                            Console.Write("〇");
                        }
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
                    tileOpen(playerPosX,playerPosY);
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

        //爆弾の生成
        public void CreateStage()
        {
            if (created != 0) return;
            int count = 0;
            while(count < bomCount)
            {
                Random rnd = new Random();
                int x = rnd.Next(0, stageSize);
                int y = rnd.Next(0, stageSize);
                if(stage[x,y] == 0 && x != playerPosX && y != playerPosY)
                {
                    stage[x, y] = 9;
                    count++;
                }
            }
            created++;
        }

        //接している爆弾の個数を調べる
        public int NumCheck(int _x, int _y)
        {
            if (stage[_x, _y] == 9) return 9;
            int n = 0;
            for(int i = -1; i <= 1; i++)
            {
                for(int j = -1; j <= 1; j++)
                {
                    if (BomCheck(_x + j, _y + i)) n++;
                }
            }
            return n;
        }

        /// <summary>
        /// 爆弾と接しているかどうか
        /// </summary>
        /// <param name="_x">プレイヤーカーソルのｘ座標</param>
        /// <param name="_y">プレイヤーカーソルのｙ座標</param>
        /// <return>true = 接している</returns>
        public bool BomCheck(int _x, int _y)
        {
            if (_x < 0 || _x >= stageSize || _y < 0 || _y >= stageSize) return false;//送られてきた座標データが盤面サイズ以上かどうか判断
            if (stage[_x, _y] == 9) return true;
            else return false;
        }

        /// <summary>
        /// 爆弾と接していなければタイルをオープン
        /// </summary>
        /// <param name="_x"></param>
        /// <param name="_y"></param>
        public void tileOpen(int _x, int _y)
        {

            if (_x < 0 || _x >= stageSize || _y < 0 || _y >= stageSize
                || stageFlag[_x, _y] != 0) return;

            stage[_x, _y] = NumCheck(_x, _y);

            stageFlag[_x, _y] = 1;

            if(stage[_x, _y] == 9)
            {
                GameEnd();
            }
            
            if (stage[_x,_y] == 0)
            {
                tileOpen(_x - 1, _y + 1);
                tileOpen(_x - 1, _y);
                tileOpen(_x - 1, _y - 1);
                tileOpen(_x + 1, _y + 1);
                tileOpen(_x + 1, _y);
                tileOpen(_x + 1, _y - 1);
                tileOpen(_x, _y - 1);
                tileOpen(_x, _y + 1);
            }
        }

        public void GameEnd()
        {
            System.Media.SystemSounds.Hand.Play();
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
