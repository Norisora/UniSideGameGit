using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirtualPad : MonoBehaviour
{
    public float MaxLength = 70;        //�^�u�������ő勗��
    public bool is4DPad = false;        //�㉺���E�ɓ������t���O
    GameObject player;                  //���삷��v���C���[��GameObject
    Vector2 defPos;                     //�^�u�̏����ʒu
    Vector2 downPos;                    //�^�b�`�ʒu
    // Start is called before the first frame update
    void Start()
    {
        //�v���C���[�L�����N�^�[���擾
        player = GameObject.FindGameObjectWithTag("Player");
        //�^�u�̏������W
        defPos = GetComponent<RectTransform>().localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //�_�E���C�x���g
    public void PadDown()
    {
        //�}�E�X�|�C���g�̃X�N���[�����W
        downPos = Input.mousePosition;
    }
    //�h���b�O�C�x���g
    public void PadDrag()
    {
        //�}�E�X�|�C���g�̃X�N���[�����W
        Vector2 mousePosition = Input.mousePosition;
        //�V�����^�u�̈ʒu�����߂�
        Vector2 newTabPos = mousePosition - downPos;    //�}�E�X�_�E���ʒu����̈ړ�����
        if(is4DPad == false)
        {
            newTabPos.y = 0;    //���X�N���[���̏ꍇ��Y����0�ɂ���
        }
        //�ړ��x�N�g�����v�Z����
        Vector2 axis = newTabPos.normalized;
        //2�_�ԋ��������߂�
        float len = Vector2.Distance(defPos, newTabPos);
        if(len > MaxLength) 
        {
            //���E�����𒴂����̂Ō��E���W��ݒ肷��
            newTabPos.x = axis.x * MaxLength;
            newTabPos.y = axis.y * MaxLength;
        }
        //�^�u���ړ�������
        GetComponent<RectTransform>().localPosition = newTabPos;
        //�v���C���[�L�����N�^�[���ړ�������
        PlayerController plcnt = player.GetComponent<PlayerController>();
        plcnt.SetAxis(axis.x, axis.y);
        Debug.Log(axis.x + "+" + axis.y);
    }
    //�A�b�v�C�x���g
    public void PadUp()
    {
        //�^�u�̈ʒu�̏�����
        GetComponent<RectTransform>().localPosition = defPos;
        //�v���C���[�L�����N�^�[���~������
        PlayerController plcnt = player.GetComponent<PlayerController>();
        plcnt.SetAxis(0, 0);
    }
}