using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamesetCanvasSet : MonoBehaviour
{
    //パネルを登録する
    public GameObject panel;

    //パーティクルを登録する
    public ParticleSystem particle;

    //テキストエリアを登録する
    public Text clearText;


    // Start is called before the first frame update
    void Start()
    {
        //パネルを隠す
        panel.SetActive(false);
    }

    //クリアパネルを表示させる
    //CheckGameからSendMessageで呼ばれる
    void OnEnter()
    {
        //パネルを表示させる
        panel.SetActive(true);

        //パーティクルを再生する
        particle.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
