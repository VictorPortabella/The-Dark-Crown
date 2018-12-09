using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SpamEnemy
{
    public List<EnemiesPoint> enemiesPoint;

    public Dialogue futureDialogue;

    public GameObject checkPoint;

    public bool changeScene;

    public bool endGame;
}


[System.Serializable]
public class EnemiesPoint
{
    public List<GameObject> enemiesList;
    public Transform spawnPoint;
    public Transform firstMovement;
}
