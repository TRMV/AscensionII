using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCBehavior : MonoBehaviour
{
    public GameObject talkButton;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            talkButton.SetActive(true);
            talkButton.GetComponent<Button>().onClick.AddListener(delegate { GetComponent<DialogueTrigger>().StartDialogue(); });
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            talkButton.SetActive(false);
        }
    }
}
