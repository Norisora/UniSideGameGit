using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rbody;
    float axisH = 0.0f;
    public float speed = 3.0f;

    public float jump = 9.0f;       //ジャンプ力
    public LayerMask groundLayer;   //着地できるレイヤー
    bool goJump = false;            //ジャンプ開始フラグ
    bool onGround = false;          //地面に立っているフラグ

    // Start is called before the first frame update
    void Start()
    {
        rbody = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //水平方向の入力をチェックする
        axisH = Input.GetAxisRaw("Horizontal");
        //向きの調整
        if (axisH > 0.0f)
        {
            //右移動
            Debug.Log("右移動");
            transform.localScale = new Vector2(1, 1);
        }
        else if (axisH < 0.0f) 
        {
            //左移動
            Debug.Log("左移動");
            transform.localScale = new Vector2(-1, 1);  //左右反転させる
        }
        //キャラクターをジャンプさせる
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        //地上判定
        onGround = Physics2D.Linecast(transform.position,
                                        transform.position - (transform.up * 0.1f),
                                        groundLayer);
        if (onGround || axisH != 0)
        {
            //速度を更新する
            rbody.velocity = new Vector2(speed * axisH * 3.0f, rbody.velocity.y);
        }
        if (onGround && goJump)
        {
            //地上でジャンプキーが押された
            //ジャンプさせる
            Debug.Log("ジャンプ！");
            Vector2 jumpPw = new Vector2(0, jump);          //ジャンプさせるベクトルをつくる
            rbody.AddForce(jumpPw, ForceMode2D.Impulse);    //瞬間的な力を加える
            goJump = false; //ジャンプフラグを下す
        }
    }
    //ジャンプ
    public void Jump() 
    {
        goJump = true;
        Debug.Log("ジャンプボタンが押された");
    }
}
