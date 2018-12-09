using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionRangeOut : MonoBehaviour {

    Transform transform;
    Vector3 originalPosition;
    public EnemyMovement enemyMovement;


    // Use this for initialization
    void Start () {
        transform = GetComponent<Transform>();
        originalPosition = transform.position;	
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = originalPosition;	
	}

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            enemyMovement.SetAttackEnemy(false);
        }
    }
}
