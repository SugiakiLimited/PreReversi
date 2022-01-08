using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonStart : MonoBehaviour
{
    [Header("�t�F�[�h")]public FadeImage fade;
    private bool firstPush = false;
    private bool goNextScene = false;

    [SerializeField]
    private GameObject loadAsobikata01; //�V�[���ψڒ��\���̉��
    [SerializeField]
    private GameObject loadAsobikata02; //�V�[���ψڒ��\���̉��

    private float step_time;   // �o�ߎ��ԃJ�E���g�p

    public void PressStart()
    {
        if (!firstPush)
        {
            fade.StartFadeOut();
            firstPush = true;
            StartCoroutine("ChangeImage1"); //�R���[�`���ݒ�
        }
    }
 

    // Start is called before the first frame update
    void Start()
    {
        step_time = 0.0f; // �o�ߎ��ԏ�����
    }

    IEnumerator ChangeImage1()
    {
        //1�b��~
        yield return new WaitForSeconds(1);
        //�@���[�h���UI���A�N�e�B�u�ɂ���
        loadAsobikata01.SetActive(true);
      
        //������̃R���[�`�������s����
        StartCoroutine("ChangeImage2");

    }

    IEnumerator ChangeImage2()
    {
        //2�b��~
        yield return new WaitForSeconds(2);

        //�@���[�h���UI���A�N�e�B�u�ɂ���
        loadAsobikata02.SetActive(true);
    }


    // Update is called once per frame
    void Update()
    {
        // �o�ߎ��Ԃ��J�E���g
        step_time += Time.deltaTime;

        // 10�b��ɉ�ʑJ�ځiscene2�ֈړ��j
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
