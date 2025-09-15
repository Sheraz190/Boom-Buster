using UnityEngine;
using Unity;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    #region Variables
    public static PlayerController Instance;
    [SerializeField] private GameObject player;
    [SerializeField] private Rigidbody2D _playerRb;
    [SerializeField] private Vector2 _originalScale;
    [SerializeField] private Animator animator;
    private bool _isJumping = false;
    private bool _isGrounded = true;
    private float duration = 3.5f;
    private float elapsedTime = 0.0f;
    private float _currentSpeed = 0;
    private float _maxSpeed = 250;
    private bool _canMoveLeft;
    private bool _canMoveRight;
    #endregion

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        _playerRb = GetComponent<Rigidbody2D>();
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
        if (_canMoveLeft || _canMoveRight)
        {
            Movings();
        }
    }

    private void IncreaseSpeed()
    {
        if (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            _currentSpeed = Mathf.Lerp(0, _maxSpeed, t);
        }
    }

    private void Movings()
    {
        if (_canMoveRight)
        {
            IncreaseSpeed();
            SetWalkTrue();
            _playerRb.linearVelocity = new Vector2(_currentSpeed, _playerRb.linearVelocity.y);
            transform.localScale = new Vector2(_originalScale.x, _originalScale.y);
        }
        else if (_canMoveLeft)
        {
            IncreaseSpeed();
            SetWalkTrue();
            _playerRb.linearVelocity = new Vector2(-_currentSpeed, _playerRb.linearVelocity.y);
            transform.localScale = new Vector2(-_originalScale.x, _originalScale.y);
        }
        else
        {
            elapsedTime = 0;
            _currentSpeed = 0;
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
        _playerRb.linearVelocity = Vector2.zero;
        BackToIdleState();
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

    public void Jump()
    {
        if (_isGrounded )
        {
            _playerRb.AddForce(Vector2.up * 500, ForceMode2D.Impulse);
            StartCoroutine_Auto(DropDown());
           // _playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            _isGrounded = false;
            SetJumpAnimation();
        }
    }

    public IEnumerator DropDown()
    {
        yield return new WaitForSeconds(0.2f);
        _playerRb.linearVelocity = new Vector2(_playerRb.linearVelocity.x, -50f);
    }

    private void SetJumpAnimation()
    {
        animator.SetBool("Jumping", true);
    }

    private void StopJumpAnim()
    {
        animator.SetBool("Jumping", false); 
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            _isGrounded = true;
            StopJumpAnim();
        }
    }
}

