using UnityEngine;

public class MovementController : MonoBehaviour
{
    public bool isGrounded;
    public bool canMove = true;
    [SerializeField] private float _walkingSpeed = 5f;
    [SerializeField] private float _runningSpeed = 10f;
    [SerializeField] private float _jumpHeight = 6.0f;
    [SerializeField] private float _gravity = 10.0f;
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Camera _playerCamera;
    private float _lookSpeed = 2.0f;
    private float _lookXLimit = 60.0f;
    private float _jumpTimer = 0.0f;
    private float _jumpInterval = 0.7f;
    private Vector3 _moveDirection = Vector3.zero;
    private float _rotationX = 0;
    private bool isPaused = false;

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        isGrounded = _characterController.isGrounded;
        // Calculate the Direction to Move based on the tranform of the Player
        Vector3 moveDirectionForward = transform.forward * Input.GetAxis("Vertical");
        Vector3 moveDirectionSide = transform.right * Input.GetAxis("Horizontal");

        // Normalize XZ-move vector
        Vector3 moveDirectionXZ;
        moveDirectionXZ = (moveDirectionForward + moveDirectionSide).normalized;
        //Calculate moveDirection
        _moveDirection.x = moveDirectionXZ.x * (isRunning ? _runningSpeed : _walkingSpeed);
        _moveDirection.z = moveDirectionXZ.z * (isRunning ? _runningSpeed : _walkingSpeed);
        //Calculate Y-move vector
        _jumpTimer += Time.deltaTime;
        if (Input.GetButton("Jump") && canMove && _characterController.isGrounded && (_jumpTimer > _jumpInterval))
        {
            _moveDirection.y = _jumpHeight;
            _jumpTimer = 0.0f;
        }
        if (_jumpTimer > _jumpInterval)
            _jumpTimer -= Time.deltaTime;
        // Apply gravity.
        if (!_characterController.isGrounded)
            _moveDirection.y -= _gravity * Time.deltaTime;
        // Move the controller
        if (canMove)
            _characterController.Move(_moveDirection * Time.deltaTime);
        // Player and Camera rotation
        if (canMove && !isPaused)
        {
            _rotationX += -Input.GetAxis("Mouse Y") * _lookSpeed;
            _rotationX = Mathf.Clamp(_rotationX, -_lookXLimit, _lookXLimit);
            _playerCamera.transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * _lookSpeed, 0);
        }
    }
    void OnApplicationFocus(bool hasFocus)
    {
        isPaused = !hasFocus;
    }
}
