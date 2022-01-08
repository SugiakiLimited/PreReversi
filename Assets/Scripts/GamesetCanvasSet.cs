using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamesetCanvasSet : MonoBehaviour
{
    //�p�l����o�^����
    public GameObject panel;

    //�p�[�e�B�N����o�^����
    public ParticleSystem particle;

    //�e�L�X�g�G���A��o�^����
    public Text clearText;


    // Start is called before the first frame update
    void Start()
    {
        //�p�l�����B��
        panel.SetActive(false);
    }

    //�N���A�p�l����\��������
    //CheckGame����SendMessage�ŌĂ΂��
    void OnEnter()
    {
        //�p�l����\��������
        panel.SetActive(true);

        //�p�[�e�B�N�����Đ�����
        particle.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
