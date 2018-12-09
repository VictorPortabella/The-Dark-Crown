using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableAttackTrigger : MonoBehaviour {

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerAttacking.MyInstance.EnableAttack = true;
        }
    }
}
