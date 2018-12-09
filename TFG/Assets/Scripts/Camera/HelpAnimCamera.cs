using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpAnimCamera : MonoBehaviour {
    Animator anim;
    public Animator animatorFundidoNegro;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKey(KeyCode.F))
        {
            animatorFundidoNegro.SetBool("Fundido", true);
            anim.SetTrigger("AnimCamera");
        }


    }
}
