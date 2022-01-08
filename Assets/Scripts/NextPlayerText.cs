using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextPlayerText : MonoBehaviour
{
    GameController gameController;
    Text text;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.Find("GameManager").GetComponent<GameController>();
        text = this.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        string colorText = "";
        switch (gameController.player)
        {
            case GameController.COLOR.BLACK:
                colorText = "��";
                break;
            case GameController.COLOR.WHITE:
                colorText = "��";
                break;
            default:
                break;
        }
        text.text = "���̔� : " + colorText; 
    }
}
