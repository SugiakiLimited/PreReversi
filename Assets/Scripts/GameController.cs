using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

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
    //���@�}�X
    [SerializeField] //�ϐ����C���X�y�N�^�[�ɕ\��
    GameObject magicObject = null; //magicObject�Ƃ������O��GameObject�^�ϐ���錾
    //�Ֆ�
    [SerializeField]
    GameObject boardDisplay = null;

    //enum���g���Đ����ɖ��O������
    public enum COLOR
    {
        EMPTY, //�� = 0
        BLACK, //���F = 1
        WHITE, //���F = 2
        MAGIC  //���@ = 3
    }

    const int WIDTH = 8;
    const int HEIGHT = 8;

    COLOR[,] board = new COLOR[WIDTH, HEIGHT]; //������8*8��COLOR�^2�����z��board��錾
    int mp_B = 0, mp_W = 0;

    public COLOR player = COLOR.BLACK;

    //���@����\������e�L�X�g
    private GameObject Text_B_Magic;
    private GameObject Text_W_Magic;

    //��
    public AudioClip sound1;
    AudioSource audioSource;

    //���@�̕\�����Ԃ̃f�B���C
    float delayTime = 1.0f;

    //���@������ʂ̕\��
    [SerializeField]
    GameObject magiceffectUI;
    //���@��\������e�L�X�g
    [SerializeField]
    Text magicSpellText= null;

    //�V�ѕ���ʂ̕\��
    [SerializeField]
    GameObject asobikataUI;
    //�{�^���̎��
    [SerializeField] Button asobikataButton = null;
    [SerializeField] Button returngameButton = null;
    public void Button(Button button)
    {
        if (button == asobikataButton)
        {
            asobikataUI.SetActive(true);  // �V�ѕ��p�l����\��
        }
        else if (button == returngameButton)
        {
            asobikataUI.SetActive(false);  // �V�ѕ��p�l�������
        }
    }

    //Result��ʂ̕\��
    [SerializeField]
    GameObject gamesetUI;
  
    // Start is called before the first frame update
    void Start()
    {
        this.Text_B_Magic = GameObject.Find("Text_B_Magic"); //���@���\���e�L�X�g
        this.Text_W_Magic = GameObject.Find("Text_W_Magic");

        audioSource = GetComponent<AudioSource>(); //���Q�b�g

        Initialize(); //�Ֆʂ̏����l��ݒ�        
    }

    //�Ֆʂ̏����l��ݒ�
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
        ShowBoard();              //�Ֆʂ�\��
        resultText.text = ""; //���ʕ\�����󗓂ɂ���
        mp_B = 0; //���@���̏�����
        mp_W = 0;
        this.Text_B_Magic.GetComponent<Text>().text = "�~" + mp_B;
        this.Text_W_Magic.GetComponent<Text>().text = "�~" + mp_W;
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

                //�l��EMPTY�Ȃ�piece�ɉ������̃C�x���g��ݒ�
                if (board[h, v] == COLOR.EMPTY || board[h, v] == COLOR.MAGIC)
                {
                    //���W���ꎟ�I�ɕێ�
                    int x = h;
                    int y = v;
                    //piece�ɃC�x���g��ݒ�
                    piece.GetComponent<Button>().onClick.AddListener(() => { PutStone(x + "," + y); });
                }

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
            case COLOR.MAGIC: //���@�̎�
                prefab = Instantiate(magicObject);
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
        //position���J���}�ŕ�����
        int h = int.Parse(position.Split(',')[0]);
        int v = int.Parse(position.Split(',')[1]);
        int mp_flag = 0;

        if (board[h, v] == COLOR.MAGIC)
        {
            mp_flag = 1;
        }

        //�Ђ�����Ԃ�
        ReverseAll(h, v);
        //��ʂ�\��
        ShowBoard();
        //�Ђ�����Ԃ��Ă���Α���̔ԁA��̐F��ύX
        if (board[h, v] == player)
        {
            //���@�}�X�ɋ��u���ꍇ��mp�̒ǉ�
            if (mp_flag == 1)
            {
                if (player == COLOR.BLACK)
                {
                    mp_B += 1;
                    this.Text_B_Magic.GetComponent<Text>().text = "�~" + mp_B;
                }
                else if (player == COLOR.WHITE)
                {
                    mp_W += 1;
                    this.Text_W_Magic.GetComponent<Text>().text = "�~" + mp_W;
                }
                
            }

            Debug.Log("����mp:" + mp_B);
            Debug.Log("����mp:" + mp_W);

            //��̐F�𑊎�̐F�ɕύX
            player = player == COLOR.BLACK ? COLOR.WHITE : COLOR.BLACK;
            //���肪�p�X������
            if (CheckPass())
            {
                //���肪�p�X�̏ꍇ�A��̐F�������̐F�ɕύX
                player = player == COLOR.BLACK ? COLOR.WHITE : COLOR.BLACK;

                //�������p�X������
                if(CheckPass())
                {
                    //�������p�X�������ꍇ�A���s�𔻒�
                    CheckGame();
                    //gemesetUI�Ƀ��b�Z�[�W�𑗐M����
                    gamesetUI.SetActive(true);
                }
            }
        }
        
    }

    //�S�����ɂЂ�����Ԃ�
    void ReverseAll(int h, int v)
    {
        Reverse(h, v, 1, 0);  //�E����
        Reverse(h, v, -1, 0); //������
        Reverse(h, v, 0, -1); //�����
        Reverse(h, v, 0, 1);  //������
        Reverse(h, v, 1, -1); //�E�����
        Reverse(h, v, -1, -1);//�������
        Reverse(h, v, 1, 1);  //�E������
        Reverse(h, v, -1, 1); //��������
    }

    //1�����ɂЂ�����Ԃ�
    void Reverse(int h, int v, int directionH, int directionV)
    {
        //�m�F������Wx,y��錾
        int x = h + directionH, y = v + directionV;

        //����ł��邩�m�F���ĂЂ�����Ԃ�
        while (x < WIDTH && x >= 0 && y < HEIGHT && y >= 0)
        {
            //�����̋�����ꍇ
            if(board[x, y] == player)
            {
                //�Ђ�����Ԃ�
                int x2 = h + directionH, y2 = v + directionV;
                int count = 0; //�J�E���g�p�̕ϐ�
                while (!(x2 == x && y2 == y))
                {
                    board[x2, y2] = player;
                    x2 += directionH;
                    y2 += directionV;
                    count++;
                }
                //1�ȏ�Ђ�����Ԃ����ꍇ
                if (count > 0)
                {
                    //���u��
                    board[h, v] = player;
                }
                break;
            }

            //�󗓂������ꍇ
            else if (board[x, y] == COLOR.EMPTY || board[x, y] == COLOR.MAGIC)
            {
                //����ł��Ȃ��̂ŏ������I����
                break;
            }

            //�m�F���W�����ɐi�߂�
            x += directionH;
            y += directionV;
        }
    }

    //�p�X�𔻒肷��
    bool CheckPass()
    {
        for (int v = 0; v < HEIGHT; v++)
        {
            for (int h = 0; h < WIDTH; h++)
            {
                //board[h, v]���󗓂̏ꍇ
                if (board[h, v] == COLOR.EMPTY || board[h, v] == COLOR.MAGIC)
                {
                    COLOR[,] boardTemp = new COLOR[WIDTH, HEIGHT]; //�Ֆʕۑ��p�̕ϐ���錾
                    Array.Copy(board, boardTemp, board.Length); //�Ֆʂ̏�Ԃ�ۑ��p�ϐ��ɕۑ����Ă���
                    ReverseAll(h, v); //���Wh,v�ɋ��u�����Ƃ��ĂЂ�����Ԃ��Ă݂�

                    //�Ђ�����Ԃ����board[h,v]�ɋ�u����Ă���
                    if (board[h, v] == player)
                    {
                        //�Ђ�����Ԃ����̂Ńp�X�ł͂Ȃ�
                        board = boardTemp; //�Ֆʂ����Ƃɖ߂�
                        return false;
                    }
                }
            }
        }
        //�P���Ђ�����Ԃ��Ȃ������ꍇ�p�X
        return true;
    }

    //���s��\������e�L�X�g
    [SerializeField]
    Text resultText = null;

    //���s�𔻒肷��
    void CheckGame()
    {
        int black = 0;
        int white = 0;

        //��̐��𐔂���
        for (int v = 0; v < HEIGHT; v++)
        {
            for (int h = 0; h < WIDTH; h++)
            {
                switch (board[h, v])
                {
                    case COLOR.BLACK:
                        black++; //�����J�E���g
                        break;
                    case COLOR.WHITE:
                        white++; //�����J�E���g
                        break;
                    default:
                        break;
                }
            }
        }

        if (black > white)
        {
            resultText.text = "��" + black + "�F��" + white + "�ō��̏����I";
        }
        else if (black < white)
        {
            resultText.text = "��" + black + "�F��" + white + "�Ŕ��̏����I";
        }
        else
        {
            resultText.text = "��" + black + "�F��" + white + "�ň��������I";
        }
    }

    public void LoadTitleScene()
    {
        SceneManager.LoadScene("TitleScene");
    }

    private void MagicAct(COLOR C)
    {
        int num = UnityEngine.Random.Range(0, 6);
        magiceffectUI.SetActive(true);   // ���@������ʂ�\��
        audioSource.PlayOneShot(sound1); // ���@���������P��Đ�

        switch (num)
        {
            case 0:
                Invoke("Bomb", delayTime);
                magicSpellText.text = "�w�{���x�𔭓��I";
                Debug.Log("�w�{���x���g�����I");
                break;

            case 1:
                Invoke("Llotheo", delayTime);
                magicSpellText.text = "�w���Z�I�x�𔭓��I";
                Debug.Log("�w���Z�I�x���g�����I");
                break;

            case 2:
                Invoke("Gravity", delayTime);
                magicSpellText.text = "�w�O���r�e�B�x�𔭓��I";
                Debug.Log("�w�O���r�e�B�x���g�����I");
                break;

            case 3:
                Invoke("Trimmer", delayTime);
                magicSpellText.text = "�w�g���}�[�x�𔭓��I";
                Debug.Log("�w�g���}�[�x���g�����I");
                break;

            case 4:
                Invoke("Beam", delayTime);
                magicSpellText.text = "�w�r�[���x�𔭓��I";
                Debug.Log("�w�r�[���x���g�����I");
                break;

            case 5:
                Invoke("Steal", delayTime);
                magicSpellText.text = "�w�X�e�B�[���x�𔭓��I";
                Debug.Log("�w�X�e�B�[���x���g�����I");
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

    public void Magic_B()�@//���̖��@�̊Ǘ�
    {
        if (player == COLOR.BLACK && mp_B > 0)
        {
            MagicAct(COLOR.BLACK);
            mp_B -= 1;
            this.Text_B_Magic.GetComponent<Text>().text = "�~" + mp_B;
        }
    }

    public void Magic_W()�@//���̖��@�̊Ǘ�
    {
        if (player == COLOR.WHITE && mp_W > 0)
        {
            MagicAct(COLOR.WHITE);
            mp_W -= 1;
            this.Text_W_Magic.GetComponent<Text>().text = "�~" + mp_W;
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

        //��̐��𐔂���
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
