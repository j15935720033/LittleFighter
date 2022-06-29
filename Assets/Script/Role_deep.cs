using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Role_deep : Role
{
    #region  屬性

    [SerializeField]
    private float speedWalk = 500;
    [SerializeField]
    private float jumpForce = 250;


    private Animator animator;
    private Rigidbody2D rig2D;
    private Transform trans;
    private Collider2D coll2D;
    private bool clickWalk;
    private bool clickJump;
    #endregion

    #region 事件:程式入口
    //喚醒事件:開始事件前執行一次，，取得元件等等
    private void Awake()
    {

        animator = GetComponent<Animator>();
        rig2D = GetComponent<Rigidbody2D>();


        trans = GetComponent<Transform>();

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    //更新事件:每秒執行約60次，60FPS Frame per second
    void Update()
    {
        JumpKey();
        Walk();
    }
    //一秒固定50次，物理移動放這裡
    private void FixedUpdate()
    {
        Jump();
        // Walk();
    }




    protected override void WalkKey()
    {

    }
    protected override void Walk()
    {

        #region Input.GetAxis 
        float moveDir = Input.GetAxis("Horizontal");//取得-1~1
        //float moveDir = Input.GetAxisRaw("Horizontal");

        // Time.deltaTime:Make it move 10 meters per second instead of 10 meters per frame...

        //****************移動方式*******************//
        rig2D.velocity = new Vector2(moveDir * speedWalk * Time.deltaTime, rig2D.velocity.y);
        //rig2D.AddForce(new Vector2(moveDir* speedWalk * Time.deltaTime,0));
        //trans.Translate(new Vector3(moveDir * speedWalk * Time.deltaTime,0,0));
        //****************人物轉向*******************//
        if (moveDir > 0)
        {
            trans.localScale = new Vector2(1f, 1f);//向左改变图像朝向左
        }
        else if (moveDir < 0)
        {
            trans.localScale = new Vector2(-1f, 1f);//向左改变图像朝向左
        }


        #endregion






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
            rig2D.AddForce(new Vector2(0, jumpForce));
            clickJump = false;
        }
    }
    #endregion

}
