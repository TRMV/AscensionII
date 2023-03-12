using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingBehavior : MonoBehaviour
{
    public GameObject dialoguetri;

    void Start()
    {
        GetComponent<DialogueTrigger>().StartDialogue();
    }

    private void Update()
    {
        if (dialoguetri.GetComponent<DialogueManager>().isfinished)
        {
            SceneManager.LoadScene(0);
        }
    }
}
