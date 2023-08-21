using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Animator jellyAnimator;

    [SerializeField] private LayerMask platformLayerMask;

    public BoxCollider2D groundChecker;

    #region GameEndVariable
    private bool isDead;
    private bool isGameEnd;
    [SerializeField] private float fdt;
    private float gameEndFdt = 2.5f;
    #endregion
   
    private Rigidbody2D rigid;

    #region PlayerMoveVariable
    private float horInput;
    private Vector3 playerScale;
    [SerializeField] private float playerSpeed;
    private float playerXScale;
    #endregion

    #region PlayerJumpVariable
    private bool isGround;
    private bool isItemGet;

    [SerializeField] private GameObject doubleJumpObject;
    [SerializeField] private Animator doubleJumpAnim;
    [SerializeField] private float jumpHeight;
    [SerializeField] private int maxJump;
    private float jumpForce;
    [SerializeField] private int jumpCount;
    #endregion

    #region MonoBehaviour Method

    void Start()
    {
        doubleJumpObject.SetActive(false);
        doubleJumpAnim.enabled = false;
        if (!jellyAnimator.enabled) jellyAnimator.enabled = true;
        jumpCount = 0;
        rigid = GetComponent<Rigidbody2D>();
        playerXScale = transform.localScale.x;
        isDead = false;
    }

    void Update()
    {        
        isGround = isGrounded();

        if (!isGround) fdt += Time.deltaTime;
        
        CheckGameOver();
      
        if (!isDead) PlayerAct();
    }

    private void FixedUpdate()
    {
        if (rigid != null)
        {
            if (rigid.velocity.y < -15)
            {
                rigid.velocity = new Vector3(rigid.velocity.x, -15,0f);
            }
        }
    }
    #endregion

    void PlayerAct()
    {   // check Player Act     
        PlayerJump();
        PlayerMove();
    }

    void PlayerJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJump && fdt < 0.1f)
        {
            PlayerJumpFx();
        }

        else if(Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJump && isItemGet)
        {
            PlayerJumpFx();
        }
    }

    void PlayerJumpFx()
    {
        doubleJumpObject.SetActive(false);
        doubleJumpAnim.enabled = false;
        isItemGet = false;
        ColorManager.instance.AutoSwitchMainColoring();
        rigid.velocity = Vector2.zero;
        jumpForce = Mathf.Sqrt(jumpHeight * -2 * (Physics2D.gravity.y * rigid.gravityScale));
        rigid.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
    }


    void PlayerMove()
    {
        horInput = Input.GetAxisRaw("Horizontal");
        
        playerScale = transform.localScale;
        if (horInput > 0)
        {
            playerScale.x = playerXScale;
        }
        else if(horInput < 0)
        {
            playerScale.x = -playerXScale;
        }
        transform.localScale = playerScale;
        transform.Translate(Vector2.right * Time.deltaTime * playerSpeed * horInput);
    }
    /*
    void OnCollisionExit2D(Collision2D collision)
    { // check to player Fall, use OnCollisionExit
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJump = true;
            jumpCount = maxJump;
        }
    }
    */

    void OnTriggerEnter2D(Collider2D other)
    {
        ColoredObject _co = other.gameObject.GetComponent<ColoredObject>();
        if (other.gameObject.CompareTag("Item"))
        {
            if (_co.isTriggerActive && !isGround)
            {
                //StopAllCoroutines();
                fdt = 0f;
                jumpCount = 0;

                StartCoroutine(ResetItem());
            }
            

            IEnumerator ResetItem()
            { // item regenerate after 3 sec
                doubleJumpObject.SetActive(true);
                doubleJumpAnim.enabled = true;
                isItemGet = true;
                other.gameObject.SetActive(false);
                yield return new WaitForSeconds(2);
                doubleJumpObject.SetActive(false);
                doubleJumpAnim.enabled = false;
                isItemGet = false;
                yield return new WaitForSeconds(1);
                _co.UpdateColoringLogic();
                other.gameObject.SetActive(true);
            }
        }

        if (other.gameObject.CompareTag("Spike"))
        {
            //ColoredObject _co = other.gameObject.GetComponent<ColoredObject>();
            if (_co.isTriggerActive)
            {
                if (isDead == false) StartCoroutine(DeathCoroutine());
            }
        }

    }

    bool isGrounded()
    {
        float extraHeightText = 0.2f;
        // RaycastHit2D rayCastHit = Physics2D.Raycast(groundChecker.bounds.center, Vector2.down, groundChecker.bounds.extents.y + extraHeightText, platformLayerMask);
        RaycastHit2D[] rayCastHit = Physics2D.BoxCastAll(groundChecker.bounds.center, groundChecker.bounds.size, 0f, Vector2.down, extraHeightText, platformLayerMask);
        
        bool value = false;


        for(int i = 0; i < rayCastHit.Length; i++)
        {
            if (rayCastHit[i].collider != null && !rayCastHit[i].collider.isTrigger)
            {
                //doubleJumpAnim.SetActive(false);
                isItemGet= false;
                fdt = 0f;
                jumpCount = 0;
                value = true;
            }           
        }

        if(!value && !isItemGet)
        {           
            jumpCount++;
        }

        value = false;
        /*
        Color rayColor;
        if(rayCastHit.collider != null)
        {
            rayColor = Color.green;
        } else
        {
            rayColor = Color.red;
        }

        Debug.DrawRay(groundChecker.bounds.center + new Vector3(groundChecker.bounds.extents.x, 0), Vector2.down * (groundChecker.bounds.extents.y + extraHeightText), rayColor);
        Debug.DrawRay(groundChecker.bounds.center - new Vector3(groundChecker.bounds.extents.x, 0), Vector2.down * (groundChecker.bounds.extents.y + extraHeightText), rayColor);
        Debug.DrawRay(groundChecker.bounds.center - new Vector3(groundChecker.bounds.extents.x, groundChecker.bounds.extents.y + extraHeightText), Vector2.right * (groundChecker.bounds.extents.x), rayColor);
        */
        return value;
    }

    void CheckGameOver()
    {
        if(fdt > gameEndFdt)
        {
            GameOver();
        }
    }

    public IEnumerator DeathCoroutine()
    {
        isDead = true;
        jellyAnimator.enabled = false;
        DeathEffect();
        yield return new WaitForSeconds(1f);
        GameOver();
    }

    void DeathEffect()
    {
        float _minPower = 3f;
        float _maxPower = 9f;
        float _torquePower = 9f;

        for (int i = 0; i < transform.childCount; i++)
        {
            Rigidbody2D _rb2d = transform.GetChild(i).AddComponent<Rigidbody2D>();

            float _angle = Random.Range(0f, 360f);
            Vector2 _dir = new Vector2(Mathf.Cos(Mathf.Deg2Rad * _angle), Mathf.Sin(Mathf.Deg2Rad * _angle));
            float _power = Random.Range(_minPower, _maxPower);
            _rb2d.AddForce(_dir * _power, ForceMode2D.Impulse);

            int _rnd = Random.Range(0, 2);
            _rb2d.AddTorque((_rnd * 2f - 1f) * _torquePower,ForceMode2D.Impulse);

            if (transform.GetChild(i).GetComponent<Collider2D>() != null) transform.GetChild(i).GetComponent<Collider2D>().enabled = false;
        }
        transform.DetachChildren();
    }

    void GameOver()
    {
        if (isGameEnd == true) return;

        isGameEnd = true;
        UIManager.instance._isGameEnd = true;
    }
}
