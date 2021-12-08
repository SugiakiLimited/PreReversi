using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    //盤面
    [SerializeField]
    GameObject boardDisplay = null;

    //enumを使って数字に名前をつける
    enum COLOR
    {
        EMPTY, //空欄 = 0
        BLACK, //黒色 = 1
        WHITE  //白色 = 2
    }

    const int WIDTH = 8;
    const int HEIGHT = 8;

    COLOR[,] board = new COLOR[WIDTH, HEIGHT]; //長さが8*8のCOLOR型2次元配列boardを宣言

    // Start is called before the first frame update
    void Start()
    {
        Initialize(); //盤面の初期値を設定
        ShowBoard(); //盤面を表示
        
    }

    //盤面の初期値を設定
    void Initialize()
    {
        board[3, 3] = COLOR.WHITE;
        board[3, 4] = COLOR.BLACK;
        board[4, 3] = COLOR.BLACK;
        board[4, 4] = COLOR.WHITE;
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

                //座標を一次的に保持
                int x = h;
                int y = v;
                //pieceにイベントを設定
                piece.GetComponent<Button>().onClick.AddListener(() => { PutStone(x + "," + y); });

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
        Debug.Log(position); //コンソールに座標を表示
        //positionをカンマで分ける
        int h = int.Parse(position.Split(',')[0]);
        int v = int.Parse(position.Split(',')[1]);
        //クリックされた座標に駒を置く
        board[h, v]  =  COLOR.BLACK;
        //画面を表示
        ShowBoard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
