using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Role_deep : Role
{
    #region  屬性
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

    #region 事件:程式入口
    //喚醒事件:開始事件前執行一次，，取得元件等等
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

    //更新事件:每秒執行約60次，60FPS Frame per second
    void Update()
    {
        JumpKey();
        //WalkKey();
        Walk();
    }
    //一秒固定50次
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

        //rigidbody2D.velocity = new Vector2(speedWalk, rigidbody2D.velocity.y);//設定加速度，rig.velocity.y-->rig中原本y軸的加速度
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
        //設定加速度，rig.velocity.y-->rig中原本y軸的加速度
        //rigidbody2D.AddForce(new Vector2(speedWalk, rigidbody2D.velocity.y));
        trans.Translate(new Vector3(speedWalk, rigidbody2D.velocity.y,0));
        */


    }

    //如果玩家按下空白鍵就往上跳躍
    protected override void JumpKey()
    {
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            print("跳躍");
            clickJump = true;
        }
    }
    //clickJump=true時給向上的力量
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
