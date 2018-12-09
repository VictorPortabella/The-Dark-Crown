using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour {

    public SpamEnemy spamEnemy;

    [SerializeField]
    public bool deleteTrigger;

    public void TriggerEnemy()
    {
        FindObjectOfType<EnemyManager>().Spawn(spamEnemy);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            TriggerEnemy();
            if (deleteTrigger == true)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
