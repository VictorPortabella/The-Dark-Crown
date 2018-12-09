using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour {
    public GameObject sigrid;
    public GameObject humo;
    public BoxCollider muro;
    public GameObject piedraPoder;

    public Dialogue dialogue;

    public Transform sigridInstantiatePosition;
    

    public void EndGameFunction(Transform bossPosition)
    {
        GameObject Sigrid = Instantiate(sigrid, sigridInstantiatePosition.position, sigridInstantiatePosition.rotation);
        GameObject Piedra = Instantiate(piedraPoder, bossPosition.position, bossPosition.rotation);

        Destroy(humo);
        muro.enabled = false;

        StartCoroutine(FinalDialogue(dialogue, 4.75f));

    }

    IEnumerator FinalDialogue(Dialogue dialogue, float timer)
    {
        yield return new WaitForSeconds(timer);
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
}
