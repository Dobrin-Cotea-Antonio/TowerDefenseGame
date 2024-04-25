using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Class <c>CameraMovement</c> is used to move the camera in the scene.
/// </summary>
public class CameraMovement : MonoBehaviour{

    [SerializeField] float cameraMovementSpeed=1;

    Rigidbody rb;

    DefaultInputActions input;
    Vector3 right;
    Vector3 forward;
    Vector3 moveDirection;

    private void Start(){
        input = new DefaultInputActions();
        rb = GetComponent<Rigidbody>();

        Quaternion cameraRotation = transform.rotation;
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        right = transform.right;
        forward = transform.forward;
        transform.rotation = cameraRotation;

    }

    private void Update(){
        if (Time.timeScale == 0)
            return;

        rb.velocity = moveDirection*(1/Time.timeScale);
    }

    private void OnMove(InputValue pInputValue){
        Vector2 direction = pInputValue.Get<Vector2>()*cameraMovementSpeed;
        moveDirection = direction.y * forward + direction.x * right;
    }



}
