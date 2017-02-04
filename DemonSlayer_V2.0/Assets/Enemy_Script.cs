using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Script : MonoBehaviour {

    public Transform target;
    public float lookSmooth = 0.09f;
    public Vector3 offsetFromTarget = new Vector3(0, 6, -8);
    public float xTilt = 10;

    Vector3 destination = Vector3.zero;
    CharacterController_Script charController;
    float rotateVel = 0;

    void Start()
    {
        SetCameraTarget(target);
    }

    public void SetCameraTarget(Transform t)
    {
        target = t;

        if (target != null)
        {
            if (target.GetComponent<CharacterController_Script>())
            {
                charController = target.GetComponent<CharacterController_Script>();
            }
            else
                Debug.Log("The camera's target needs a character controller.");
        }
        else
            Debug.Log("Your camera needs a target.");
    }

    void LateUpdate()
    {
        // moving
        MoveToTarget();
        // rotating
        LookAtTargeT();
    }

    void MoveToTarget()
    {
        destination = charController.TargetRotation * offsetFromTarget;
        destination += target.position;
        transform.position = destination;
    }

    void LookAtTargeT()
    {
        float eulerYAngle = Mathf.SmoothDamp(transform.eulerAngles.y, target.eulerAngles.y, ref rotateVel, lookSmooth);
        transform.rotation = Quaternion.Euler(new Vector3(transform.eulerAngles.x, eulerYAngle, 0));
    }
}
