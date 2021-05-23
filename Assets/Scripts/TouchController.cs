using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : MonoBehaviour
{
    //Refrences
    public Transform cameraTransform;
    public CharacterController characterController;

    //Player Settings
    public float cameraSensitivity;
    public float moveSpeed;
    public float moveInputDeadZone;


    //Touch detection
    int leftFIngerId, rightFingerId;
    float halfScreenWidth;

    //Camera Control
    Vector2 lookInput;
    float cameraPitch;

    //player movement
    Vector2 moveTouchStartPosition;
    Vector2 moveInput;

    // Start is called before the first frame update
    void Start()
    {
        //id = -1 means the finger is not been tracked yet
        leftFIngerId = -1;
        rightFingerId = -1; 

        //only calculate once
        halfScreenWidth = Screen.width / 2;

        //Calculate the movement input dead zone
        moveInputDeadZone = Mathf.Pow(Screen.height / moveInputDeadZone, 2);
    }

    // Update is called once per frame
    void Update()
    {
        //Handles Input
        GetTouchInput();
        
        if(rightFingerId !=-1)
        {
            //look around if the right finger is being tracked 
            LookAround();
        }
       
        if(leftFIngerId !=-1)
        {
            //move if the left finger is being tracked
            Move();
            Debug.Log("Moving");

        }
    }

    void GetTouchInput()
    {
        //iterate through all the detected touch
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch t = Input.GetTouch(i);
            //check each touch phase
            switch (t.phase)
            {
                case TouchPhase.Began:
                    if (t.position.x < halfScreenWidth && leftFIngerId == -1)
                    {
                        leftFIngerId = t.fingerId;
                        // Debug.Log("Tracking left finger");

                        //Set the position for the movement control finger
                        moveTouchStartPosition = t.position;
                    }
                    else if (t.position.x > halfScreenWidth && rightFingerId == -1)
                    {
                        rightFingerId = t.fingerId;
                        Debug.Log("Tracking right finger");
                    }
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:

                    if (t.fingerId == leftFIngerId)
                    {
                        leftFIngerId = -1;
                        Debug.Log("Stopped tracking left finger ");
                    }
                    else if (t.fingerId == rightFingerId)
                    {
                        rightFingerId = -1;
                        Debug.Log("Stopped tracking right finger");
                    }
                    break;
                case TouchPhase.Moved:
                    
                    //Get Input to look around
                    if(t.fingerId == rightFingerId)
                    {
                        lookInput = t.deltaPosition * cameraSensitivity * Time.deltaTime; 
                    }
                    else if(t.fingerId == leftFIngerId)
                    {
                        moveInput = t.position - moveTouchStartPosition;
                    }
                    break;
                case TouchPhase.Stationary:

                    //Set the look input to zero if the finger is still
                    if(t.fingerId == rightFingerId)
                    {
                        lookInput = Vector2.zero;
                    }
                    break;
            }
        }
    }

    void LookAround()
    {
        //Vertical pitch rotation
        cameraPitch = Mathf.Clamp(cameraPitch - lookInput.y, -41f, 29f);
        cameraTransform.localRotation = Quaternion.Euler(cameraPitch, 0, 0);

        //horizontal yaw rotation
        transform.Rotate(transform.up, lookInput.x);
    }

    private void Move()
    {
        //dont move if the touch delta is shorter than the designated dead zone
        if (moveInput.sqrMagnitude <= moveInputDeadZone) return;

        //multiply the normalized direction by the speed 
        Vector2 movementDirection = moveInput.normalized * moveSpeed * Time.deltaTime;

        //move relatively to the local transform direction
        characterController.Move(transform.right * movementDirection.x + transform.forward * movementDirection.y);
    }    
}
