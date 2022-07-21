using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Role_deep : Role
{
    #region  屬性

    [SerializeField, Header("走路速度"),Tooltip("用velocivy控制")]//用velocivy控制
    private float speedWalk = 1500;
    [SerializeField, Header("跳躍力量")]
    private float jumpForce = 500;
    [SerializeField, Header("檢查地板尺寸")]
    private Vector3 v3CheckGroundSize = new Vector3(3.61f, 0.27f, 0);
    [SerializeField, Header("檢查地板位移")]
    private Vector3 v3CheckGroundOffset = new Vector3(0.02f, -1.7f, 0);
    [SerializeField, Header("檢查地板顏色")]
    private Color colorCheckGround = new Color(1, 0, 0.2f, 0.5f);
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
    private string parWalk = "Walk";
    private float pressRightTime;//按下右鍵時間
    private float releaseRightTime;//放開右鍵時間
    private float pressLeftTime;//按下左鍵時間
    private float releaseLeftTime;//放開左鍵時間
    private float pressUpTime;
    private float releaseUpTime;
    private float pressDownTime;
    private float releaseDownTime;
    private float pressInterval = 0.1f;//2點個按鍵區間秒數
    private float pSpeedWalk = 0.01f;//用position走路
    private float pSpeedRun = 0.05f;//用position跑步路
    private float pSpeedJump = 0.3f;
    //測試
    private int twoJump;
    #endregion

    #region 事件:程式入口
    //喚醒事件:開始事件前執行一次，物件步開啟也會執行，取得元件等等
    private void Awake()
    {

        animator = GetComponent<Animator>();
        rig2D = GetComponent<Rigidbody2D>();

        deep = GameObject.Find("deep");
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
        //Walk();
        Walk2();

    }
    //一秒固定50次，物理移動放這裡
    private void FixedUpdate()
    {
        JumpForce();

    }





    #endregion
    #region unity方法
    private void OnDrawGizmos()
    {
        //1.決定顏色
        Gizmos.color = colorCheckGround;

        //2.繪製圖示
        //trans.position 當前物件座標trans
        Gizmos.DrawCube(transform.position + v3CheckGroundOffset, v3CheckGroundSize);
        //Gizmos.DrawCube(trans.position + v3CheckGroundOffset, v3CheckGroundSize);
    }
    #endregion

    #region 自訂方法


    /// <summary>
    /// 用Input.GetAxisRaw
    /// </summary>
    protected override void Walk()
    {

        //****************水平*******************//
        float moveH = Input.GetAxis("Horizontal");//取得-1~1
        float moveHDir = Input.GetAxisRaw("Horizontal");//取得-1、0、1


        //****************垂直走*******************//
        float moveV = Input.GetAxis("Vertical");//取得-1、0、1
        float moveVDir = Input.GetAxisRaw("Vertical");//取得-1、0、1



        // Time.deltaTime:Make it move 10 meters per second instead of 10 meters per frame...
        //****************人物加速度*******************//
        //rig2D.AddForce(new Vector2(moveDir * speedWalk * Time.deltaTime, rig2D.velocity.y));
        //rig2D.velocity = new Vector2(moveDir * speedWalk * Time.deltaTime, rig2D.velocity.y);
        //***************人物加速度上下左右*******************//
        rig2D.velocity = new Vector2(moveH * speedWalk * Time.deltaTime, moveVDir * speedWalk * Time.deltaTime);

        //****************走路動畫*******************//
        //print($"velocity={rig2D.velocity.x}");
        if (Mathf.Abs(moveHDir) > 0)
        {
            animator.SetBool(parWalk, true);
        }
        else
        {
            animator.SetBool(parWalk, false);
        }

        //****************人物轉向*******************//
        if (moveHDir > 0)
        {
            //trans.localScale = new Vector2(1f, trans.localScale.y);//向左改变图像朝向左
            trans.rotation = new Quaternion(trans.rotation.x, 0, trans.rotation.z, trans.rotation.w);
        }
        else if (moveHDir < 0)
        {
            //trans.localScale = new Vector2(-1f, trans.localScale.y);//向左改变图像朝向左
            trans.rotation = new Quaternion(trans.rotation.x, 180, trans.rotation.z, trans.rotation.w);
        }
    }
    /// <summary>
    /// 用Input.GetKey控制
    /// </summary>
    protected override void Walk2()
    {
        //************************GetKey*************************************************//

        //按右鍵
        if (Input.GetKey(KeyCode.RightArrow))
        {
            pressRightTime = Time.time;
            //print($"pressRightTime:{pressRightTime}");
            if (pressRightTime - releaseRightTime <= pressInterval)//跑步
            {
                print("<Color=yellow>跑步</Color>");
                gameObject.transform.position += new Vector3(pSpeedRun, 0, 0);
                releaseRightTime = Time.time;//跑步狀態中要持續更新鬆鍵時間
            }
            else//走路
            {
                //print("<Color=yellow>走路</Color>");
                gameObject.transform.position += new Vector3(pSpeedWalk, 0, 0);
                animator.SetBool(parWalk, true);
            }
            //轉向
            gameObject.transform.rotation = new Quaternion(trans.rotation.x, 0, trans.rotation.z, trans.rotation.w);
        }

        //按左鍵
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            pressLeftTime = Time.time;
            if (pressLeftTime - releaseLeftTime <= pressInterval)//跑步
            {
                gameObject.transform.position += new Vector3(-pSpeedRun, 0, 0);
                releaseLeftTime = Time.time;
            }
            else//走路
            {
                gameObject.transform.position += new Vector3(-pSpeedWalk, 0, 0);
                animator.SetBool(parWalk, true);
            }
            gameObject.transform.rotation = new Quaternion(trans.rotation.x, 180, trans.rotation.z, trans.rotation.w);
        }

        //按上鍵
        if (Input.GetKey(KeyCode.UpArrow))
        {
            pressUpTime = Time.time;
            if (pressUpTime - releaseUpTime <= pressInterval)//跑步
            {
                gameObject.transform.position += new Vector3(0, pSpeedRun, 0);
                releaseUpTime = Time.time;
            }
            else//走路
            {
                gameObject.transform.position += new Vector3(0, pSpeedWalk, 0);
                animator.SetBool(parWalk, true);
            }
        }

        //按下鍵
        if (Input.GetKey(KeyCode.DownArrow))
        {
            pressDownTime = Time.time;
            if (pressDownTime - releaseDownTime <= pressInterval)//跑步
            {
                gameObject.transform.position += new Vector3(0, -pSpeedRun, 0);
                releaseDownTime = Time.time;
            }
            else//走路
            {
                gameObject.transform.position += new Vector3(0, -pSpeedWalk, 0);
                animator.SetBool(parWalk, true);
            }
        }
        //**************************GetKeyDown**************************************//
        /*if (Input.GetKeyDown(KeyCode.RightShift))
        {
            gameObject.transform.position += new Vector3(0, pSpeedJump, 0);
            releaseDownTime = Time.time;
            //print($"releaseRightTime:{releaseRightTime}");
        }*/
        //**************************GetKeyUp**************************************//
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            animator.SetBool(parWalk, false);
            releaseRightTime = Time.time;
            //print($"releaseRightTime:{releaseRightTime}");
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            animator.SetBool(parWalk, false);
            releaseLeftTime = Time.time;
            //print($"releaseRightTime:{releaseRightTime}");
        }
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            animator.SetBool(parWalk, false);
            releaseUpTime = Time.time;
            //print($"releaseRightTime:{releaseRightTime}");
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            animator.SetBool(parWalk, false);
            releaseDownTime = Time.time;
            //print($"releaseRightTime:{releaseRightTime}");
        }
      
    }



    private void CheckGround()//判斷是否在地板
    {
        //2D碰撞器=物理.覆蓋型區域(中心點,尺寸,角度,圖層)。transform.position+v3CheckGroundOffset:代表DrawCube位置
        Collider2D hit = Physics2D.OverlapBox(transform.position + v3CheckGroundOffset, v3CheckGroundSize, 0, layerGround);


        //isGround=hit //簡寫hit有東西，就是trues
        if (hit != null)
        {
            //print($"hit是否有撞到東西={hit.name}");
            isGround = true;//在地板
        }
        else
        {
            isGround = false;
        }
    }
    protected override void JumpKey()//如果玩家按下空白鍵就往上跳躍
    {

        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            print("跳躍");
            clickJump = true;
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
