using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class CameraMovement : MonoBehaviour {

    public Button cameraStartPosition;
    public Vector3 startposition;

    public GameObject cameraObject;

    private float speed = 0.05f;
    

    float rotationY = 0.0f;
	float rotationX = 0.0f;


    public void onCameraStartPositionButtonClicked()
    {
        transform.position = startposition;
    }
    

    private void Start()
    {
        //rotationX = transform.rotation.eulerAngles.y;
        cameraObject = GameObject.FindGameObjectWithTag("CameraObject");
        startposition = cameraObject.transform.position;
        Debug.Log("TEST" + startposition);

        //cameraStartPosition.onClick.AddListener(() => { onCameraStartPositionButtonClicked(); });
    }

    void Update () {

        
        
        if(Input.touchCount == 2)
        {
            zoomCamera();
        }
        else
        {
            moveCamera();
            
        }

    }


    //move camera along x(right,left) and z(forward,back)
    //moves if more than one touches and moved
    //clamps to limit how far camera can move
    private void moveCamera()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            // Get movement of the finger since last frame
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            

            if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                transform.Translate(-touchDeltaPosition.x * speed, 0, -touchDeltaPosition.y * speed);

                
                Vector3 clampedPosition = transform.position;
                clampedPosition.x = Mathf.Clamp(transform.position.x, 20.1f, 400.1f);
                clampedPosition.z = Mathf.Clamp(transform.position.z, 40.1f, 400.1f);
                transform.position = clampedPosition;
            }

        }
    }


    //zoom camera(y direction)
    //gets touches of both fingers
    //calculates distanz from first and last touch
    //distanz as factor to zoom
    //clamps to limit zoom
    private void zoomCamera()
    {
        Touch touchZero = Input.GetTouch(0);
        Touch touchOne = Input.GetTouch(1);

        Vector2 touchZeroPrevPosition = touchZero.position - touchZero.deltaPosition;
        Vector2 touchOnePrevPosition = touchOne.position - touchOne.deltaPosition;

        float prevTouchDeltaMag = (touchZeroPrevPosition - touchOnePrevPosition).magnitude;
        float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

        float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

        transform.Translate(0, deltaMagnitudeDiff * speed, 0);


        // initially, the temporary vector should equal the player's position
        Vector3 clampedPosition = transform.position;
        // Now we can manipulte it to clamp the y element
        clampedPosition.y = Mathf.Clamp(transform.position.y, 11.1f, 70.1f);
        // re-assigning the transform's position will clamp it
        transform.position = clampedPosition;
    }
}
