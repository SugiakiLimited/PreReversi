using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameController : MonoBehaviour
{

    //黒い駒
    [SerializeField] //変数をインスペクターに表示
    GameObject blackObject = null; //blackObjectという名前のGameObject型変数を宣言
    //白い駒
    [SerializeField] //変数をインスペクターに表示
    GameObject whiteObject = null; //whiteObjectという名前のGameObject型変数を宣言
    //何もないマス
    [SerializeField] //変数をインスペクターに表示
    GameObject emptyObject = null; //emptyObjectという名前のGameObject型変数を宣言
    //魔法マス
    [SerializeField] //変数をインスペクターに表示
    GameObject magicObject = null; //magicObjectという名前のGameObject型変数を宣言
    //盤面
    [SerializeField]
    GameObject boardDisplay = null;

    //enumを使って数字に名前をつける
    public enum COLOR
    {
        EMPTY, //空欄 = 0
        BLACK, //黒色 = 1
        WHITE, //白色 = 2
        MAGIC  //魔法 = 3
    }

    const int WIDTH = 8;
    const int HEIGHT = 8;

    COLOR[,] board = new COLOR[WIDTH, HEIGHT]; //長さが8*8のCOLOR型2次元配列boardを宣言
    int mp_B = 0, mp_W = 0;

    public COLOR player = COLOR.BLACK;

    //魔法数を表示するテキスト
    private GameObject Text_B_Magic;
    private GameObject Text_W_Magic;

    //音
    public AudioClip sound1;
    AudioSource audioSource;

    //魔法の表示時間のディレイ
    float delayTime = 1.0f;

    //魔法発動画面の表示
    [SerializeField]
    GameObject magiceffectUI;
    //魔法を表示するテキスト
    [SerializeField]
    Text magicSpellText= null;

    //遊び方画面の表示
    [SerializeField]
    GameObject asobikataUI;
    //ボタンの種類
    [SerializeField] Button asobikataButton = null;
    [SerializeField] Button returngameButton = null;
    public void Button(Button button)
    {
        if (button == asobikataButton)
        {
            asobikataUI.SetActive(true);  // 遊び方パネルを表示
        }
        else if (button == returngameButton)
        {
            asobikataUI.SetActive(false);  // 遊び方パネルを閉じる
        }
    }

    //Result画面の表示
    [SerializeField]
    GameObject gamesetUI;
  
    // Start is called before the first frame update
    void Start()
    {
        this.Text_B_Magic = GameObject.Find("Text_B_Magic"); //魔法数表示テキスト
        this.Text_W_Magic = GameObject.Find("Text_W_Magic");

        audioSource = GetComponent<AudioSource>(); //音ゲット

        Initialize(); //盤面の初期値を設定        
    }

    //盤面の初期値を設定
    public void Initialize()
    {
        board = new COLOR[WIDTH, HEIGHT];
        board[3, 3] = COLOR.WHITE;
        board[3, 4] = COLOR.BLACK;
        board[4, 3] = COLOR.BLACK;
        board[4, 4] = COLOR.WHITE;
        board[1, 1] = COLOR.MAGIC;
        board[1, 6] = COLOR.MAGIC;
        board[6, 1] = COLOR.MAGIC;
        board[6, 6] = COLOR.MAGIC;
     
        player = COLOR.BLACK;
        ShowBoard();              //盤面を表示
        resultText.text = ""; //結果表示を空欄にする
        mp_B = 0; //魔法数の初期化
        mp_W = 0;
        this.Text_B_Magic.GetComponent<Text>().text = "×" + mp_B;
        this.Text_W_Magic.GetComponent<Text>().text = "×" + mp_W;
    }

    //盤面を表示する
    void ShowBoard()
    {
        //boardDisplayの全ての子オブジェクトを削除
        foreach (Transform child in boardDisplay.transform)
        {
            Destroy(child.gameObject);
        }

        for(int v = 0; v < 8; v++) // vertical(垂直方向)のv
        {
            for(int h = 0; h < 8; h++) //horizontal(水平方向)のh
            {
                //boardの色に合わせて適切なprefabを取得
                GameObject piece = GetPrefab(board[h, v]);

                //値がEMPTYならpieceに押下時のイベントを設定
                if (board[h, v] == COLOR.EMPTY || board[h, v] == COLOR.MAGIC)
                {
                    //座標を一次的に保持
                    int x = h;
                    int y = v;
                    //pieceにイベントを設定
                    piece.GetComponent<Button>().onClick.AddListener(() => { PutStone(x + "," + y); });
                }

                //取得したPrefabをboardDisplayの子オブジェクトにする
                piece.transform.SetParent(boardDisplay.transform);
            }
        }
    }

    //色によって適切なprefabを取得し返す
    GameObject GetPrefab(COLOR color)
    {
        GameObject prefab;
        switch (color)
        {
            case COLOR.EMPTY: //空欄の時
                prefab = Instantiate(emptyObject);
                break;
            case COLOR.MAGIC: //魔法の時
                prefab = Instantiate(magicObject);
                break;
            case COLOR.BLACK: //黒の時
                prefab = Instantiate(blackObject);
                break;
            case COLOR.WHITE: //白の時
                prefab = Instantiate(whiteObject);
                break;
            default:          //それ以外の時（ここに入ることは想定されてない時）
                prefab = null;
                break;
                
        }
        return prefab; //取得したPrefabを返す
    }

    //駒を置く
    public void PutStone(string position)
    {
        //positionをカンマで分ける
        int h = int.Parse(position.Split(',')[0]);
        int v = int.Parse(position.Split(',')[1]);
        int mp_flag = 0;

        if (board[h, v] == COLOR.MAGIC)
        {
            mp_flag = 1;
        }

        //ひっくり返す
        ReverseAll(h, v);
        //画面を表示
        ShowBoard();
        //ひっくり返していれば相手の番、駒の色を変更
        if (board[h, v] == player)
        {
            //魔法マスに駒を置く場合のmpの追加
            if (mp_flag == 1)
            {
                if (player == COLOR.BLACK)
                {
                    mp_B += 1;
                    this.Text_B_Magic.GetComponent<Text>().text = "×" + mp_B;
                }
                else if (player == COLOR.WHITE)
                {
                    mp_W += 1;
                    this.Text_W_Magic.GetComponent<Text>().text = "×" + mp_W;
                }
                
            }

            Debug.Log("黒のmp:" + mp_B);
            Debug.Log("白のmp:" + mp_W);

            //駒の色を相手の色に変更
            player = player == COLOR.BLACK ? COLOR.WHITE : COLOR.BLACK;
            //相手がパスか判定
            if (CheckPass())
            {
                //相手がパスの場合、駒の色を自分の色に変更
                player = player == COLOR.BLACK ? COLOR.WHITE : COLOR.BLACK;

                //自分もパスか判定
                if(CheckPass())
                {
                    //自分もパスだった場合、勝敗を判定
                    CheckGame();
                    //gemesetUIにメッセージを送信する
                    gamesetUI.SetActive(true);
                }
            }
        }
        
    }

    //全方向にひっくり返す
    void ReverseAll(int h, int v)
    {
        Reverse(h, v, 1, 0);  //右方向
        Reverse(h, v, -1, 0); //左方向
        Reverse(h, v, 0, -1); //上方向
        Reverse(h, v, 0, 1);  //下方向
        Reverse(h, v, 1, -1); //右上方向
        Reverse(h, v, -1, -1);//左上方向
        Reverse(h, v, 1, 1);  //右下方向
        Reverse(h, v, -1, 1); //左下方向
    }

    //1方向にひっくり返す
    void Reverse(int h, int v, int directionH, int directionV)
    {
        //確認する座標x,yを宣言
        int x = h + directionH, y = v + directionV;

        //挟んでいるか確認してひっくり返す
        while (x < WIDTH && x >= 0 && y < HEIGHT && y >= 0)
        {
            //自分の駒だった場合
            if(board[x, y] == player)
            {
                //ひっくり返す
                int x2 = h + directionH, y2 = v + directionV;
                int count = 0; //カウント用の変数
                while (!(x2 == x && y2 == y))
                {
                    board[x2, y2] = player;
                    x2 += directionH;
                    y2 += directionV;
                    count++;
                }
                //1つ以上ひっくり返した場合
                if (count > 0)
                {
                    //駒を置く
                    board[h, v] = player;
                }
                break;
            }

            //空欄だった場合
            else if (board[x, y] == COLOR.EMPTY || board[x, y] == COLOR.MAGIC)
            {
                //挟んでいないので処理を終える
                break;
            }

            //確認座標を次に進める
            x += directionH;
            y += directionV;
        }
    }

    //パスを判定する
    bool CheckPass()
    {
        for (int v = 0; v < HEIGHT; v++)
        {
            for (int h = 0; h < WIDTH; h++)
            {
                //board[h, v]が空欄の場合
                if (board[h, v] == COLOR.EMPTY || board[h, v] == COLOR.MAGIC)
                {
                    COLOR[,] boardTemp = new COLOR[WIDTH, HEIGHT]; //盤面保存用の変数を宣言
                    Array.Copy(board, boardTemp, board.Length); //盤面の状態を保存用変数に保存しておく
                    ReverseAll(h, v); //座標h,vに駒を置いたとしてひっくり返してみる

                    //ひっくり返せればboard[h,v]に駒が置かれている
                    if (board[h, v] == player)
                    {
                        //ひっくり返したのでパスではない
                        board = boardTemp; //盤面をもとに戻す
                        return false;
                    }
                }
            }
        }
        //１つもひっくり返せなかった場合パス
        return true;
    }

    //勝敗を表示するテキスト
    [SerializeField]
    Text resultText = null;

    //勝敗を判定する
    void CheckGame()
    {
        int black = 0;
        int white = 0;

        //駒の数を数える
        for (int v = 0; v < HEIGHT; v++)
        {
            for (int h = 0; h < WIDTH; h++)
            {
                switch (board[h, v])
                {
                    case COLOR.BLACK:
                        black++; //黒をカウント
                        break;
                    case COLOR.WHITE:
                        white++; //白をカウント
                        break;
                    default:
                        break;
                }
            }
        }

        if (black > white)
        {
            resultText.text = "黒" + black + "：白" + white + "で黒の勝ち！";
        }
        else if (black < white)
        {
            resultText.text = "黒" + black + "：白" + white + "で白の勝ち！";
        }
        else
        {
            resultText.text = "黒" + black + "：白" + white + "で引き分け！";
        }
    }

    public void LoadTitleScene()
    {
        SceneManager.LoadScene("TitleScene");
    }

    private void MagicAct(COLOR C)
    {
        int num = UnityEngine.Random.Range(0, 6);
        magiceffectUI.SetActive(true);   // 魔法発動画面を表示
        audioSource.PlayOneShot(sound1); // 魔法発動音を１回再生

        switch (num)
        {
            case 0:
                Invoke("Bomb", delayTime);
                magicSpellText.text = "『ボム』を発動！";
                Debug.Log("『ボム』を使った！");
                break;

            case 1:
                Invoke("Llotheo", delayTime);
                magicSpellText.text = "『ロセオ』を発動！";
                Debug.Log("『ロセオ』を使った！");
                break;

            case 2:
                Invoke("Gravity", delayTime);
                magicSpellText.text = "『グラビティ』を発動！";
                Debug.Log("『グラビティ』を使った！");
                break;

            case 3:
                Invoke("Trimmer", delayTime);
                magicSpellText.text = "『トリマー』を発動！";
                Debug.Log("『トリマー』を使った！");
                break;

            case 4:
                Invoke("Beam", delayTime);
                magicSpellText.text = "『ビーム』を発動！";
                Debug.Log("『ビーム』を使った！");
                break;

            case 5:
                Invoke("Steal", delayTime);
                magicSpellText.text = "『スティール』を発動！";
                Debug.Log("『スティール』を使った！");
                break;

            default:
                break;
        }

        Invoke("MagicUI_OFF", delayTime);
    }

    private void MagicUI_OFF()
    {
        magiceffectUI.SetActive(false);
    }

    public void Magic_B()　//黒の魔法の管理
    {
        if (player == COLOR.BLACK && mp_B > 0)
        {
            MagicAct(COLOR.BLACK);
            mp_B -= 1;
            this.Text_B_Magic.GetComponent<Text>().text = "×" + mp_B;
        }
    }

    public void Magic_W()　//白の魔法の管理
    {
        if (player == COLOR.WHITE && mp_W > 0)
        {
            MagicAct(COLOR.WHITE);
            mp_W -= 1;
            this.Text_W_Magic.GetComponent<Text>().text = "×" + mp_W;
        }
    }

    void Bomb()
    {
        for (int i = 0; i < 10; i++)
        {
            int h = UnityEngine.Random.Range(0, WIDTH);
            int v = UnityEngine.Random.Range(0, HEIGHT);
            if (board[h, v] != COLOR.MAGIC)
            {
                board[h, v] = COLOR.EMPTY;
            }
        }
        ShowBoard();
    }

    void Llotheo()
    {
        for (int v = 0; v < HEIGHT; v++)
        {
            for (int h = 0; h < WIDTH; h++)
            {
                if (board[h, v] == COLOR.BLACK)
                {
                    board[h, v] = COLOR.WHITE;
                }
                else if (board[h, v] == COLOR.WHITE)
                {
                    board[h, v] = COLOR.BLACK;
                }
            }
        }
        ShowBoard();
    }

    void Gravity()
    {
        int endFlag = 0;
        
        while (endFlag == 0)
        {
            endFlag = 1;
            for (int v = HEIGHT - 1; v > 0; v--)
            {
                for (int h = 0; h < WIDTH; h++)
                {
                    if (board[h, v] == COLOR.EMPTY && board[h, v - 1] != COLOR.MAGIC && board[h, v - 1] != COLOR.EMPTY)
                    {
                        board[h, v] = board[h, v - 1];
                        board[h, v - 1] = COLOR.EMPTY;
                        endFlag = 0;
                    }

                }
            }
        }
        ShowBoard();
    }

    void Trimmer()
    {
        board[0, 0] = COLOR.EMPTY;
        board[0, 7] = COLOR.EMPTY;
        board[7, 0] = COLOR.EMPTY;
        board[7, 7] = COLOR.EMPTY;
        ShowBoard();
    }

    void Beam()
    {
        int h = UnityEngine.Random.Range(2, WIDTH - 2);
        int v = UnityEngine.Random.Range(2, HEIGHT - 2);

        for (int i = 0; i < HEIGHT; i++)
        {
            board[h, i] = COLOR.EMPTY;
        }
        for (int i = 0; i < WIDTH; i++)
        {
            board[i, v] = COLOR.EMPTY;
        }
        ShowBoard();
    }

    void Steal()
    {
        COLOR C = player;
        COLOR AntiC = COLOR.EMPTY;
        if (C == COLOR.BLACK)
        {
            AntiC = COLOR.WHITE;
        }
        else if (C == COLOR.WHITE)
        {
            AntiC = COLOR.BLACK;
        }

        int check = 0;

        //駒の数を数える
        for (int v = 0; v < HEIGHT; v++)
        {
            for (int h = 0; h < WIDTH; h++)
            {
                if(board[h, v] == AntiC)
                {
                    check += 1;
                }
            }
        }
        int steal1 = UnityEngine.Random.Range(0, check/2);
        int steal2 = UnityEngine.Random.Range(check/2, check);

        check = 0;
        for (int v = 0; v < HEIGHT; v++)
        {
            for (int h = 0; h < WIDTH; h++)
            {
                if (board[h, v] == AntiC)
                {
                    if (check == steal1 || check == steal2)
                    {
                        board[h, v] = C;
                    }
                    check += 1;
                }
            }
        }
        ShowBoard();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
