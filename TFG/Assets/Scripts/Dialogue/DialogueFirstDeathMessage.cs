using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueFirstDeathMessage : MonoBehaviour {

    [SerializeField]
    public string name;

    [SerializeField]
    [TextArea(3, 10)]
    public string[] dialogueSentences;

    public Text nameText;
    public Text dialogueText;

    public Animator animatorDialogue;

    private Queue<string> sentences;

    void Awake()
    {
        sentences = new Queue<string>();
    }
    // Use this for initialization
    public void StartDialogue () {
        animatorDialogue.SetBool("IsOpen", true);
        sentences.Clear();
        foreach (string sentence in dialogueSentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        Debug.Log(sentences.Count);
        if (sentences.Count == 0)
        {
           EndDialogue();
            return;
        }
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    void EndDialogue()
    {
        animatorDialogue.SetBool("IsOpen", false);
    }
}
