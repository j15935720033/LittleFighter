using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Role_deep : Role
{
    #region  屬性

    [SerializeField,Header("走路速度")]
    private float speedWalk = 1500;
    [SerializeField,Header("跳躍力量")]
    private float jumpForce = 200;
    [SerializeField, Header("檢查地板尺寸")]
    private Vector3 v3CheckGroundSize=new Vector3(3.61f, 0.27f,0);
    [SerializeField, Header("檢查地板位移")]
    private Vector3 v3CheckGroundOffset=new Vector3(0.02f, -1.7f, 0);
    [SerializeField, Header("檢查地板顏色")]
    private Color colorCheckGround = new Color(1,0,0.2f,0.5f);
    [SerializeField, Header("檢查地板圖層")]
    private LayerMask layerGround;


    private Animator animator;
    private Rigidbody2D rig2D;
    private Transform trans;
    private Collider2D coll2D;
    private GameObject deep;
    private bool clickJump;
    private bool isGround;//是否在地面上,預設是false
    private bool isWalk;//是否走路,預設是false
    private string nameWalk = "Walk";

    //測試
    private int twoJump;
    #endregion

    #region 事件:程式入口
    //喚醒事件:開始事件前執行一次，物件步開啟也會執行，取得元件等等
    private void Awake()
    {

        animator = GetComponent<Animator>();
        rig2D = GetComponent<Rigidbody2D>();
        
        deep=GameObject.Find("deep");
        //trans = deep.GetComponent<Transform>();
          trans = animator.GetComponent<Transform>();

        //print(LayerMask.NameToLayer("Ground"));
        layerGround.value = LayerMask.GetMask("Ground");//設定LayerMask
        
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    //更新事件:每秒執行約60次，60FPS Frame per second
    void Update()
    {
        CheckGround();
        JumpKey();
        Walk();
        //UpdateAnimatorWalk();

    }
    //一秒固定50次，物理移動放這裡
    private void FixedUpdate()
    {
        JumpForce();
        // Walk();
    }





    #endregion
    #region unity方法
    private void OnDrawGizmos()
    {
        //1.決定顏色
        Gizmos.color = colorCheckGround;

        //2.繪製圖示
        //trans.position 當前物件座標trans
        Gizmos.DrawCube(transform.position+v3CheckGroundOffset,v3CheckGroundSize);
        //Gizmos.DrawCube(trans.position + v3CheckGroundOffset, v3CheckGroundSize);
    }
    #endregion

    #region 自訂方法



    protected override void Walk()
    {



        float moveVertical = Input.GetAxisRaw("Vertical");//取得-1、0、1
        rig2D.velocity = new Vector2(rig2D.velocity.x,moveVertical * speedWalk * Time.deltaTime);
        
        //****************水平*******************//
        //float moveDir = Input.GetAxis("Horizontal");//取得-1~1
        float moveDir = Input.GetAxisRaw("Horizontal");//取得-1、0、1

        // Time.deltaTime:Make it move 10 meters per second instead of 10 meters per frame...
        //****************人物加速度*******************//
        //rig2D.AddForce(new Vector2(moveDir * speedWalk * Time.deltaTime, rig2D.velocity.y));
        rig2D.velocity = new Vector2(moveDir * speedWalk * Time.deltaTime, rig2D.velocity.y);
       
        //****************判斷人物加速度*******************//
        //print($"velocity={rig2D.velocity.x}");
        if (rig2D.velocity.x!=0)
        {
            isWalk = true;
            UpdateAnimatorWalk();
        }else
        {
            isWalk = false;
            UpdateAnimatorWalk();
        }
       
        //****************人物轉向*******************//
        if (moveDir > 0)
        {
            trans.localScale = new Vector2(1f, trans.localScale.y);//向左改变图像朝向左
        }
        else if (moveDir < 0)
        {
            trans.localScale = new Vector2(-1f, trans.localScale.y);//向左改变图像朝向左
        }


    }

    protected override void UpdateAnimatorWalk()//走路動畫
    {
            animator.SetBool(nameWalk,isWalk);
    }

    protected override void JumpKey()//如果玩家按下空白鍵就往上跳躍
    {

        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            print("跳躍");
            clickJump = true;
        }
    }
    private void CheckGround()//判斷是否在地板
    {
        //2D碰撞器=物理.覆蓋型區域(中心點,尺寸,角度,圖層)。transform.position+v3CheckGroundOffset:代表DrawCube位置
        Collider2D hit = Physics2D.OverlapBox(transform.position+v3CheckGroundOffset,v3CheckGroundSize,0, layerGround);
        
        
        //isGround=hit //簡寫hit有東西，就是trues
        if (hit!=null)
        {
            //print($"hit是否有撞到東西={hit.name}");
            isGround = true;//在地板
        }
        else
        {
            isGround = false;
        }
    }
    protected override void JumpForce()//案跳躍&&在地板時給向上的力量
    {
        if (clickJump&&isGround)
        {
            rig2D.AddForce(new Vector2(0, jumpForce));
            clickJump = false;
        }
    }


    //**********測試********************//
    void TwoJump()
    {
        if (isGround)//在地面上
        {
            twoJump = 2;
        }
        if (Input.GetKeyDown(KeyCode.RightShift) && twoJump > 0)//能跳躍次數
        {
            rig2D.velocity = Vector2.up * jumpForce;//Vector2.up 等於new Vector2(0,1)
            twoJump -= 1;
        }
        if (Input.GetKeyDown(KeyCode.RightShift) && twoJump == 0 && isGround)
        {
            rig2D.velocity = Vector2.up * jumpForce;//Vector2.up 等於new Vector2(0,1)

        }
    }
    #endregion
}
