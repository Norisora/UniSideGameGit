using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //============�f�o�b�O�@�\=============
    public bool cantDie = false;
    //=====================================

    Rigidbody2D rbody;
    float axisH = 0.0f;
    public float speed = 3.0f;

    public float jump = 9.0f;       //�W�����v��
    public LayerMask groundLayer;   //���n�ł��郌�C���[
    bool goJump = false;            //�W�����v�J�n�t���O
    bool onGround = false;          //�n�ʂɗ����Ă���t���O

    //�A�j���[�V�����Ή�
    Animator animator;
    public string stopAnime = "PlayerStop";
    public string moveAnime = "PlayerMove";
    public string jumpAnime = "PlayerJump";
    public string goalAnime = "PlayerGoal";
    public string deadAnime = "PlayerOver";
    string nowAnime = "";
    string oldAnime = "";

    public static string gameState = "playing"; //�Q�[���̏��

    public int score = 0;

    //�^�b�`�X�N���[���Ή��ǉ�
    bool isMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        rbody = this.GetComponent<Rigidbody2D>();
        //Animator ���Ƃ��Ă���
        animator = GetComponent<Animator>();
        nowAnime = stopAnime;
        oldAnime = stopAnime;

        gameState = "playing";  //�Q�[�����ɂ���
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState != "playing")
        {
            return;
        }
        //�ړ�
        if(isMoving == false)
        {
            //�����ړ��̓��͂��`�F�b�N����
            axisH = Input.GetAxisRaw("Horizontal");
        }

        //�����̒���
        if (axisH > 0.0f)
        {
            //�E�ړ�
            Debug.Log("�E�ړ�");
            transform.localScale = new Vector2(1, 1);
        }
        else if (axisH < 0.0f) 
        {
            //���ړ�
            Debug.Log("���ړ�");
            transform.localScale = new Vector2(-1, 1);  //���E���]������
        }
        //�L�����N�^�[���W�����v������
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        if (gameState != "playing")
        { 
            return; 
        }
        //�n�㔻��
        onGround = Physics2D.Linecast(transform.position,
                                        transform.position - (transform.up * 0.1f),
                                        groundLayer);
        if (onGround || axisH != 0)
        {
            //���x���X�V����
            rbody.velocity = new Vector2(speed * axisH * 3.0f, rbody.velocity.y);
        }
        if (onGround && goJump)
        {
            //�n��ŃW�����v�L�[�������ꂽ
            //�W�����v������
            Debug.Log("�W�����v�I");
            Vector2 jumpPw = new Vector2(0, jump);          //�W�����v������x�N�g��������
            rbody.AddForce(jumpPw, ForceMode2D.Impulse);    //�u�ԓI�ȗ͂�������
            goJump = false; //�W�����v�t���O������
        }
        if (onGround)
        {
            //�n�ʂ̏�
            if (axisH == 0)
            {
                nowAnime = stopAnime;   //��~��
            }
            else 
            {
                nowAnime = moveAnime;   //�ړ�
            }
        }
        else
        {
            //��
            nowAnime = jumpAnime;
        }

        if (nowAnime != oldAnime)
        {
            oldAnime = nowAnime;
            animator.Play(nowAnime);    //�A�j���[�V�����Đ�
        }
    }
    //�W�����v
    public void Jump() 
    {
        goJump = true;
        Debug.Log("�W�����v�{�^���������ꂽ");
    }
    //�ڐG�J�n
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Goal")
        {
            Goal(); //�S�[��
        }
        else if (collision.gameObject.tag == "Dead")
        {
            GameOver(); //�Q�[���I�[�o�[
        }
        else if (collision.gameObject.tag == "ScoreItem")
        {
            //�X�R�A�A�C�e��
            //ItemData�𓾂�
            ItemData item = collision.gameObject.GetComponent<ItemData>();
            //�X�R�A�𓾂�
            score = item.value;

            //�A�C�e�����폜����
            Destroy(collision.gameObject);
        }
    }
    //�S�[��
    public void Goal()
    {
        animator.Play(goalAnime);

        gameState = "gameclear";
        GameStop(); //�Q�[����~
    }
    //�Q�[���I�[�o�[
    public void GameOver()
    {
        if (cantDie) return;
        animator.Play(deadAnime);

        gameState = "gameover";
        GameStop(); //�Q�[����~
        //====
        //�Q�[���I�[�o�[���o
        //====
        //�v���C���[�����������
        GetComponent<CapsuleCollider2D>().enabled = false;
        //�v���C���[��������ɂ͂ˏグ�鉉�o
        rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
    }
    //�Q�[����~
    void GameStop()
    {
        //Rigidbody2D������Ă���
        Rigidbody2D rbody = GetComponent<Rigidbody2D>();
        //���x��0�ɂ��ċ�����~
        rbody.velocity = new Vector2(0, 0);
    }

    //�^�b�`�X�N���[���Ή��ǉ�
    public void SetAxis(float h, float v)
    {
        axisH = h;
        if(axisH == 0)
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
            Debug.Log(isMoving);
        }
    }
}
