using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReturnGameButtonCont : MonoBehaviour
{
    [SerializeField]
    GameObject asobikataUI;

    public void OnClick()
    {
        asobikataUI.SetActive(false);  // �V�ѕ��p�l�������;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
