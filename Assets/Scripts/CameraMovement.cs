using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class CameraMovement : MonoBehaviour {

    private float speed = 0.05f;
    

    float rotationY = 0.0f;
	float rotationX = 0.0f;


    

    private void Start()
    {
        rotationX = transform.rotation.eulerAngles.y;
    }

    void Update () {


        

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            // Get movement of the finger since last frame
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;

            // Move object across XY plane

            if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {

                transform.Translate(-touchDeltaPosition.x * speed, 0, -touchDeltaPosition.y * speed);
            }

        }
    }

    
}
