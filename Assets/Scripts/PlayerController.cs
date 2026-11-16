using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private CharacterController _characterController;
    private InputAction _moveAction;
    private Vector2 _moveInput;
    private InputAction _runAction;
    private InputAction _jumpAction;

    private Animator _animator;

    [SerializeField] private float _movementSpeed = 3;
    [SerializeField] private float _runSpeed = 6;

    private float _turnSmoothvelocity;
    [SerializeField] float _smoothTime = 0.1f;

    private float _gravity;
    [SerializeField] private Vector3 _playerGravity;

    [SerializeField] private Transform _sensorTransform;
    [SerializeField] private float _sensorRadius = 0.2f;
    [SerializeField] private LayerMask _groundLayer;

    [SerializeField] private float _jumpHeight = 2;

    private Transform _cameraTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
        _moveAction = InputSystem.actions["Move"];
        _jumpAction = InputSystem.actions["Jump"];
        _runAction = InputSystem.actions["Sprint"];
    }

    void Start()
    {
        _gravity = Physics.gravity.y;
        _cameraTransform = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        _moveInput = _moveAction.ReadValue<Vector2>();
        Gravity();

        if (_jumpAction.WasPressedThisFrame() && IsGrounded())
        {
            Jump();
        }

        TPSMovement();
    }

    void TPSMovement()
    {
        Vector3 direction = new Vector3(_moveInput.x, 0, _moveInput.y);

        float speed = _movementSpeed;
        float animationValue = 0.5f;

        if (_runAction.IsPressed())
        {
            speed = _runSpeed;
            animationValue = 1f;
        }

        _animator.SetFloat("Horizontal", _moveInput.x * animationValue, 0.1f, Time.deltaTime);
        _animator.SetFloat("Vertical", _moveInput.y * animationValue, 0.1f, Time.deltaTime);
        _animator.SetBool("IsJumping", !IsGrounded());

        float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _cameraTransform.eulerAngles.y, ref _turnSmoothvelocity, _smoothTime);
        transform.rotation = Quaternion.Euler(0, smoothAngle, 0);

        if (direction != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _cameraTransform.eulerAngles.y;
            Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

            _characterController.Move(moveDirection * speed * Time.deltaTime);
        }
    }

    void Gravity()
    {
        if (!IsGrounded())
        {
            _playerGravity.y += _gravity * Time.deltaTime;
        }
        else if (IsGrounded() && _playerGravity.y < 0)
        {
            _playerGravity.y = _gravity;
        }

        _characterController.Move(_playerGravity * Time.deltaTime);
    }

    void Jump()
    {
        _playerGravity.y = Mathf.Sqrt(_jumpHeight * -2 * _gravity);
    }

    bool IsGrounded()
    {
        return Physics.CheckSphere(_sensorTransform.position, _sensorRadius, _groundLayer);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_sensorTransform.position, _sensorRadius);
    }
}