using Mono.Cecil.Cil;
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
    public GameObject timeBar;      //���ԕ\��Image
    public GameObject timeText;     //����Text
    TimeController timeCnt;         //TimeController

    // +++ �X�R�A�ǉ� +++
    public GameObject scoreText;
    public static int totalScore;
    public int stageScore;

    // +++ �T�E���h�Đ��ǉ� +++
    public AudioClip meGameOver;    //�Q�[���I�[�o�[
    public AudioClip meGameClear;   //�Q�[���N���A

    // +++ �v���C���[���� +++
    public GameObject inputUI;      //����UI�p�l��

    // Start is called before the first frame update
    void Start()
    {
        //�摜���\���ɂ���
        Invoke("InactiveImage", 1.0f);
        //�{�^���i�p�l���j���\���ɂ���
        panel.SetActive(false);

        // +++ ���Ԑ����ǉ� +++
        //TimeController���擾
        timeCnt = GetComponent<TimeController>();
        if (timeCnt != null)
        {
            if (timeCnt.gameTime == 0.0f)
            {
                timeBar.SetActive(false);   //�������ԂȂ��Ȃ�B��
            }
        }
        // +++ �X�R�A�ǉ� +++
        UpdateScore();
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

            // +++ ���Ԑ����ǉ� +++
            if (timeCnt != null)
            {
                timeCnt.isTimeOver = true;  //���ԃJ�E���g��~
                // +++ �X�R�A�ǉ� +++
                //�����ɑ�����邱�Ƃŏ�����؂�̂Ă�
                int time = (int)timeCnt.displayTime;
                totalScore += time * 10;        // �c�莞�Ԃ��X�R�A�ɉ�����
            }

            // +++ �X�R�A�ǉ� +++
            totalScore += stageScore;
            stageScore = 0;
            UpdateScore();

            // +++ �T�E���h�Đ��ǉ� +++
            //�T�E���h�Đ�
            AudioSource soundPlayer = GetComponent<AudioSource>();
            if(soundPlayer != null)
            {
                //BGM��~
                soundPlayer.Stop();
                soundPlayer.PlayOneShot(meGameClear);
            }

            // +++ �v���C���[���� +++
            inputUI.SetActive(false);       //����UI���B��
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

            // +++ ���Ԑ����ǉ� +++
            if (timeCnt != null) 
            {
                timeCnt.isTimeOver = true;  //���ԃJ�E���g��~
            }

            // +++ �T�E���h�Đ��ǉ� +++
            //�T�E���h�Đ�
            AudioSource soundPlayer = GetComponent<AudioSource>();
            if(soundPlayer != null) 
            {
                //BGM��~
                soundPlayer.Stop();
                soundPlayer.PlayOneShot(meGameOver);
            }
        }
        else if (PlayerController.gameState == "playing")
        {
            //�Q�[����
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            //PlayerController���擾����
            PlayerController playerCnt = player.GetComponent<PlayerController>();
            // +++ ���Ԑ����ǉ� +++
            //�^�C�����X�V����
            if (timeCnt != null)
            {
                if (timeCnt.gameTime > 0.0f)
                {
                    //�����ɑ�����邱�Ƃŏ����_��؂�̂Ă�
                    int time = (int)timeCnt.displayTime;
                    //�^�C���X�V
                    timeText.GetComponent<Text>().text = time.ToString();
                    //�^�C���I�[�o�[
                    if (time == 0) 
                    {
                        playerCnt.GameOver();   //�Q�[���I�[�o�[�ɂ���
                    }
                }
            }

            // +++ �X�R�A�ǉ� +++
            if (playerCnt.score != 0)
            {
                stageScore += playerCnt.score;
                playerCnt.score = 0;
                UpdateScore();
            }
        }
    }
    //�摜���\���ɂ���
    void InactiveImage()
    {
        mainImage.SetActive(false);
    }
    // +++ �X�R�A�ǉ� +++
    void UpdateScore()
    {
        int score = stageScore + totalScore;
        scoreText.GetComponent<Text>().text = score.ToString();
    }

    // +++ �v���C���[���� +++
    //�W�����v
    public void Jump()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerController playerCnt = player.GetComponent <PlayerController>();
        playerCnt.Jump();
    }
}
