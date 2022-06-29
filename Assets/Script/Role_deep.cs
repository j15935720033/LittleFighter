using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Role_deep : Role
{
    #region  �ݩ�
    [SerializeField]
    private float speedWalk = 350;
    [SerializeField]
    private float heightJump = 350;
    private Animator animator;
    private Rigidbody2D rigidbody2D;
    private Transform trans;

    private bool clickWalk;
    private bool clickJump;
    #endregion

    #region �ƥ�:�{���J�f
    //����ƥ�:�}�l�ƥ�e����@���A�A���o���󵥵�
    private void Awake()
    {

        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();


        trans = GetComponent<Transform>();
        print("constraints" + rigidbody2D.constraints);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    //��s�ƥ�:�C������60���A60FPS Frame per second
    void Update()
    {
        JumpKey();
        //WalkKey();
        Walk();
    }
    //�@��T�w50��
    private void FixedUpdate()
    {
        Jump();
       // Walk();
    }




    protected override void WalkKey()
    {
       // print("Horizontal"+Input.GetAxis("Horizontal"));
        if (Input.GetAxis("Horizontal") != 0)
        {
            clickWalk = true;
        }


    }
    protected override void Walk()
    {

        //rigidbody2D.velocity = new Vector2(speedWalk, rigidbody2D.velocity.y);//�]�w�[�t�סArig.velocity.y-->rig���쥻y�b���[�t��
        print($"clickWalk:{clickWalk}");
        print($"speedWalk:{speedWalk}");
       
            speedWalk = Input.GetAxis("Horizontal") * speedWalk;
            speedWalk *= Time.deltaTime;
            //rigidbody2D.AddForce(new Vector2(speedWalk, rigidbody2D.velocity.y));
        Vector3 v3_1 = new Vector3(speedWalk, rigidbody2D.velocity.y, 0);
        trans.Translate(v3_1 * Time.deltaTime);
        //rigidbody2D.velocity = new Vector2(speedWalk, rigidbody2D.velocity.y);





        /*
        speedWalk = Input.GetAxis("Horizontal")* speedWalk;
        // Make it move 10 meters per second instead of 10 meters per frame...
        speedWalk *= Time.deltaTime;
        //rigidbody2D.velocity = new Vector2(speedWalk, rigidbody2D.velocity.y);
        //�]�w�[�t�סArig.velocity.y-->rig���쥻y�b���[�t��
        //rigidbody2D.AddForce(new Vector2(speedWalk, rigidbody2D.velocity.y));
        trans.Translate(new Vector3(speedWalk, rigidbody2D.velocity.y,0));
        */


    }

    //�p�G���a���U�ť���N���W���D
    protected override void JumpKey()
    {
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            print("���D");
            clickJump = true;
        }
    }
    //clickJump=true�ɵ��V�W���O�q
    protected override void Jump()
    {
        if (clickJump)
        {
            rigidbody2D.AddForce(new Vector2(0, heightJump));
            clickJump = false;
        }
    }
    #endregion

}
