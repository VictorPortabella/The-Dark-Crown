using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public Transform target;

    float targetHeight = 4f;
    float distance = 12;
    float offsetFromWall = 0.1f;
    float maxDistance = 20;
    float minDistance = 0.6f;
    float xSpeed = 200;
    float ySpeed = 200;
    float yMinLimit = -80;
    float yMaxLimit = 80;
    float zoomRate = 40;
    float rotationDampening = 3; //smooth rotation
    float zoomDampening = 5;
    LayerMask collisionLayers = -1;
    //bool lockToRearOfTarget = false;
    //bool allowMouseInputX = true;
    //bool allowMouseInputY = true;

    private float xDeg = 0;
    private float yDeg = 0;
    private float currentDistance;
    private float desiredDistance;
    private float correctedDistance;
    
    Vector3 angles;
    Rigidbody rigidbody;

	// Use this for initialization
	void Start () {
        angles = target.eulerAngles;
        xDeg = angles.x;
        yDeg = angles.y;
        currentDistance = distance;
        desiredDistance = distance;
        correctedDistance = distance;
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.freezeRotation = true;
	}
	
	// Update is called once per frame
	void LateUpdate () {

        Vector3 vTargetOffset;

        RotateBehindTarget();

        yDeg = ClampAngle(yDeg, yMinLimit, yMaxLimit);         //Sets value between min and max and returns value.

        //Set camera rotation
        Quaternion rotation = Quaternion.Euler(yDeg, xDeg, 0);

        //Calculate desired camera position
        vTargetOffset = new Vector3(0, -targetHeight, 0);
        // Vector3.forward = (0,0,1)
        Vector3 position = target.position - (rotation * Vector3.forward * desiredDistance + vTargetOffset);

        // Check for collision using the true target's desired registration point as set by user using height
        RaycastHit collisionHit;
        Vector3 trueTargetPosition = new Vector3(target.position.x, target.position.y + targetHeight, target.position.z);

        // If there was a collision, correct the camera position and calculate the corrected distance
        bool isCorrected = false;
        if (Physics.Linecast(trueTargetPosition, position, out collisionHit, collisionLayers, QueryTriggerInteraction.UseGlobal))
        {
            correctedDistance = Vector2.Distance(trueTargetPosition, collisionHit.point) - offsetFromWall;
            isCorrected = true;
        }

        if(!isCorrected || correctedDistance > currentDistance)
        {

            currentDistance = Mathf.Lerp(currentDistance, correctedDistance, Time.deltaTime * zoomDampening);
        }
        else
        {
            currentDistance = correctedDistance;
        }

        // Keep within limits
        currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);

        // Recalculate position based on the new currentDistance
        position = target.position - (rotation * Vector3.forward * currentDistance + vTargetOffset);

        //Finally Set rotation and position of camera
        transform.rotation = rotation;
        transform.position = position;

    }

    void RotateBehindTarget()
    {
        float targetRotationAngle = target.eulerAngles.y;      //desired target angle
        float currentRotationAngle = transform.eulerAngles.y;  //current camera angle

        //interpola
        xDeg = Mathf.LerpAngle(currentRotationAngle, targetRotationAngle,rotationDampening * Time.deltaTime );
    }
    
    static float ClampAngle ( float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}
