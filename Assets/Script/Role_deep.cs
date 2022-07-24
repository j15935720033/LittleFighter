using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Role_deep : Role
{
    #region  屬性

    [SerializeField, Header("走路速度"), Tooltip("用velocivy控制")]//用velocivy控制
    private float speedWalk = 1500;
    [SerializeField, Header("跳躍力量")]
    private float jumpForce = 250;
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
    private string parRun = "Run";
    private string parJump = "Jump";
    private bool canJump=true ;//是否能做跳躍。動畫:[true:不做跳躍動畫,false:做跳躍動畫]

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
    private float pSpeedJump = 1f;

    private float originalY;//紀錄跳起時，原本y的位置
    //測試
    private int twoJump;
    #endregion

    #region 事件:程式入口
    //喚醒事件:開始事件前執行一次，物件步開啟也會執行，取得元件等等
    private void Awake()
    {
        deep = GameObject.Find("deep");

        animator = deep.GetComponent<Animator>();
        rig2D = deep.GetComponent<Rigidbody2D>();
        trans = deep.GetComponent<Transform>();


        //print(LayerMask.NameToLayer("Ground"));
        layerGround.value = LayerMask.GetMask("Ground");//設定LayerMask
        //deep.layer=LayerMask.NameToLayer("Ground");//設定LayerMask
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
        //Jump();
        //Walk();
        Walk2();
        UpdateJumpAnimator();
    }
    //一秒固定50次，物理移動放這裡
    private void FixedUpdate()
    {
        JumpForce();

    }
    private void OnGUI()//called several times per frame
    {
        if (Event.current.rawType == EventType.KeyDown)
        {
            EventCallBack(Event.current);
            //Debug.Log(Event.current.keyCode);
        }
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
                animator.SetBool(parRun,true);//開啟跑步動畫
                releaseRightTime = Time.time;//跑步狀態中要持續更新鬆鍵時間
            }
            else//走路
            {
                //print("<Color=yellow>走路</Color>");
                gameObject.transform.position += new Vector3(pSpeedWalk, 0, 0);
                animator.SetBool(parWalk, true);
            }
            //向右轉向
            gameObject.transform.rotation = new Quaternion(trans.rotation.x, 0, trans.rotation.z, trans.rotation.w);
        }

        //按左鍵
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            pressLeftTime = Time.time;
            if (pressLeftTime - releaseLeftTime <= pressInterval)//跑步
            {
                gameObject.transform.position += new Vector3(-pSpeedRun, 0, 0);
                animator.SetBool(parRun, true);//開啟跑步動畫
                releaseLeftTime = Time.time;
            }
            else//走路
            {
                gameObject.transform.position += new Vector3(-pSpeedWalk, 0, 0);
                animator.SetBool(parWalk, true);
            }
            //向左轉向
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
            animator.SetBool(parWalk, false);//關閉走路動畫
            animator.SetBool(parRun, false);//關閉跑步動畫
            releaseRightTime = Time.time;
            //print($"releaseRightTime:{releaseRightTime}");
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            animator.SetBool(parWalk, false);
            animator.SetBool(parRun, false);
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
            //print($"不在地板hit是否有撞到東西={hit.name}");
            isGround = false;
        }
    }
    protected override void Jump()
    {
        if (Input.GetKeyDown(KeyCode.RightShift) && isGround )
        {
            print("跳躍");
            print(gameObject.transform.position);
            gameObject.transform.position += new Vector3(0, pSpeedJump, 0);
            //rig2D.gravityScale = 1;
        }
    }
    protected override void JumpKey()//如果玩家按下RightShift就往上跳躍
    {

        if (Input.GetKeyDown(KeyCode.RightShift)&& canJump)
        {
            print("跳躍");
            clickJump = true;
        }

        //print(temeV3);
        //print("transform" + transform.position);
        if (transform.position.y <= originalY)//落下時的位置，小於等於起跳點時，取消地心引力和y速度
        {
            //print("HI");
            rig2D.gravityScale = 0;
            rig2D.velocity = new Vector2(0, 0);//碰到撞時也會有反向作用力的速度，因為有重力讓y軸有加速度用，會繼續掉落，所以y軸速度要用0
            canJump = true;
        }

    }
    protected override void JumpForce()//案跳躍&&在地板時給向上的力量
    {
        if (clickJump&& canJump)//有按rightShift && 能跳
        {
            
            originalY = transform.position.y;//記錄跳起的位置
            //print(temeV3);
            rig2D.AddForce(new Vector2(0, jumpForce));
            rig2D.gravityScale = 1;//跳起後地心引力設1
            clickJump = false;
            canJump = false;
        }
    }
    private void UpdateJumpAnimator()
    {
        animator.SetBool(parJump, canJump);
    }
    
    
    //***********unity 组合键******************//
    private void EventCallBack(Event e)
    {
        //print(e.modifiers & EventModifiers.Control);//Control
        bool eventDown = (e.modifiers & EventModifiers.Control) != 0;

        if (!eventDown) return;

        e.Use();        //使用这个事件
       
        switch (e.keyCode)
        {
            case KeyCode.UpArrow:
                Debug.Log("按下组合键:ctrl+↑");
                break;
            case KeyCode.DownArrow:
                Debug.Log("按下组合键:ctrl+↓");
                break;
            case KeyCode.LeftArrow:
                Debug.Log("按下组合键:ctrl+←");
                break;
            case KeyCode.RightArrow:
                Debug.Log("按下组合键:ctrl+→");
                break;
        }
    }

    private void skill()
    {

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
