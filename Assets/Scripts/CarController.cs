using DG.Tweening;
using UnityEngine;

public class CarController : MonoBehaviour
{
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

    [Header("Drifting")]
    public float minSpeedToDrift = 5;
    public float driftTurnSpeedFactor = .6f;
    public float driftMinSpeedFactor = .5f;

    private float timeStartDrift;
    private float driftSpeedFactor = 1.0f;
    private Vector3 startDriftVelocity;
    private Vector3 startDriftRotation;
    private Tween driftSpeedFactorTween;
    
    [SerializeField] private AudioSource driftSource;
    [SerializeField] private AudioSource shootOffSource;

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.IsPlaying == false) return;
        
        int powerCollected = 0;

        // Get user input
        GetInput();

        // Set linear velocity on this car
        float turnFactor = isDrifting ? driftTurnSpeedFactor : 1.0f;
        Vector3 linearVelocity = body.linearVelocity + (transform.forward * (turnFactor * forwardInput * baseSpeed * Time.deltaTime));
        // If drifting, just use linear velocity & slow down the car slightly
        if (isDrifting)
        {
            linearVelocity = startDriftVelocity * driftSpeedFactor;
            powerCollected += Mathf.FloorToInt(linearVelocity.magnitude / 15);

            Debug.Log(linearVelocity);
        }
        else
        {
            linearVelocity = Vector3.Dot(transform.forward, linearVelocity) * transform.forward;
            powerCollected += Mathf.FloorToInt(linearVelocity.magnitude / 18);

            Debug.Log(powerCollected);
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
        }

        totalPowerCollected += powerCollected;

        // particles.Emit(powerCollected);
    }

    private void GetInput()
    {
        if (IsPlayerOne)
        {
            horizontalInput = 0;
            if (Input.GetKey(KeyCode.A))
            {
                horizontalInput += -1;
            }
            if (Input.GetKey(KeyCode.D))
            {
                horizontalInput += 1;
            }

            forwardInput = 0;
            if (Input.GetKey(KeyCode.S))
            {
                forwardInput += -1;
            }
            if (Input.GetKey(KeyCode.W))
            {
                forwardInput += 1;
            }

            bool goingFastEnough = body.linearVelocity.magnitude > minSpeedToDrift;
            if (!isDrifting && Input.GetKeyDown(KeyCode.LeftShift) && goingFastEnough)
            {
                StartDrift();
            }

            if (isDrifting && Input.GetKeyUp(KeyCode.LeftShift))
            {
                StopDrift();
            }
        }
        else
        {
            horizontalInput = 0;
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                horizontalInput += -1;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                horizontalInput += 1;
            }

            forwardInput = 0;
            if (Input.GetKey(KeyCode.DownArrow))
            {
                forwardInput += -1;
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                forwardInput += 1;
            }

            bool goingFastEnough = body.linearVelocity.magnitude > minSpeedToDrift;
            if (!isDrifting && Input.GetKeyDown(KeyCode.Space) && goingFastEnough)
            {
                StartDrift();
            }

            if (isDrifting && Input.GetKeyUp(KeyCode.Space))
            {
                StopDrift();
            }
        }

        if (forwardInput < 0)
        {
            horizontalInput *= -1;
        }
    }

    private void StopDrift()
    {
        driftSource.Stop();
        shootOffSource.Play();
        
        isDrifting = false;
        driftSpeedFactor = 1.0f;

        var magOfCurVel = body.linearVelocity.magnitude;
        float timeDrifting = (Time.time - timeStartDrift);
        float speedBoost = Time.time - timeStartDrift > .3f ? 1.5f : 1.0f;
        float moreThanpoint3 = timeDrifting - .3f;
        if (speedBoost > 1.0f) {
            speedBoost += moreThanpoint3 * 2;
        }
        body.linearVelocity = transform.forward * (magOfCurVel * speedBoost);
        particles.Emit(25);
    }

    private void StartDrift()
    {
        driftSource.Play();
        
        timeStartDrift = Time.time;
        isDrifting = true;
        startDriftVelocity = body.linearVelocity;
        driftSpeedFactorTween?.Kill();

        startDriftRotation = transform.localEulerAngles;
        driftSpeedFactor = 1.0f;
        driftSpeedFactorTween = DOTween.To(() => driftSpeedFactor, (x) => driftSpeedFactor = x, driftMinSpeedFactor, 2.0f)
            .SetEase(Ease.Linear);
    }
}
