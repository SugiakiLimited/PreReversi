using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonStart : MonoBehaviour
{
    [Header("フェード")]public FadeImage fade;
    private bool firstPush = false;
    private bool goNextScene = false;

    [SerializeField]
    private GameObject loadAsobikata01; //シーン変移中表示の画面
    [SerializeField]
    private GameObject loadAsobikata02; //シーン変移中表示の画面

    private float step_time;   // 経過時間カウント用

    public void PressStart()
    {
        if (!firstPush)
        {
            fade.StartFadeOut();
            firstPush = true;
            StartCoroutine("ChangeImage1"); //コルーチン設定
        }
    }
 

    // Start is called before the first frame update
    void Start()
    {
        step_time = 0.0f; // 経過時間初期化
    }

    IEnumerator ChangeImage1()
    {
        //1秒停止
        yield return new WaitForSeconds(1);
        //　ロード画面UIをアクティブにする
        loadAsobikata01.SetActive(true);
      
        //もう一つのコルーチンを実行する
        StartCoroutine("ChangeImage2");

    }

    IEnumerator ChangeImage2()
    {
        //2秒停止
        yield return new WaitForSeconds(2);

        //　ロード画面UIをアクティブにする
        loadAsobikata02.SetActive(true);
    }


    // Update is called once per frame
    void Update()
    {
        // 経過時間をカウント
        step_time += Time.deltaTime;

        // 10秒後に画面遷移（scene2へ移動）
        if (step_time >= 11.5f)
        {
            if (!goNextScene && fade.IsFadeOutComplete())
            {
            SceneManager.LoadScene("SampleScene");
            goNextScene = true;
            }
        }

    }

}
