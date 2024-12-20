using UnityEngine;

[RequireComponent(typeof(Camera))] //Script can be attached only to camera
public class CameraScript : MonoBehaviour
{
    [Header("Initial camera settings")]
    [SerializeField] Transform player;
    [SerializeField] float horizontalOffset;
    [SerializeField] float verticalOffset;
    [SerializeField] float cameraTiltAngle;
    [SerializeField] float horizontalSmoothTime, verticalSmoothTime;
    [Tooltip("xDeadzone >= 0")]
    [SerializeField] float xDeadzone;

    float xref, yref;
    float y = 0;
    float xSmoothVelocity, ySmoothVelocity; // ref for smoothdamp
    float x = 0;
    float xDif = 0;
   
   
    [Header("Camera look forward settings")]
    public bool cameraLookForward;
    public float targetAngleValue;
    public float Angle_SmoothTime;

    float targetAngle;
    float yAng = 0;
    float s; // ref for smoothdamp

    [Header("Camera field of view")]
    [SerializeField] float cameraFOV;

    Camera camera;
    private void Start()
    {
        camera = this.GetComponent<Camera>();
    }
    float CameraYAngle()
    {
        if (cameraLookForward)
        {
            //Input from playerInputScript
            if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
            {
                targetAngle = targetAngleValue;
            }
            else if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            {
                targetAngle = -targetAngleValue;
            }
            else
            {
                targetAngle = 0;
            }
            yAng = Mathf.SmoothDamp(yAng, targetAngle, ref s, Angle_SmoothTime);
            return yAng;
        }
        else
        {
            return 0;
        }
    }
    void LateUpdate()
    {        
        transform.rotation = Quaternion.Euler(cameraTiltAngle, CameraYAngle(), 0);
        camera.fieldOfView = cameraFOV;

        xDif = player.position.x - xref;
        if (Mathf.Abs(xDif) > xDeadzone)
        {
            xref = player.position.x - xDeadzone * Mathf.Sign(xDif);
        }
        x = Mathf.SmoothDamp(transform.position.x, xref, ref xSmoothVelocity, horizontalSmoothTime);

        yref = player.position.y + verticalOffset;
        y = Mathf.SmoothDamp(transform.position.y, yref, ref ySmoothVelocity, verticalSmoothTime);

        transform.position = new Vector3(x, y, horizontalOffset);
    }

}
