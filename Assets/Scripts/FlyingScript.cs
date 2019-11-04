using UnityEngine;
public class FlyingScript : MonoBehaviour
{
    public float LeftPosition;
    public float RightPosition;


    public float speed;
    public float targetSpeed;

    public float MinSpeed = 2;
    public float MaxSpeed = 15;


    public bool firstFlapp = false;

    public GameObject target;
    public GameObject LeftWing;
    public GameObject RightWing;

    public GameObject TopHeightReference;
    public GameObject BottomHeightReference;

    public GameObject LTracker;
    public GameObject RTracker;

    public float[] Leftpositions;
    public float[] Rightpositions;

    public int frames = 0;

    private void Start()
    {
        speed = 0;
        targetSpeed = MinSpeed;

        Leftpositions = new float[5];
        Rightpositions = new float[5];
    }

    private void Update()
    {
        frames++;

        LeftPosition = LTracker.transform.position.y;
        RightPosition = RTracker.transform.position.y;

        Leftpositions[Time.frameCount % 5] = LeftPosition;
        Rightpositions[Time.frameCount % 5] = RightPosition;

        SynchronizeWings();

        if (frames < 10)
            return;

        if (firstFlapp == true)
        {
            DetectLeftFlap();
            DetectRightFlap();

            UpdateSpeed();

            MoveOverSpeed();
        }
        else
        {
            DetectFirstFlap();
        }
    }

    private void SynchronizeWings()
    {
        float leftHeight = Mathf.InverseLerp(BottomHeightReference.transform.position.y, TopHeightReference.transform.position.y, LeftPosition);
        float leftWingAngle = Mathf.Lerp(-40, 50, leftHeight);
        LeftWing.transform.localEulerAngles = new Vector3(leftWingAngle, LeftWing.transform.localEulerAngles.y, LeftWing.transform.localEulerAngles.z);

        float rightHeight = Mathf.InverseLerp(BottomHeightReference.transform.position.y, TopHeightReference.transform.position.y, RightPosition);
        float rightWingAngle = Mathf.Lerp(40, -50, rightHeight);
        RightWing.transform.localEulerAngles = new Vector3(rightWingAngle, RightWing.transform.localEulerAngles.y, RightWing.transform.localEulerAngles.z);
    }

    private void DetectRightFlap()
    {
        targetSpeed += Mathf.Abs(RightPosition - Rightpositions[(Time.frameCount - 1) % 5]);
    }

    private void DetectLeftFlap()
    {
        targetSpeed += Mathf.Abs(LeftPosition - Leftpositions[(Time.frameCount - 1) % 5]);
    }

    private void UpdateSpeed()
    {
        speed = Mathf.MoveTowards(speed, targetSpeed, Time.deltaTime);
        targetSpeed -= 0.02f;
        targetSpeed = Mathf.Clamp(targetSpeed, MinSpeed, MaxSpeed);
    }

    private void MoveOverSpeed()
    {
        if (transform.position != target.transform.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        }
    }

    private void DetectFirstFlap()
    {
        if (Mathf.Abs(RightPosition - Rightpositions[(Time.frameCount - 4) % 5]) >= 0.4f || Mathf.Abs(LeftPosition - Leftpositions[(Time.frameCount - 4) % 5]) >= 0.4f)
        {
            firstFlapp = true;
        }
    }
}
