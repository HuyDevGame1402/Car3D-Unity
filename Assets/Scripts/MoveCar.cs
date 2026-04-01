using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MoveCarSimple : MonoBehaviour
{
    public float acceleration = 25f;
    public float maxSpeed = 15f;
    public float brakeForce = 40f;
    public float turnSpeed = 120f;
    public float naturalDeceleration = 10f;

    private Rigidbody rb;
    private float currentSpeed;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.drag = 0;
        rb.angularDrag = 4f;
    }

    void FixedUpdate()
    {
        HandleMovement();
        HandleTurning();
        ApplyNaturalDeceleration();
    }

    void HandleMovement()
    {
        // W = tăng tốc
        if (Input.GetKey(KeyCode.W))
        {
            currentSpeed += acceleration * Time.fixedDeltaTime;
        }

        // S = phanh
        if (Input.GetKey(KeyCode.S))
        {
            currentSpeed -= brakeForce * Time.fixedDeltaTime;
        }

        // Giới hạn tốc độ
        currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);

        // Áp dụng vận tốc
        Vector3 move = transform.forward * currentSpeed;
        rb.velocity = new Vector3(move.x, rb.velocity.y, move.z);
    }

    void HandleTurning()
    {
        if (currentSpeed > 0.1f)
        {
            float turnInput = 0;

            if (Input.GetKey(KeyCode.A))
                turnInput = -1;

            if (Input.GetKey(KeyCode.D))
                turnInput = 1;

            float turn = turnInput * turnSpeed * Time.fixedDeltaTime;
            rb.MoveRotation(rb.rotation * Quaternion.Euler(0, turn, 0));
        }
    }

    void ApplyNaturalDeceleration()
    {
        // Nếu không nhấn W thì tự giảm tốc
        if (!Input.GetKey(KeyCode.W) && currentSpeed > 0)
        {
            currentSpeed -= naturalDeceleration * Time.fixedDeltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
        }
    }
}
