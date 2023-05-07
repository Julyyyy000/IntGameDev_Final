using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    AudioSource myAudio;
    public DialogueRunner dialogueRunner;
    //public GameObject telephone;
    //public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        myAudio = GetComponent<AudioSource>();

        dialogueRunner.AddCommandHandler<GameObject>(
            "play_sound",     // the name of the command
            PlaySound // the method to run
        );
        dialogueRunner.AddCommandHandler<GameObject>(
            "set_to_interactive",     // the name of the command
            SetToInteractive // the method to run
        );

        dialogueRunner.AddCommandHandler<string>(
            "change_scene",     // the name of the command
            ChangeScene // the method to run
        );
    }

    public void PlaySound(GameObject target)
    {
        //target.GetComponent<AudioSource>().Play();
        target.GetComponent<AudioSource>().Play();
    }

    public void SetToInteractive(GameObject target)
    {
        target.tag = "interactive";
    }

    public void ChangeScene(string target)
    {
        SceneManager.LoadScene(target);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
