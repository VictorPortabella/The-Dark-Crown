using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    private GameSystemInGame gm;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameSystemInGame.MyInstance.LastCheckPointPos = transform.position;
        }
    }
}
