using UnityEngine;

public class CarController : MonoBehaviour
{
    public Rigidbody body;
    public float speed;
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
        body.linearVelocity = transform.forward * forwardInput * 5;
        body.angularVelocity = Vector3.up * horizontalInput;
    }
}
