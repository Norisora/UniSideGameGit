using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    public float moveX = 0.0f;      //X移動量
    public float moveY = 0.0f;      //Y移動量
    public float times = 0.0f;      //時間
    public float weight = 0.0f;     //停止時間
    public bool isMoveWhenOn = false;   //乗ったときに動くフラグ

    public bool isCanMove = true;   //動くフラグ
    float perDX;        //1フレームのX移動値
    float perDY;        //           Y移動値
    Vector3 defPos;     //初期値
    bool isReverse = false; //反転フラグ

    // Start is called before the first frame update
    void Start()
    {
        //初期位置
        defPos = transform.position;
        //１フレームの移動時間取得
        float timestep = Time.fixedDeltaTime;
        //１フレームのX移動値
        perDX = moveX / (1.0f / timestep * times);
        //１フレームのY移動値
        perDY = moveY / (1.0f / timestep * times);

        if (isMoveWhenOn)
        {
            //乗ったときに動くので最初は動かさない
            isCanMove = false;
        }
    }
    private void FixedUpdate()
    {
        if (isCanMove)
        {
            float x = transform.position.x;
            float y = transform.position.y;
            bool endX = false;
            bool endY = false;
            if (isReverse)
            {
                //逆方向移動中・・・
                //移動量がプラスで移動位置が初期位置より小さい
                //または、移動量がマイナスで移動位置が初期位置より大きい
                if ((perDX >= 0.0f && x <= defPos.x) || 
                    (perDX < 0.0f && x >= defPos.x))
                {
                    //移動量が＋で
                    endX = true;    //X方向の移動終了
                }
                if ((perDY >= 0.0f && y <= defPos.y) || 
                    (perDY < 0.0f && y >= defPos.y))
                {
                    //移動量が＋で
                    endY = true;    //Y方向の移動終了
                }
                //床を移動させる
                transform.Translate(new Vector3(-perDX, -perDY, defPos.z));
                ;
            }
            else
            {
                //正方向移動中・・・
                //移動量がプラスで位置が初期＋移動距離より大きい
                //または、移動量がマイナスで位置が初期＋移動距離より小さい
                if ((perDX >= 0.0f && x >= defPos.x + moveX) || 
                    (perDX < 0.0f && x <= defPos.x + moveX))
                {
                    //移動量が＋で
                    endX = true;    //X方向の移動終了
                }
                if ((perDY >= 0.0f && y >= defPos.y + moveY) || 
                    (perDY < 0.0f && y <= defPos.y + moveY))
                {
                    //移動量が＋で
                    endY = true;    //Y方向の移動終了
                }
                //床を移動させる
                Vector3 v = new Vector3(perDX, perDY, defPos.z);
                transform.Translate(v);
            }
            if (endX && endY)
            {
                //移動終了
                if (isReverse)
                {
                    //正方向移動に戻る前に初期位置に戻す。（位置がずれていくため）
                    transform.position = defPos;
                }
                isReverse = !isReverse; //フラグを反転させる
                isCanMove = false;      //移動フラグを下す
                if (isMoveWhenOn == false)
                {
                    //乗ったときに動くふらぐOFF
                    Invoke("Move", weight); //移動フラグを立てる遅延実行
                }
            }
        }
    }

    //移動フラグを立てる
    public void Move()
    {
        Debug.Log("動く");
        isCanMove = true;
    }

    //移動フラグを下す
    public void Stop()
    {
        Debug.Log("止まる");
        isCanMove = false;
    }

    //接触開始
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("プレイヤー乗った");
            //接触したのがプレイヤーなら移動の床の子にする
            collision.transform.SetParent(transform);
            if (isMoveWhenOn)
            {
                //乗ったときに動く　フラグON
                isCanMove = true;   //移動フラグを立てる
            }

        }
    }
    //接触終了
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //接触し、離れたのがプレイヤーなら移動床の子から外す
            collision.transform.SetParent(null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
