using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    private List<killEnemiesToDialogue> listEnemies;

    private EnemyHealth enemyHealth;
    private EnemyMovement enemyMovement;

    private GameObject checkPoint;

    private GameObject lastEnemyDeath;

    private static EnemyManager instance;
    public static EnemyManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<EnemyManager>();
            }
            return instance;
        }
    }

    void Start()
    {
        listEnemies = new List<killEnemiesToDialogue>();
    }
    void Update()
    {
        foreach (killEnemiesToDialogue actualEnemieList in listEnemies)
        {

                foreach (GameObject enemy in actualEnemieList.MyEnemiesAlive)
                {
                    enemyHealth = enemy.GetComponent<EnemyHealth>();
                    if (enemyHealth.isDead == true)
                    {
                        lastEnemyDeath = enemy;
                        actualEnemieList.MyEnemiesAlive.Remove(enemy);
                    }
                }
                if (actualEnemieList.MyEnemiesAlive.Count == 0)
                {
                    if (actualEnemieList.MyDialogueUp == true)
                    {
                        FindObjectOfType<DialogueManager>().StartDialogue(actualEnemieList.MyFutureDialogue);
                    }                
                    if(actualEnemieList.CheckPoint != null)
                    {
                        GameSystemInGame.MyInstance.LastCheckPointPos = actualEnemieList.CheckPoint.transform.position;
                    }
                    if(actualEnemieList.ChangeScene == true)
                    {
                        MenuStats.MyInstance.Scene = 2;
                        GameSystemInGame.MyInstance.ChangeScene("Scene 2", 22);
                    }
                    if(actualEnemieList.EndGame == true) //ENDGAME
                    {
                        FindObjectOfType<EndGame>().EndGameFunction(lastEnemyDeath.GetComponent<Transform>());
                        actualEnemieList.EndGame = false;
                    }
                listEnemies.Remove(actualEnemieList);
                }
        }       
    }

    // Update is called once per frame
    public void Spawn(SpamEnemy spamEnemy)
    {
         killEnemiesToDialogue newListEnemies = new killEnemiesToDialogue();
         foreach (EnemiesPoint enemyPoint in spamEnemy.enemiesPoint)
         {
                foreach (GameObject enemy in enemyPoint.enemiesList)
                {
                    GameObject enemySpawned = Instantiate(enemy, enemyPoint.spawnPoint.position + new Vector3(Random.Range(0, 10.0f), Random.Range(0, 1.0f), Random.Range(0, 10.0f)), enemyPoint.spawnPoint.rotation);
                    newListEnemies.MyEnemiesAlive.Add(enemySpawned);
                    if (enemyPoint.firstMovement != null)
                    {
                        enemyMovement = enemySpawned.GetComponent<EnemyMovement>();
                        enemyMovement.originalPosition = enemyPoint.firstMovement.position;
                    }
                }
         }

        if (spamEnemy.futureDialogue.sentences.Length != 0)
        {
            newListEnemies.MyDialogueUp = true;
            newListEnemies.MyFutureDialogue = spamEnemy.futureDialogue;
        }

        if(spamEnemy.checkPoint != null)
        {
            newListEnemies.CheckPoint = spamEnemy.checkPoint;
        }
        else { newListEnemies.CheckPoint = null; }

        newListEnemies.ChangeScene = spamEnemy.changeScene;
        newListEnemies.EndGame = spamEnemy.endGame;
        listEnemies.Add(newListEnemies);
    }
}

public class killEnemiesToDialogue : MonoBehaviour
{
    private List<GameObject> enemiesAlive = new List<GameObject>();
    public List<GameObject> MyEnemiesAlive
    {
        get { return enemiesAlive; }
        set { enemiesAlive = value; }
    }
  
    private Dialogue futureDialogue;
    public Dialogue MyFutureDialogue
    {
        get { return futureDialogue; }
        set { futureDialogue = value; }
    }
    private bool dialogueUp;
    public bool MyDialogueUp
    {
        get { return dialogueUp; }
        set { dialogueUp = value; }
    }

    private GameObject checkPoint;
    public GameObject CheckPoint
    {
        get { return checkPoint; }
        set { checkPoint = value; }
    }

    private bool changeScene;
    public bool ChangeScene
    {
        get { return changeScene; }
        set { changeScene = value; }
    }

    private bool endGame;
    public bool EndGame
    {
        get { return endGame; }
        set { endGame = value; }
    }
}


