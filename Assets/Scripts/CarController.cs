using UnityEngine;

public class CarController : MonoBehaviour {
    public bool IsPlayerOne;
    
    public Rigidbody body;
    public ParticleSystem particles;
    public float baseSpeed = 20f;
    public float topSpeed = 20f;
    public Vector3 currentSpeed;

    public float rotationSpeed = 750f;
    public float topRotationSpeed = 2.25f;
    public Vector3 currentRotationSpeed;

    public float horizontalInput;
    public float forwardInput;
    public bool isDrifting;
    public int totalPowerCollected = 0;

    // Update is called once per frame
    void Update()
    {
        int powerCollected = 0;

        // Get user input
        GetInput();

        // Set linear velocity on this car
        Vector3 linearVelocity = body.linearVelocity + (transform.forward * forwardInput * baseSpeed * Time.deltaTime);
        // If drifting, just use linear velocity & slow down the car slightly
        if (isDrifting)
        {
            linearVelocity = linearVelocity * 0.99f;
            powerCollected += Mathf.FloorToInt(linearVelocity.magnitude / 3);
        }
        else
        {
            linearVelocity = Vector3.Dot(transform.forward, linearVelocity) * transform.forward;
            powerCollected += Mathf.FloorToInt(linearVelocity.magnitude / 10);
        }
        body.linearVelocity = Vector3.ClampMagnitude(linearVelocity, topSpeed);

        currentSpeed = body.linearVelocity;

        // Set angular velocity on this car
        // Determine how fast rotation speed should be based on current speed
        // rotationSpeed = 
        body.AddTorque(new Vector3(0, horizontalInput * rotationSpeed * Time.deltaTime, 0));
        body.angularVelocity = Vector3.ClampMagnitude(body.angularVelocity, topRotationSpeed);

        currentRotationSpeed = body.angularVelocity;

        // Add speed to kilowatts
        if (GameManager.Instance != null)
        {
            GameManager.Instance.Kilowatts += powerCollected;
            Debug.Log("Kilowatts: " + GameManager.Instance.Kilowatts);
        }

        totalPowerCollected += powerCollected;

        // particles.Emit(powerCollected);
    }

    private void GetInput() {
        if (IsPlayerOne) {
            horizontalInput = 0;
            if (Input.GetKey(KeyCode.A)) {
                horizontalInput += -1;
            }
            if (Input.GetKey(KeyCode.D)) {
                horizontalInput += 1;
            }
            
            forwardInput = 0;
            if (Input.GetKey(KeyCode.S)) {
                forwardInput += -1;
            }
            if (Input.GetKey(KeyCode.W)) {
                forwardInput += 1;
            }

            if (forwardInput < 0) {
                horizontalInput *= -1;
            }

            isDrifting = false;
            if (Input.GetKey(KeyCode.LeftShift)) {
                isDrifting = true;
            }
        }
        else {
            horizontalInput = 0;
            if (Input.GetKey(KeyCode.LeftArrow)) {
                horizontalInput += -1;
            }
            if (Input.GetKey(KeyCode.RightArrow)) {
                horizontalInput += 1;
            }
            
            forwardInput = 0;
            if (Input.GetKey(KeyCode.DownArrow)) {
                forwardInput += -1;
            }
            if (Input.GetKey(KeyCode.UpArrow)) {
                forwardInput += 1;
            }

            isDrifting = false;
            if (Input.GetKey(KeyCode.Space)) {
                isDrifting = true;
            }
        }
    }
}
