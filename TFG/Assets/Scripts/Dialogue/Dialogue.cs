using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DialogueType { DIALOGUE, TUTORIAL };


[System.Serializable]
public class Dialogue
{

    public DialogueType type;

    public string name;

    [TextArea(3, 10)]
    public string[] sentences;

    public SpamEnemy spamEnemy;

    public List<Item> itemList;

    public bool enableCombo;

    public AudioSource situationalAudio;

    public bool changeScene;


}
