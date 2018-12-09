using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EsferaFinalScene : MonoBehaviour {

    Transform esferaTransform;
    private Vector3 desiredPosition;
    public float velocidadAltura;

	// Use this for initialization
	void Start () {
        esferaTransform = GetComponent<Transform>();
    }
	
	// Update is called once per frame
	void Update () {


        desiredPosition = Vector3.Lerp(esferaTransform.position , esferaTransform.position + new Vector3(0, velocidadAltura, 0), Time.deltaTime);

        esferaTransform.position = desiredPosition;

    }
}
