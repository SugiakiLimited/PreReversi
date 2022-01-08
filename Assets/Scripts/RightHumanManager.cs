using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RightHumanManager : MonoBehaviour
{
    //Image�R���|�[�l���g���i�[����ϐ�
    private Image m_Image;
    //�X�v���C�g�I�u�W�F�N�g���i�[����z��
    public Sprite[] m_Sprite;
    //�X�v���C�g���㉺������ݒ�
    RectTransform rect;
    private int counter = 0;
    private float move = 0.0f;
    //�v���C���[�̐F���擾����ݒ�
    GameController gameController;

    private int time = 0;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        
        //Image�R���|�[�l���g���擾���ĕϐ�m_Image�Ɋi�[
        m_Image = GetComponent<Image>();
        //GameManager����
        gameController = GameObject.Find("GameManager").GetComponent<GameController>();

    }

    void Update()
    {
        //�X�v���C�g���㉺������
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

        //�N���b�N���ꂽ�ꍇ
        if(Input.GetMouseButtonDown(0))
        {
            //player��BLACK��������
            if(gameController.player == GameController.COLOR.BLACK)
            {
                m_Image.sprite = m_Sprite[1];
                time = 100;

            }
            
        }
        
    }

}
