using UnityEngine;

public class CarController : MonoBehaviour
{
    public Rigidbody body;
    public float baseSpeed = 20f;
    public Vector3 currentSpeed;
    public float rotationSpeed = 20f;
    public float acceleration;
    public float topSpeed = 0;
    public float horizontalInput;
    public float forwardInput;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        forwardInput = Input.GetAxisRaw("Vertical");
        
        body.angularVelocity += Vector3.up * horizontalInput * rotationSpeed * Time.deltaTime;
        body.linearVelocity += transform.forward * forwardInput * baseSpeed * Time.deltaTime;

        body.linearVelocity = transform.forward * (body.linearVelocity).magnitude;

        currentSpeed = body.linearVelocity;
        
    }
}
