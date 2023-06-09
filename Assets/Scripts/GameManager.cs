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

    Image titleImage;       //画像を表示しているImageコンポーネント

    // +++ 時間制限追加 +++
    public GameObject timeBar;      //時間表示Image
    public GameObject timeText;     //時間Text
    TimeController timeCnt;         //TimeController

    // +++ スコア追加 +++
    public GameObject scoreText;
    public static int totalScore;
    public int stageScore;

    // +++ サウンド再生追加 +++
    public AudioClip meGameOver;    //ゲームオーバー
    public AudioClip meGameClear;   //ゲームクリア

    // +++ プレイヤー操作 +++
    public GameObject inputUI;      //操作UIパネル

    // Start is called before the first frame update
    void Start()
    {
        //画像を非表示にする
        Invoke("InactiveImage", 1.0f);
        //ボタン（パネル）を非表示にする
        panel.SetActive(false);

        // +++ 時間制限追加 +++
        //TimeControllerを取得
        timeCnt = GetComponent<TimeController>();
        if (timeCnt != null)
        {
            if (timeCnt.gameTime == 0.0f)
            {
                timeBar.SetActive(false);   //制限時間なしなら隠す
            }
        }
        // +++ スコア追加 +++
        UpdateScore();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.gameState == "gameclear")
        {
            //ゲームクリア
            //まずRESTARTボタンを無効化する（ボタンが一瞬アクティブに見えるため）
            Button bt = restartButton.GetComponent<Button>();
            bt.interactable = false;
            mainImage.SetActive(true);      //画像を表示する
            panel.SetActive(true);
            mainImage.GetComponent<Image>().sprite = gameClearSpr;  //画像を設定する
            PlayerController.gameState = "gameend";

            // +++ 時間制限追加 +++
            if (timeCnt != null)
            {
                timeCnt.isTimeOver = true;  //時間カウント停止
                // +++ スコア追加 +++
                //整数に代入することで少数を切り捨てる
                int time = (int)timeCnt.displayTime;
                totalScore += time * 10;        // 残り時間をスコアに加える
            }

            // +++ スコア追加 +++
            totalScore += stageScore;
            stageScore = 0;
            UpdateScore();

            // +++ サウンド再生追加 +++
            //サウンド再生
            AudioSource soundPlayer = GetComponent<AudioSource>();
            if(soundPlayer != null)
            {
                //BGM停止
                soundPlayer.Stop();
                soundPlayer.PlayOneShot(meGameClear);
            }

            // +++ プレイヤー操作 +++
            inputUI.SetActive(false);       //操作UIを隠す
        }
        else if (PlayerController.gameState == "gameover")
        {
            //ゲームオーバー
            //NEXTボタンを無効化する
            Button bt = nextButton.GetComponent<Button>();
            bt.interactable = false;
            mainImage.SetActive(true);      //画像を表示する
            panel.SetActive(true);
            mainImage.GetComponent<Image>().sprite = gameOverSpr;
            PlayerController.gameState = "gameend";

            // +++ 時間制限追加 +++
            if (timeCnt != null) 
            {
                timeCnt.isTimeOver = true;  //時間カウント停止
            }

            // +++ サウンド再生追加 +++
            //サウンド再生
            AudioSource soundPlayer = GetComponent<AudioSource>();
            if(soundPlayer != null) 
            {
                //BGM停止
                soundPlayer.Stop();
                soundPlayer.PlayOneShot(meGameOver);
            }
        }
        else if (PlayerController.gameState == "playing")
        {
            //ゲーム中
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            //PlayerControllerを取得する
            PlayerController playerCnt = player.GetComponent<PlayerController>();
            // +++ 時間制限追加 +++
            //タイムを更新する
            if (timeCnt != null)
            {
                if (timeCnt.gameTime > 0.0f)
                {
                    //整数に代入することで小数点を切り捨てる
                    int time = (int)timeCnt.displayTime;
                    //タイム更新
                    timeText.GetComponent<Text>().text = time.ToString();
                    //タイムオーバー
                    if (time == 0) 
                    {
                        playerCnt.GameOver();   //ゲームオーバーにする
                    }
                }
            }

            // +++ スコア追加 +++
            if (playerCnt.score != 0)
            {
                stageScore += playerCnt.score;
                playerCnt.score = 0;
                UpdateScore();
            }
        }
    }
    //画像を非表示にする
    void InactiveImage()
    {
        mainImage.SetActive(false);
    }
    // +++ スコア追加 +++
    void UpdateScore()
    {
        int score = stageScore + totalScore;
        scoreText.GetComponent<Text>().text = score.ToString();
    }

    // +++ プレイヤー操作 +++
    //ジャンプ
    public void Jump()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerController playerCnt = player.GetComponent <PlayerController>();
        playerCnt.Jump();
    }
}
