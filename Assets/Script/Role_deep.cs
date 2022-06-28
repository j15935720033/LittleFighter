using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deep : Role
{
    #region  屬性
    [SerializeField]
    private float speedWalk;

    private Animator ani;
    private Rigidbody2D rig;
    #endregion

    #region 事件:程式入口
    //喚醒事件:開始事件前執行一次，，取得元件等等
    private void Awake()
    {
        ani = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    //更新事件:每秒執行約60次，60FPS Frame per second
    void Update()
    {

    }
    //一秒固定50次
    private void FixedUpdate()
    {
        
    }
    protected override void Walk()
    {
        //設定加速度，rig.velocity.y-->rig中原本y軸的加速度
        rig.velocity=new Vector2(speedWalk,rig.velocity.y);
    }
    #endregion

}
