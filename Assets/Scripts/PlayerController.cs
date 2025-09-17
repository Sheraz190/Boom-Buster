using UnityEngine;
using Unity;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    #region Variables
    public static PlayerController Instance;

    [SerializeField] private GameObject player;
    [SerializeField] private Rigidbody2D playerRb;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject canvas;

    private Vector2 _originalScale;
    private bool _isGrounded = true;

    private bool _canMoveLeft;
    private bool _canMoveRight;
    private float _maxSpeed = 20;
    private int _jumpForce = 20;
    private int _jumpCount;
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
        SetOriginalScale();
    }

    private void SetOriginalScale()
    {
        _originalScale = transform.localScale;
    }

    private void Update()
    {
        CheckKeyboardInputs();
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
        if (_isGrounded||CheckIfDoubleJump())
        {
            _jumpCount++;
            SetJumpAnimation();
            playerRb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
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
    }



    private void SetWalkTrue()
    {
        animator.SetBool("Idle", false);
        animator.SetBool("Walk", true);
    }

    private void BackToIdleState()
    {
        animator.SetBool("Idle", true);
        animator.SetBool("Walk", false);
        animator.SetBool("Running", false);
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
        animator.SetBool("Attack", true);
        StartCoroutine(StopAttackAnimation());
    }

    private IEnumerator StopAttackAnimation()
    {
        yield return new WaitForSeconds(0.3f);
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


}


