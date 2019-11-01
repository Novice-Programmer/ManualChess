using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera thisCamera;
    private bool cameraChange;
    private const float cameraSpeed = 10.0f;
    private float fieldOfViewValue;
    // Start is called before the first frame update
    void Start()
    {
        thisCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (NetworkManager.Instance.gamePlay)
        {
            if (!Graveyard.Instance.graveViewB)
            {
                if (cameraChange)
                {
                    thisCamera.fieldOfView = fieldOfViewValue;
                    cameraChange = false;
                }
                float wheelValue = Input.GetAxis("Mouse ScrollWheel");

                if (thisCamera.fieldOfView <= 28.0f && wheelValue > 0)
                {
                    thisCamera.fieldOfView = 28.0f;
                }
                else if (thisCamera.fieldOfView >= 35.0f && wheelValue < 0)
                {
                    thisCamera.fieldOfView = 35.0f;
                }
                else
                {
                    thisCamera.fieldOfView += -wheelValue * cameraSpeed;
                }
            }
            else
            {
                if (!cameraChange)
                {
                    fieldOfViewValue = thisCamera.fieldOfView;
                    thisCamera.fieldOfView = 15.0f;
                    cameraChange = true;
                }
            }
        }
    }
}
