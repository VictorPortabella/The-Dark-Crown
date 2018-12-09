using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Muro : MonoBehaviour {

    public BoxCollider muro;
    // Use this for initialization
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            muro.enabled = true;
            Destroy(this);
        }
    }
}
