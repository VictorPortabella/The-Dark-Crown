using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextControllerFinalScene : MonoBehaviour {

    public Text comingoSoon;
    public Text tittle;

    public Text continueText;
    public Button continueButton;



    // Use this for initialization
    void Start () {
        comingoSoon.enabled = true;
        tittle.enabled = false;
        continueText.enabled = false;
        continueButton.enabled = false;

        StartCoroutine(TypeSentence(comingoSoon, comingoSoon.text));
        StartCoroutine(ChangeText(1));
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator TypeSentence(Text text, string sentence)
    {
        text.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            text.text += letter;
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator ChangeText(int numText)
    {
        if (numText == 1)
        {
            yield return new WaitForSeconds(5);
            comingoSoon.enabled = false;
            tittle.enabled = true;
            StartCoroutine(TypeSentence(tittle, tittle.text));
            StartCoroutine(ChangeText(2));
        }
        else if (numText == 2)
        {
            yield return new WaitForSeconds(5);
            continueText.enabled = true;
            continueButton.enabled = true;
            StartCoroutine(TypeSentence(continueText, continueText.text));
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
