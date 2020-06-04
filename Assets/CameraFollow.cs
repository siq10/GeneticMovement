
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.3f;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    private Vector3 velocity = Vector3.zero;
    private void FixedUpdate()
    {
       /* 
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPostion = Vector3.SmoothDamp(transform.position, desiredPosition,ref velocity, smoothTime * Time.deltaTime);
        transform.position = smoothedPostion;

        transform.LookAt(target);
       */
    }
}
