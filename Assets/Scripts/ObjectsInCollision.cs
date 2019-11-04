using System.Collections.Generic;
using UnityEngine;

public class ObjectsInCollision : MonoBehaviour
{
    public List<GameObject> objectsInCollision = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (!objectsInCollision.Contains(other.gameObject))
            objectsInCollision.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (objectsInCollision.Contains(other.gameObject))
            objectsInCollision.Remove(other.gameObject);
    }
}
