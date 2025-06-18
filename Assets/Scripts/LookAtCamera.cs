using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] private LookAtMode lookAtMode;

    private enum LookAtMode
    {
        LookAtCamera,
        LookAtCameraInverted,
        CameraForward,
        CameraForwardInverted
    }

    private void LateUpdate()
    {
        switch (lookAtMode)
        {
            case LookAtMode.LookAtCamera:
                transform.LookAt(Camera.main.transform);
                break;
            case LookAtMode.LookAtCameraInverted:
                Vector3 directionToCamera = transform.position - Camera.main.transform.position;
                transform.LookAt(transform.position + directionToCamera);
                break;
            case LookAtMode.CameraForward:
               transform.forward = Camera.main.transform.forward;
                break;
            case LookAtMode.CameraForwardInverted:
                transform.forward = -Camera.main.transform.forward;
                break;
        }
    }
}
