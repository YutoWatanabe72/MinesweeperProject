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
        //ステージの大きさの範囲
        const int minStageSize = 5;
        const int maxStageSize = 20;

        //入力情報
        string input = "";

        //ステージ情報
        int stageSize = 0;
        int bomCount = 0;

        //カーソルの座標
        int coursolPosX = 0;
        int coursolPosY = 0;

        //ステージの情報
        int[,] stageInfo = new int[0, 0];
        int[,] stageFlag = new int[0, 0];

        //ステージ生成したか判断
        int created = 0;

        //開いているタイルの数と開くのに必要な枚数
        int opendTile = 0;
        int needOpenTile = 0;

        //ゲームの状態
        public bool gameEnd = false;
        bool gameClear = false;


        /// <summary>
        /// 盤面サイズの設定
        /// </summary>
        public void StageSizeDecision()
        {
            Console.WriteLine("希望するステージサイズを入力してください。");

            while (stageSize == 0)
            {
                Console.WriteLine("入力可能範囲は「{0}～{1}」です。", minStageSize, maxStageSize);


                //数字の入力
                input = Console.ReadLine();
                int num;
                if (int.TryParse(input, out num))
                {
                    if (num >= minStageSize && num <= maxStageSize)
                    {
                        stageSize = num;
                        //bomCount = stageSize;

                        //stage配列
                        //左：横軸　右：縦軸
                        stageInfo = new int[stageSize, stageSize];
                        stageFlag = new int[stageSize, stageSize];
                        created = 0;
                        return;
                    }
                    else
                    {
                        Log("outofrange");
                    }
                }
                else
                {
                    Log("notnum");
                }
            }
        }

        /// <summary>
        /// 爆弾の個数の設定
        /// </summary>
        public void BomCountDecision()
        {
            Console.WriteLine("出現させる爆弾の個数を入力してください。");

            while (bomCount == 0)
            {
                Console.WriteLine("入力可能範囲は「{0}～{1}」です。", stageSize, stageSize * stageSize / 2);

                //数字の入力
                input = Console.ReadLine();
                int num;
                if (int.TryParse(input, out num))
                {
                    if (num >= stageSize && num <= stageSize * stageSize / 2)
                    {
                        bomCount = num;
                        needOpenTile = stageSize * stageSize - bomCount;
                        return;
                    }
                    else
                    {
                        Log("outofrange");
                    }
                }
                else
                {
                    Log("notnum");
                }
            }
        }

        /// <summary>
        /// 盤面の表示
        /// </summary>
        public void StageDisp()
        {
            for (int i = 0; i < stageSize; i++)
            {
                for (int j = 0; j < stageSize; j++)
                {
                    if (coursolPosX == j && coursolPosY == i)
                    {
                        Console.Write("◎");
                    }
                    else
                    {
                        switch (stageFlag[j, i])
                        {
                            case 0:
                                Console.Write("■");
                                break;
                            case 1:
                                int n = stageInfo[j, i];
                                if (n == 0)
                                {
                                    Console.Write("・");
                                }
                                else
                                {
                                    char num = '０';
                                    n += (int)num;
                                    Console.Write((char)n);
                                }
                                break;
                            case 2:
                                Console.Write("Ｆ");
                                break;
                            case 3:
                                Console.Write("Ｂ");
                                break;
                            case 4:
                                Console.Write("Ｑ");
                                break;
                            default:
                                break;
                        }
                    }
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// 入力されたキー情報のチェック
        /// </summary>
        public void KeyCheck()
        {
            ConsoleKeyInfo c = Console.ReadKey(true);
            switch (c.Key)
            {
                case ConsoleKey.UpArrow:
                    if (coursolPosY > 0 && coursolPosY <= stageSize - 1) PlayerMove(0, -1);
                    break;
                case ConsoleKey.DownArrow:
                    if (coursolPosY >= 0 && coursolPosY < stageSize - 1) PlayerMove(0, 1);
                    break;
                case ConsoleKey.LeftArrow:
                    if (coursolPosX > 0 && coursolPosX <= stageSize - 1) PlayerMove(-1, 0);
                    break;
                case ConsoleKey.RightArrow:
                    if (coursolPosX >= 0 && coursolPosX < stageSize - 1) PlayerMove(1, 0);
                    break;
                case ConsoleKey.F:
                    if (!OpendCheck()) stageFlag[coursolPosX, coursolPosY] = 2;
                    break;
                case ConsoleKey.B:
                    if (!OpendCheck()) stageFlag[coursolPosX, coursolPosY] = 3;
                    break;
                case ConsoleKey.Q:
                    if (!OpendCheck()) stageFlag[coursolPosX, coursolPosY] = 4;
                    break;
                case ConsoleKey.R:
                    stageFlag[coursolPosX, coursolPosY] = 0;
                    break;
                case ConsoleKey.Enter:
                    if (!OpendCheck()) CreateStage();
                    tileOpen(coursolPosX, coursolPosY);
                    break;
                default:
                    break;
            }

        }

        /// <summary>
        /// カーソルの移動
        /// </summary>
        /// <param name="_x"></param>
        /// <param name="_y"></param>
        public void PlayerMove(int _x, int _y)
        {
            coursolPosX += _x;
            coursolPosY += _y;
        }

        /// <summary>
        /// 爆弾の生成
        /// </summary>
        public void CreateStage()
        {
            if (created != 0) return;
            int count = 0;
            //生成する爆弾の個数に達するまでwhile文を回す
            while (count < bomCount)
            {
                Random rnd = new Random();
                int x = rnd.Next(0, stageSize);
                int y = rnd.Next(0, stageSize);
                if (stageInfo[x, y] == 0 && x != coursolPosX && y != coursolPosY)
                {
                    stageInfo[x, y] = 9;
                    count++;
                }
            }
            created++;
        }

        /// <summary>
        /// 接している爆弾の個数を調べる
        /// </summary>
        /// <param name="_x">調べるマスのx座標</param>
        /// <param name="_y">調べるマスのy座標</param>
        /// <returns></returns>
        public int NumCheck(int _x, int _y)
        {
            if (stageInfo[_x, _y] == 9) return 9;
            int n = 0;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (BomCheck(_x + j, _y + i)) n++;
                }
            }
            return n;
        }

        /// <summary>
        /// 爆弾と接しているかどうか
        /// </summary>
        /// <param name="_x">カーソルのｘ座標</param>
        /// <param name="_y">カーソルのｙ座標</param>
        /// <return>true = 接している</returns>
        public bool BomCheck(int _x, int _y)
        {
            if (_x < 0 || _x >= stageSize || _y < 0 || _y >= stageSize) return false;//送られてきた座標データが盤面サイズ以上かどうか判断
            if (stageInfo[_x, _y] == 9) return true;
            else return false;
        }

        /// <summary>
        /// タイルが開かれているかチェック
        /// </summary>
        /// <returns>true=開いている</returns>
        /// <returns>false=開いていない</returns>
        public bool OpendCheck()
        {
            if (stageFlag[coursolPosX, coursolPosY] == 1)return true;
            else return false;
        }

        /// <summary>
        /// 爆弾と接していなければタイルをオープン
        /// 接していればゲームオーバー
        /// </summary>
        /// <param name="_x"></param>
        /// <param name="_y"></param>
        public void tileOpen(int _x, int _y)
        {
            //画面範囲内にあるか
            //既に開かれているか
            if (_x < 0 || _x >= stageSize || _y < 0 || _y >= stageSize
                || stageFlag[_x, _y] == 1) return;

            //接している爆弾の個数を取得
            stageInfo[_x, _y] = NumCheck(_x, _y);

            //爆弾だったらゲームオーバー
            if (stageInfo[_x, _y] == 9)
            {
                GameEnd();
                return;
            }

            //開かれているマスの個数をカウントを増やす
            opendTile++;
            
            //規定数開かれているならゲームクリア
            if (opendTile == needOpenTile)
            {
                gameClear = true;
                GameEnd();
                return;
            }

            //マスを開く
            stageFlag[_x, _y] = 1;

            //開いたマスが爆弾に接していなかったら、隣のマスも開く
            if (stageInfo[_x, _y] == 0)
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
            gameEnd = true;
            Log("");

            //ゲームクリア音
            if (gameClear)System.Media.SystemSounds.Asterisk.Play();
            //ゲームオーバー音
            else System.Media.SystemSounds.Hand.Play();
            
            //全てのマスをオープン
            for (int i = 0; i < stageSize; i++)
            {
                for (int j = 0; j < stageSize; j++)
                {
                    stageInfo[j, i] = NumCheck(j, i);
                    int n = stageInfo[j, i];
                    if (n == 0)
                    {
                        Console.Write("・");
                    }
                    else if (n == 9)
                    {
                        Console.Write("※");
                    }
                    else
                    {
                        char num = '０';
                        n += (int)num;
                        Console.Write((char)n);
                    }
                }
                Console.WriteLine();
            }
        }


        /// <summary>
        /// ログの表記
        /// </summary>
        /// <param name="str">対応する文章</param>
        public void Log(string str)
        {
            Console.Clear();
            Console.WriteLine("「マインスイーパー」");
            Console.WriteLine();

            switch (str)
            {
                case "notnum":
                    Console.WriteLine("入力された文字は【{0}】を数字に変換できません。",input);
                    Console.WriteLine("もう一度入力しなおしてください。");
                    break;
                case "outofrange":
                    Console.WriteLine("入力された数字【{0}】は指定可能範囲ではありません。",input);
                    Console.WriteLine("もう一度入力しなおしてください。");
                    Console.WriteLine("ステージの大きさは【{0}×{0}】です。",stageSize);
                    break;
                default:
                    break;
            }
        }
    }

    class Disp
    {
        static void Main(string[] args)
        {

            var program = new Program();
            program.gameEnd = false;

            program.Log("");

            //盤面サイズの決定メソッド
            program.StageSizeDecision();
            program.BomCountDecision();


            do
            {
                program.Log("");
                program.StageDisp();
                program.KeyCheck();

            } while (!program.gameEnd);

        }
    }
}
