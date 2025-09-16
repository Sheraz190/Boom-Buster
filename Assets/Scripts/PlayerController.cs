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
    [SerializeField] private Rigidbody2D _playerRb;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject canvas;

    private Vector2 _originalScale;
    private bool _isGrounded = true;

    private bool _canMoveLeft;
    private bool _canMoveRight;

    private float _duration = 3.5f;
    private float _elapsedTime = 0.0f;
    private float _currentSpeed = 0;
    private float _maxSpeed = 20;
   
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
      //s  CheckKeyboardInputs();
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

    private void CheckInputs()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            IncreaseSpeed();
            SetWalkTrue();
            _playerRb.linearVelocity = new Vector2(-_currentSpeed, _playerRb.linearVelocity.y);
            transform.localScale = new Vector2(-_originalScale.x, _originalScale.y);
        }
       
       else if (Input.GetKey(KeyCode.RightArrow))
       {
            IncreaseSpeed();
            SetWalkTrue();
            _playerRb.linearVelocity = new Vector2(_currentSpeed, _playerRb.linearVelocity.y);
            transform.localScale = new Vector2(_originalScale.x, _originalScale.y);
       }

        else
        {
            _elapsedTime = 0;
            _currentSpeed = 0;
            _playerRb.linearVelocity = new Vector2(0, _playerRb.linearVelocity.y);
            BackToIdleState();
        }
    }

    private void IncreaseSpeed()
    {
        if (_elapsedTime < _duration)
        {
            _elapsedTime += Time.deltaTime;
            float t = _elapsedTime / _duration;
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
            _elapsedTime = 0;
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
        if (!_canMoveRight && !_canMoveLeft)
        {
            _playerRb.linearVelocity = new Vector2(0, _playerRb.linearVelocity.y);
            BackToIdleState();
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

    public void Jump()
    {
        if (_isGrounded )
        {
            _playerRb.AddForce(Vector2.up * 25, ForceMode2D.Impulse);
            StartCoroutine(DropDown());
            _isGrounded = false;
            SetJumpAnimation();
        }
    }

    public IEnumerator DropDown()
    {
        yield return new WaitForSeconds(0.2f);
        _playerRb.linearVelocity = new Vector2(_playerRb.linearVelocity.x, -10f);
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

        //if(collision.gameObject.CompareTag("Collider"))
        //{
        //    MoveDownToGround();
        //}
    }

    private void MoveDownToGround()
    {
        Debug.Log("Method move down Called");
        StartCoroutine(DropDown());
        StopMoving();
    }

    public void SetAttackAnimation()
    {
        animator.SetBool("Attack", true);
        StartCoroutine(StopAttackAnimation());
    }
        
    private IEnumerator StopAttackAnimation()
    {
        yield return new WaitForSeconds(0.55f);
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


