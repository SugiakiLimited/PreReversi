using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    //������
    [SerializeField] //�ϐ����C���X�y�N�^�[�ɕ\��
    GameObject blackObject = null; //blackObject�Ƃ������O��GameObject�^�ϐ���錾
    //������
    [SerializeField] //�ϐ����C���X�y�N�^�[�ɕ\��
    GameObject whiteObject = null; //whiteObject�Ƃ������O��GameObject�^�ϐ���錾
    //�����Ȃ��}�X
    [SerializeField] //�ϐ����C���X�y�N�^�[�ɕ\��
    GameObject emptyObject = null; //emptyObject�Ƃ������O��GameObject�^�ϐ���錾
    //�Ֆ�
    [SerializeField]
    GameObject boardDisplay = null;

    //enum���g���Đ����ɖ��O������
    enum COLOR
    {
        EMPTY, //�� = 0
        BLACK, //���F = 1
        WHITE  //���F = 2
    }

    const int WIDTH = 8;
    const int HEIGHT = 8;

    COLOR[,] board = new COLOR[WIDTH, HEIGHT]; //������8*8��COLOR�^2�����z��board��錾

    // Start is called before the first frame update
    void Start()
    {
        Initialize(); //�Ֆʂ̏����l��ݒ�
        ShowBoard(); //�Ֆʂ�\��
        
    }

    //�Ֆʂ̏����l��ݒ�
    void Initialize()
    {
        board[3, 3] = COLOR.WHITE;
        board[3, 4] = COLOR.BLACK;
        board[4, 3] = COLOR.BLACK;
        board[4, 4] = COLOR.WHITE;
    }

    //�Ֆʂ�\������
    void ShowBoard()
    {
        //boardDisplay�̑S�Ă̎q�I�u�W�F�N�g���폜
        foreach (Transform child in boardDisplay.transform)
        {
            Destroy(child.gameObject);
        }

        for(int v = 0; v < 8; v++) // vertical(��������)��v
        {
            for(int h = 0; h < 8; h++) //horizontal(��������)��h
            {
                //board�̐F�ɍ��킹�ēK�؂�prefab���擾
                GameObject piece = GetPrefab(board[h, v]);

                //���W���ꎟ�I�ɕێ�
                int x = h;
                int y = v;
                //piece�ɃC�x���g��ݒ�
                piece.GetComponent<Button>().onClick.AddListener(() => { PutStone(x + "," + y); });

                //�擾����Prefab��boardDisplay�̎q�I�u�W�F�N�g�ɂ���
                piece.transform.SetParent(boardDisplay.transform);
            }
        }
    }

    //�F�ɂ���ēK�؂�prefab���擾���Ԃ�
    GameObject GetPrefab(COLOR color)
    {
        GameObject prefab;
        switch (color)
        {
            case COLOR.EMPTY: //�󗓂̎�
                prefab = Instantiate(emptyObject);
                break;
            case COLOR.BLACK: //���̎�
                prefab = Instantiate(blackObject);
                break;
            case COLOR.WHITE: //���̎�
                prefab = Instantiate(whiteObject);
                break;
            default:          //����ȊO�̎��i�����ɓ��邱�Ƃ͑z�肳��ĂȂ����j
                prefab = null;
                break;
                
        }
        return prefab; //�擾����Prefab��Ԃ�
    }

    //���u��
    public void PutStone(string position)
    {
        Debug.Log(position); //�R���\�[���ɍ��W��\��
        //position���J���}�ŕ�����
        int h = int.Parse(position.Split(',')[0]);
        int v = int.Parse(position.Split(',')[1]);
        //�N���b�N���ꂽ���W�ɋ��u��
        board[h, v]  =  COLOR.BLACK;
        //��ʂ�\��
        ShowBoard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
