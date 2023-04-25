using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickBlock : MonoBehaviour
{
    public float length = 0.0f;     //自動落下検知距離
    public bool isDelete = false;   //落下後に削除するかどうか（trueなら落下後に消す）

    bool isFell = false;
    float fadeTime = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        //Rigidbody2dの物理挙動を停止
        Rigidbody2D rbody = GetComponent<Rigidbody2D>();
        rbody.bodyType = RigidbodyType2D.Static;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            //プレイヤーとの距離計測
            float d = Vector2.Distance(transform.position, player.transform.position);
            if (length >= d)
            {
                Rigidbody2D rbody = GetComponent<Rigidbody2D>();
                if (rbody.bodyType == RigidbodyType2D.Static) 
                {
                    //Rigidbody2Dの物理挙動を開始
                    rbody.bodyType = RigidbodyType2D.Dynamic;
                }
            }
        }
        if (isFell)     //isDeleteがtrueのとき
        {
            //落下した
            //透明値を変更してフェードアウトさせる
            fadeTime -= Time.deltaTime;
            Color col = GetComponent<SpriteRenderer>().color; //カラーを取り出す
            col.a = fadeTime;   //透明値を変更
            GetComponent<SpriteRenderer>().color = col;
            if (fadeTime <= 0.0f)
            {
                //0以下（透明）になったら消す
                Destroy(gameObject);
            }
        }
    }

    //接触開始
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDelete)
        {
            Debug.Log("落下フラグオン");
            isFell = true;  //落下フラグオン
        }
    }
}
