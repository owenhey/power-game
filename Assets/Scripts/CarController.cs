using UnityEngine;

public class CarController : MonoBehaviour
{
    public Rigidbody body;
    public float baseSpeed = 20f;
    public float topSpeed = 200f;
    public Vector3 currentSpeed;
    public float rotationSpeed = 5f;
    public float topRotationSpeed = 8f;
    public Vector3 currentRotationSpeed;
    public float horizontalInput;
    public float forwardInput;
    public bool isDrifting;
    public Vector3 InputKey;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Get user input
        horizontalInput = Input.GetAxisRaw("Horizontal");
        forwardInput = Input.GetAxisRaw("Vertical");
        isDrifting = Input.GetKey(KeyCode.LeftShift);

        // Set linear velocity on this car
        Vector3 linearVelocity = body.linearVelocity + (transform.forward * forwardInput * baseSpeed * Time.deltaTime);
        linearVelocity = isDrifting ? linearVelocity : Vector3.Dot(transform.forward, linearVelocity) * transform.forward;
        body.linearVelocity = Vector3.ClampMagnitude(linearVelocity, topSpeed);
        
        currentSpeed = body.linearVelocity;

        // Set angular velocity on this car
        // Determine how fast rotation speed should be based on current speed
        // rotationSpeed = 
        Vector3 angularVelocity = body.angularVelocity + (Vector3.up * horizontalInput * rotationSpeed * Time.deltaTime);
        body.angularVelocity = Vector3.ClampMagnitude(angularVelocity, topRotationSpeed);

        currentRotationSpeed = body.angularVelocity;
        
        Debug.Log(body.angularVelocity);

        // InputKey = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
    }

    void FixedUpdate()
    {
        // body.AddForce(InputKey * 50);
        // body.linearVelocity = InputKey * 10;
        // body.MovePosition((Vector3) transform.position * InputKey * 10 * Time.deltaTime);

        // float Angle = Mathf.Atan2(InputKey.x, InputKey.z) * Mathf.Rad2Deg;
        // transform.rotation = Quaternion.Euler(0, Angle, 0);
    }
}
