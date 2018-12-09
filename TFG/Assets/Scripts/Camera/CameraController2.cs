using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController2 : MonoBehaviour {

    private const float Y_ANGLE_MIN = 2.0f;
    private const float Y_ANGLE_MAX = 50.0f;

    public Transform lookAt;
    public Transform camTransform;

    private Vector3 desiredPosition;
    private Camera cam;
    
    private float currentX = 0.0f;
    private float currentY = 0.0f;

    public float distance = 10.0f;       //distancia de la camara respecto del jugador
    public float desired_Y_angle = 20.0f; //angulo de la camara apuntando al jugador 
    public float offsetHeight = 2.0f;   //altura añadida de la camara (Jugador respecto borde inferior)
    public float sensitivityX = 2.0f;
    public float distnace_collision = 2.0f;

    public LayerMask mask;


    void Start () {
        camTransform = transform;
        cam = Camera.main;
    }

    private void Update()
    {
        currentX = lookAt.eulerAngles.y;
        currentY = desired_Y_angle;
    }

    void LateUpdate () {

        

        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);

        //ponemos la camara en el jugador, miramos la rotacion y retrocedemos la distancia deseada
        Vector3 cameraPosition = lookAt.position + rotation * dir;  //Pensar en añadir el offset a la comprobacion

        //Queremos comprovar que nada tapa al jugador. Utiliziamos un RaycastHit
        RaycastHit collisionHit;

        float correctedDistance;
        Vector3 correctedDir;
        if(Physics.Raycast(lookAt.position + new Vector3(0,0.2f,0), camTransform.TransformDirection(Vector3.back), out collisionHit, Mathf.Infinity, ~mask))
        {
            correctedDistance = Vector2.Distance(lookAt.position, collisionHit.point);
            if (Mathf.Abs(distnace_collision * correctedDistance) < Mathf.Abs(distance))
            {
                correctedDir = new Vector3(0, 0, -distnace_collision * correctedDistance);
                cameraPosition = lookAt.position + rotation * correctedDir;
            }
        } 
        

        //Interpolamos la posicion deseada
        desiredPosition = Vector3.Lerp((camTransform.position - new Vector3(0, offsetHeight, 0)), cameraPosition, Time.deltaTime*sensitivityX);

        //Movemos la camara
        camTransform.position = desiredPosition;

        //Rotates the transform so the forward vector points at /target/'s current position.
        camTransform.LookAt(lookAt.position);

        //aplicamos la altura añadida
        camTransform.position += new Vector3(0,offsetHeight,0);

    }
}
