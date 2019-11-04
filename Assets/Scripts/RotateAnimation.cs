using UnityEngine;

public class RotateAnimation : MonoBehaviour
{
    public float speed = 1;
    public bool direction;

    void Update()
    {
        if (direction)
            transform.Rotate(transform.up, Time.deltaTime * speed);
        else
            transform.Rotate(transform.up, -Time.deltaTime * speed);
    }
}
