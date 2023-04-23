using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject mainImage;
    public Sprite gameOverSpr;
    public Sprite gameClearSpr;
    public GameObject panel;
    public GameObject restartButton;
    public GameObject nextButton;

    Image titleImage;       //�摜��\�����Ă���Image�R���|�[�l���g

    // +++ ���Ԑ����ǉ� +++
    public GameObject timeBar;
    // Start is called before the first frame update
    void Start()
    {
        //�摜���\���ɂ���
        Invoke("InactiveImage", 1.0f);
        //�{�^���i�p�l���j���\���ɂ���
        panel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.gameState == "gameclear")
        {
            //�Q�[���N���A
            //�܂�RESTART�{�^���𖳌�������i�{�^������u�A�N�e�B�u�Ɍ����邽�߁j
            Button bt = restartButton.GetComponent<Button>();
            bt.interactable = false;
            mainImage.SetActive(true);      //�摜��\������
            panel.SetActive(true);
            mainImage.GetComponent<Image>().sprite = gameClearSpr;  //�摜��ݒ肷��
            PlayerController.gameState = "gameend";
        }
        else if (PlayerController.gameState == "gameover")
        {
            //�Q�[���I�[�o�[
            //NEXT�{�^���𖳌�������
            Button bt = nextButton.GetComponent<Button>();
            bt.interactable = false;
            mainImage.SetActive(true);      //�摜��\������
            panel.SetActive(true);
            mainImage.GetComponent<Image>().sprite = gameOverSpr;
            PlayerController.gameState = "gameend";
        }
        else if (PlayerController.gameState == "playing")
        {
            //�Q�[����
        }
    }
    //�摜���\���ɂ���
    void InactiveImage()
    {
        mainImage.SetActive(false);
    }
}
