using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private static DialogueManager instance;
    public static DialogueManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DialogueManager>();
            }
            return instance;
        }
    }
    public Text nameText;
    public Text dialogueText;
    public Text tutorialText;

    public Animator animatorDialogue;
    public Animator animatorTutorial;

    private Queue<string> sentences;

    private SpamEnemy spamEnemy;
    private Dialogue dialogue;

    // Use this for initialization
    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        this.dialogue = dialogue;
        EndDialogue(); //COGE DOBLE
        switch (dialogue.type)
        {
            case DialogueType.DIALOGUE:
                if (Inventory.MyInstance.bagIsClose == false)
                {
                     Inventory.MyInstance.OpenClose();
                }

                animatorTutorial.SetBool("IsOpen", false);
                animatorDialogue.SetBool("IsOpen", true);
                nameText.text = dialogue.name;
                break;
            case DialogueType.TUTORIAL:
                animatorDialogue.SetBool("IsOpen", false);
                animatorTutorial.SetBool("IsOpen", true);
                break;
        }
        sentences.Clear();
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
        if (dialogue.spamEnemy.enemiesPoint.Count > 0)
        {
            spamEnemy = dialogue.spamEnemy;
        }
        else
        {
            spamEnemy = null;
        }
        if(dialogue.situationalAudio != null)
        {
            dialogue.situationalAudio.Play();
        }

    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            if (dialogue.itemList.Count > 0)
            {
                foreach (Item item in dialogue.itemList)
                {
                    Inventory.MyInstance.AddItem(item);
                }
            }
            if(dialogue.changeScene == true)
            {
                GameSystemInGame.MyInstance.ChangeScene("SceneFinal", 6);
            }
            EndDialogue();
          
            return;
        }
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }   

    IEnumerator TypeSentence (string sentence)
    {
        dialogueText.text = "";
        tutorialText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            tutorialText.text += letter;
            yield return null;
        }
    }
    
    void EndDialogue()
    {
        animatorDialogue.SetBool("IsOpen", false);
        animatorTutorial.SetBool("IsOpen", false);
        if (spamEnemy != null)
        {
                FindObjectOfType<EnemyManager>().Spawn(spamEnemy);
        }
        spamEnemy = null;

        if (dialogue.enableCombo == true)
        {
            PlayerAttacking.MyInstance.EnableCombo = true;
        }
        spamEnemy = null;
    }
}
