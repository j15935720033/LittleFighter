using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deep : Role
{
    #region  �ݩ�
    [SerializeField]
    private float speedWalk;

    private Animator ani;
    private Rigidbody2D rig;
    #endregion

    #region �ƥ�:�{���J�f
    //����ƥ�:�}�l�ƥ�e����@���A�A���o���󵥵�
    private void Awake()
    {
        ani = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    //��s�ƥ�:�C������60���A60FPS Frame per second
    void Update()
    {

    }
    //�@��T�w50��
    private void FixedUpdate()
    {
        
    }
    protected override void Walk()
    {
        //�]�w�[�t�סArig.velocity.y-->rig���쥻y�b���[�t��
        rig.velocity=new Vector2(speedWalk,rig.velocity.y);
    }
    #endregion

}
