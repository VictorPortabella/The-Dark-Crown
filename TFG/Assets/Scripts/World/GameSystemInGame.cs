using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameSystemInGame : MonoBehaviour {


    private static GameSystemInGame instance;
    public static GameSystemInGame MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameSystemInGame>();
            }
            return instance;
        }
    }

    public Animator animCamera;
    public Animator animColor;


    private static Vector3 lastCheckPointPos;
    public Vector3 LastCheckPointPos
    {
        get { return lastCheckPointPos; }
        set { lastCheckPointPos = value; }
    }


    // Update is called once per frame
    void Update () {
	}

    public void ChangeScene(string scene, float timer)
    {
        if(scene == "Scene 2")
        {
            ChangeSceneAnimationScene2();
        }
        if (scene == "SceneInterfaz")
        {
            ChangeSceneAnimationInterfaz();
        }
        if(scene == "SceneFinal")
        {
            ChangeSceneFinal();
        }
        StartCoroutine(SceneChanging(scene, timer));
    }

    public void ChangeSceneAnimationScene2()
    {
        animColor.SetBool("Fundido", true);
        animCamera.SetTrigger("AnimCamera");
    }

    public void ChangeSceneAnimationInterfaz()
    {
        animColor.SetTrigger("IsDeath");
        animCamera.SetTrigger("IsDeath");
    }

    public void ChangeSceneFinal()
    {
        animColor.SetTrigger("FinalScene");
        animCamera.SetTrigger("FinalScene");
    }

    IEnumerator SceneChanging(string scene, float timer)
    {
        yield return new WaitForSeconds(timer);
        Application.LoadLevel(scene);
    }
}
