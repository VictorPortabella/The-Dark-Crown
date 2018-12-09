using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTriggerEnemyDeath : MonoBehaviour {

    public Dialogue dialogue;

    public List<EnemyHealth> enemiesHealth;

    void Update()
    {
        foreach(EnemyHealth enemy in enemiesHealth)
        {
            if(enemy.isDead == true)
            {
                enemiesHealth.Remove(enemy);
            }
        }
        if(enemiesHealth.Count == 0)
        {
            TriggerDialogue();
            Destroy(this);
        }
    }

        public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
}

