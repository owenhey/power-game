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
        // If drifting, just use linear velocity & slow down the car slightly
        if (isDrifting)
        {
            linearVelocity = linearVelocity * 0.99f;
        }
        else
        {
            linearVelocity = Vector3.Dot(transform.forward, linearVelocity) * transform.forward;
        }
        body.linearVelocity = Vector3.ClampMagnitude(linearVelocity, topSpeed);
        
        currentSpeed = body.linearVelocity;

        // Set angular velocity on this car
        // Determine how fast rotation speed should be based on current speed
        // rotationSpeed = 
        body.AddTorque(new Vector3(0, horizontalInput * rotationSpeed * Time.deltaTime, 0));
        body.angularVelocity = Vector3.ClampMagnitude(body.angularVelocity, topRotationSpeed);

        currentRotationSpeed = body.angularVelocity;
        // Debug.Log(body.angularVelocity);
    }
}
