using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class <c>LookAtCamera</c> is used to make world space ui face the camera;
/// </summary>
public class LookAtCamera : MonoBehaviour{


    private void LateUpdate(){
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }


}
