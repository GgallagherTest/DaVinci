using UnityEngine;

public class TankScript : MonoBehaviour
{

    public GameObject Tank;
    public GameObject HandCrank;
    public GameObject Tracker;
    public GameObject Target;
    public float[] EulerAngles = new float[10];

    public float speed;
    public float targetSpeed;

    public float MinSpeed = 0;
    public float MaxSpeed = 5;

    public int frames = 0;

    public float InitialTrackerZLocalRotation;

    private void Start()
    {
        EulerAngles = new float[10];

        speed = MinSpeed;
        targetSpeed = MinSpeed;

        InitialTrackerZLocalRotation = Tracker.transform.localEulerAngles.z;
    }

    // Update is called once per frame
    private void Update()
    {
        frames++;

        // Compute rotation
        Quaternion newRotation = Quaternion.identity; // Create new quaternion with no rotation
        newRotation.x = HandCrank.transform.rotation.x; // Get only the X rotation from the tracker quaternion

        // Output the x rotation. It should be a value between -1 and 1
        // Convert to a value between 0 and 360
        float xEuler = (newRotation.x + 1) * 180;
        // Output the x rotation as a Euler angle

        SynchronizeCrank();

        EulerAngles[Time.frameCount % 10] = xEuler;

        if (frames < 12)
            return;

        DetectMovement(xEuler);

        UpdateSpeed();

        MoveOverSpeed();
    }

    private void SynchronizeCrank()
    {
        HandCrank.transform.localEulerAngles = new Vector3(Tracker.transform.localEulerAngles.z - InitialTrackerZLocalRotation, 0f, 0f);
    }

    private void DetectMovement(float xEuler)
    {
        if (Mathf.Abs(xEuler - EulerAngles[(Time.frameCount - 9) % 10]) > 10f)
        {
            targetSpeed = MaxSpeed;
        }
    }

    private void MoveOverSpeed()
    {
        if (transform.position != Target.transform.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, speed * Time.deltaTime);
        }
    }

    private void UpdateSpeed()
    {
        speed = Mathf.MoveTowards(speed, targetSpeed, Time.deltaTime);
        targetSpeed -= 0.15f;
        targetSpeed = Mathf.Clamp(targetSpeed, MinSpeed, MaxSpeed);
    }
}


