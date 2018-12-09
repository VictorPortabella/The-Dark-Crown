using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionRangeIn : MonoBehaviour {

    public EnemyMovement enemyMovement;
    // Use this for initialization
    void Awake () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            enemyMovement.SetAttackEnemy(true);
        }
    }
}
