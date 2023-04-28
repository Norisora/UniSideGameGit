using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickBlock : MonoBehaviour
{
    public float length = 0.0f;     //�����������m����
    public bool isDelete = false;   //������ɍ폜���邩�ǂ����itrue�Ȃ痎����ɏ����j

    bool isFell = false;
    float fadeTime = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        //Rigidbody2d�̕����������~
        Rigidbody2D rbody = GetComponent<Rigidbody2D>();
        rbody.bodyType = RigidbodyType2D.Static;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            //�v���C���[�Ƃ̋����v��
            float d = Vector2.Distance(transform.position, player.transform.position);
            if (length >= d)
            {
                Rigidbody2D rbody = GetComponent<Rigidbody2D>();
                if (rbody.bodyType == RigidbodyType2D.Static) 
                {
                    //Rigidbody2D�̕����������J�n
                    rbody.bodyType = RigidbodyType2D.Dynamic;
                }
            }
        }
        if (isFell)     //isDelete��true�̂Ƃ�
        {
            //��������
            //�����l��ύX���ăt�F�[�h�A�E�g������
            fadeTime -= Time.deltaTime;
            Color col = GetComponent<SpriteRenderer>().color; //�J���[�����o��
            col.a = fadeTime;   //�����l��ύX
            GetComponent<SpriteRenderer>().color = col;
            if (fadeTime <= 0.0f)
            {
                //0�ȉ��i�����j�ɂȂ��������
                Destroy(gameObject);
            }
        }
    }

    //�ڐG�J�n
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDelete)
        {
            Debug.Log("�����t���O�I��");
            isFell = true;  //�����t���O�I��
        }
    }
}