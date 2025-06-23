using UnityEngine;

public class CameraController : MonoBehaviour
{
    public CarController target;
    private Vector3 offsetDir;

    public float minDistance;
    public float maxDistance;
    private float activeDistance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (target != null)
        {
            offsetDir = transform.position - target.transform.position;
            activeDistance = minDistance;
            offsetDir.Normalize();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            activeDistance = minDistance + ((maxDistance - minDistance) * (target.theRB.linearVelocity.magnitude / target.maxSpeed));
            transform.position = target.transform.position + (offsetDir  * activeDistance);
        }
    }
}
