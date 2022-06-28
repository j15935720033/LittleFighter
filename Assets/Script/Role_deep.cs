using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Role_deep : Role
{
    #region  屬性
    [SerializeField]
    private float speedWalk=350;

    private Animator animator;
    private Rigidbody2D rigidbody2D;
    private Transform transform;
    #endregion

    #region 事件:程式入口
    //喚醒事件:開始事件前執行一次，，取得元件等等
    private void Awake()
    {
        
        animator= GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();


        transform = GetComponent<Transform>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    //更新事件:每秒執行約60次，60FPS Frame per second
    void Update()
    {
        //Walk();
        speedWalk = Input.GetAxis("Horizontal") * speedWalk;
        // Make it move 10 meters per second instead of 10 meters per frame...
        speedWalk *= Time.deltaTime;
        rigidbody2D.velocity = new Vector2(speedWalk, rigidbody2D.velocity.y);
        //rigidbody2D.AddForce(new Vector2(speedWalk, rigidbody2D.velocity.y));
    }
    //一秒固定50次
    private void FixedUpdate()
    {
        //Walk();
    }


    


    protected override void Walk()
    {
        speedWalk = Input.GetAxis("Horizontal")* speedWalk;
        // Make it move 10 meters per second instead of 10 meters per frame...
        speedWalk *= Time.deltaTime;
        //rigidbody2D.velocity = new Vector2(speedWalk, rigidbody2D.velocity.y);
        rigidbody2D.AddForce(new Vector2(speedWalk, rigidbody2D.velocity.y));
        //設定加速度，rig.velocity.y-->rig中原本y軸的加速度
        //rigidbody2D.velocity=new Vector2(speedWalk, rigidbody2D.velocity.y);
    }
    #endregion

}
