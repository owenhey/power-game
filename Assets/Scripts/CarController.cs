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
    public Vector3 InputKey;

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
