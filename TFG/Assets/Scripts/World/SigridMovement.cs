using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SigridMovement : MonoBehaviour {

    private static SigridMovement instance;
    public static SigridMovement MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SigridMovement>();
            }
            return instance;
        }
    }

    Transform transform;
    public Vector3 destination;
    public Vector3 Destination
    {
        get { return destination; }
        set { destination = value; }
    }

    Transform player;
    UnityEngine.AI.NavMeshAgent nav;

    // Use this for initialization
    void Start () {
        transform = GetComponent<Transform>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();

        destination = player.position + new Vector3(5, 0, 5);

    }

    // Update is called once per frame
    void Update () {
        nav.SetDestination(destination);
    }
}
