using UnityEngine;
using Unity;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    #region Variables
    public static PlayerController Instance;

    [SerializeField] private GameObject player;
    [SerializeField] private Rigidbody2D playerRb;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject canvas;
    [SerializeField] private Transform spawnPos;
    [SerializeField] private GameObject _door;
    private Vector2 _originalScale;
    private bool _isVanish;
    private bool _isGrounded = true;
    private bool _canMoveLeft;
    private bool _canMoveRight;
    private float _maxSpeed = 20;
    private int _jumpForce = 20;
    public int _jumpCount;
    #endregion

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        playerRb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        UIManager.Instance.SetAllFalse();
        SetOriginalScale();
        StartCoroutine(StartPlayer());

    }

    private void SetOriginalScale()
    {
        _originalScale = transform.localScale;
    }

    private IEnumerator StartPlayer()
    {
        Color playerColor = gameObject.GetComponent<SpriteRenderer>().color;
        playerColor.a = 0.1f;
        gameObject.GetComponent<SpriteRenderer>().color = playerColor;
        Vector3 pos = gameObject.transform.position;
        while (playerColor.a <= 1)
        {
            yield return new WaitForSeconds(0.25f);
            playerColor.a += 0.1f;
            gameObject.GetComponent<SpriteRenderer>().color = playerColor;
            pos.x += 0.22f;
            gameObject.transform.position = pos;
        }
        _door.transform.position = new Vector3(_door.transform.position.x, _door.transform.position.y, 0);
        UIManager.Instance.SetAlllTrue();
    }

    private void Update()
    {
#if UNITY_EDITOR
        CheckKeyboardInputs();
#endif
        if (_canMoveLeft || _canMoveRight)
        {
            Movings();
        }
    }

    private void CheckKeyboardInputs()
    {
        if (Keyboard.current.leftArrowKey.isPressed)
        {
            _canMoveLeft = true;
            _canMoveRight = false;
        }
        else if (Keyboard.current.rightArrowKey.isPressed)
        {
            _canMoveRight = true;
            _canMoveLeft = false;

        }
        else
        {
            StopMoving();
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Jump();
        }
    }

    private void Movings()
    {
        if (_canMoveRight)
        {
            SetWalkTrue();
            playerRb.linearVelocity = new Vector2(_maxSpeed, playerRb.linearVelocity.y);
            transform.localScale = new Vector2(_originalScale.x, _originalScale.y);
        }
        else if (_canMoveLeft)
        {
            SetWalkTrue();
            playerRb.linearVelocity = new Vector2(-_maxSpeed, playerRb.linearVelocity.y);
            transform.localScale = new Vector2(-_originalScale.x, _originalScale.y);
        }
    }

    public void MovePlayerLeft()
    {
        _canMoveLeft = true;
    }

    public void MovePlayerRight()
    {
        _canMoveRight = true;
    }

    public void StopMoving()
    {
        _canMoveRight = false;
        _canMoveLeft = false;
        if (!_canMoveRight && !_canMoveLeft)
        {
            playerRb.linearVelocity = new Vector2(0, playerRb.linearVelocity.y);
            BackToIdleState();
        }
    }

    public void Jump()
    {
        if (_isGrounded || CheckIfDoubleJump())
        {
            _jumpCount++;
            SetJumpAnimation();
            playerRb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            SoundController.Instance.TurnOnJumpSound();
            StartCoroutine(DropDown());
            _isGrounded = false;
        }
    }

    public IEnumerator DropDown()
    {
        yield return new WaitForSeconds(0.5f);
        playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, -10f);
        StopJumpAnim();
    }

    private bool CheckIfDoubleJump()
    {
        if (_jumpCount < 2)
        {
            return true;
        }
        return false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGrounded = true;
            _jumpCount = 0;
        }

        if (collision.gameObject.CompareTag("FinishPoint"))
        {
            _canMoveRight = false;
            _canMoveLeft = false;
            UIManager.Instance.SetAllFalse();
            collision.gameObject.GetComponent<Collider2D>().enabled = false;
            StartCoroutine(FadePlayer());
        }
    }

    private IEnumerator FadePlayer()
    {

        Color playerColor = gameObject.GetComponent<SpriteRenderer>().color;
        gameObject.transform.DOMoveX(100, 1.1f);
        while (playerColor.a >= 0)
        {
            yield return new WaitForSeconds(0.1f);
            playerColor.a -= 0.1f;
            gameObject.GetComponent<SpriteRenderer>().color = playerColor;

        }
    }

    public void SetWalkTrue()
    {
        animator.SetBool("Idle", false);
        animator.SetBool("Walk", true);
        animator.SetBool("isWalking", true);
    }

    public void BackToIdleState()
    {
        animator.SetBool("Idle", true);
        animator.SetBool("Walk", false);
        animator.SetBool("Running", false);
        animator.SetBool("isWalking", false);
    }

    private void SetJumpAnimation()
    {
        animator.SetBool("Jumping", true);
    }

    private void StopJumpAnim()
    {
        animator.SetBool("Jumping", false);
    }
    public void SetAttackAnimation()
    {
        if (!_isVanish)
        {
            animator.SetBool("Attack", true);
        }
    }

    private void AddAttackSound()
    {
        SoundController.Instance.TurnOnAttackSound();
    }

    private void StopAttackAnimation()
    {
        animator.SetBool("Attack", false);
    }

    public void SetVanishAnimation()
    {
        animator.SetBool("Vanish", true);
        StartCoroutine(StopVanishAnimation());
    }

    private IEnumerator StopVanishAnimation()
    {
        yield return new WaitForSeconds(0.75f);
        animator.SetBool("Vanish", false);
        StartCoroutine(VanishPlayer());
    }

    public void SetDeathAnimation()
    {
        animator.SetBool("Death", true);
        StartCoroutine(StopDeathAnimation());
    }

    private IEnumerator StopDeathAnimation()
    {
        yield return new WaitForSeconds(2f);
        animator.SetBool("Death", false);
    }

    public void ThrowShruiken()
    {
        if (!_isVanish)
        {
            OnAttack();
        }
    }

    private void OnAttack()
    {
        GameObject obj = ObjectPooler.Instance.GetShruiken();
        obj.transform.position = spawnPos.position;
        Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        obj.SetActive(true);
        obj.GetComponent<ShruikenController>().LaunchShruiken(direction);
    }

    private IEnumerator VanishPlayer()
    {
        _isVanish = true;
        SpriteRenderer sr = player.GetComponent<SpriteRenderer>();
        Color c = sr.color;
        c.a = 0.3f;
        sr.color = c;
        yield return new WaitForSeconds(5.0f);
        c.a = 1;
        sr.color = c;
        _isVanish = false;
    }
}


