using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RightHumanManager : MonoBehaviour
{
    //Imageコンポーネントを格納する変数
    private Image m_Image;
    //スプライトオブジェクトを格納する配列
    public Sprite[] m_Sprite;
    //スプライトを上下させる設定
    RectTransform rect;
    private int counter = 0;
    private float move = 0.0f;
    //プレイヤーの色を取得する設定
    GameController gameController;

    private int time = 0;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        
        //Imageコンポーネントを取得して変数m_Imageに格納
        m_Image = GetComponent<Image>();
        //GameManagerから
        gameController = GameObject.Find("GameManager").GetComponent<GameController>();

    }

    void Update()
    {
        //スプライトを上下させる
        rect.localPosition += new Vector3(0, move, 0);
        counter++;

        move = 0.5f*Mathf.Sin(Mathf.Deg2Rad*counter);

        if(counter == 360)
        {
            counter = 0;
            
        }

        if(time > 0)
        {
            time -= 1;
        }

        if(time == 1)
        {
            m_Image.sprite = m_Sprite[0];
        }

        //クリックされた場合
        if(Input.GetMouseButtonDown(0))
        {
            //playerがBLACKだったら
            if(gameController.player == GameController.COLOR.BLACK)
            {
                m_Image.sprite = m_Sprite[1];
                time = 100;

            }
            
        }
        
    }

}
