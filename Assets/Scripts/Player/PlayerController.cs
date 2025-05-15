using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float lookSpeed = 2f;

    private CharacterController _controller;
    private Transform _cam;

    private float _rotationX;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _cam = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 move = transform.right * h + transform.forward * v;
        _controller.Move(move * moveSpeed * Time.deltaTime);

        float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
        transform.Rotate(Vector3.up * mouseX);

        float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;
        _rotationX -= mouseY;
        _rotationX = Mathf.Clamp(_rotationX, -80, 80);
        _cam.localRotation = Quaternion.Euler(_rotationX, 0, 0);
    }
}
