using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Role_deep : Role
{
    #region  �ݩ�
    [SerializeField]
    private float speedWalk;

    private Animator animator;
    private Rigidbody2D rigidbody2D;
    private Transform transform;
    #endregion

    #region �ƥ�:�{���J�f
    //����ƥ�:�}�l�ƥ�e����@���A�A���o���󵥵�
    private void Awake()
    {
        
        animator= GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();


        transform = animator.GetComponent<Transform>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    //��s�ƥ�:�C������60���A60FPS Frame per second
    void Update()
    {
        Walk();
    }
    //�@��T�w50��
    private void FixedUpdate()
    {
        
    }


    


    protected override void Walk()
    {
        speedWalk = Input.GetAxis("Horizontal")* speedWalk;
        // Make it move 10 meters per second instead of 10 meters per frame...
        speedWalk *= Time.deltaTime;
        //rigidbody2D.velocity = new Vector2(speedWalk, rigidbody2D.velocity.y);
        //rigidbody2D.AddForce(new Vector2(speedWalk, rigidbody2D.velocity.y));
        //�]�w�[�t�סArig.velocity.y-->rig���쥻y�b���[�t��
        rigidbody2D.velocity=new Vector2(speedWalk, rigidbody2D.velocity.y);
    }
    #endregion

}
