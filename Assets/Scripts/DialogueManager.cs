using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public Image charaImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI messageText;
    public GameObject moveUI;
    public GameObject dialogueUI;
    
    public Character[] characters;

    public bool isfinished;

    Message[] currentMessage;
    Character[] currentChara;
    int activeMessage = 0;
    public static bool isMessageActive = false;

    public void OpenDialogue(Message[] messages)
    {
        isfinished = false;

        currentMessage = messages;
        currentChara = characters;
        activeMessage = 0;
        isMessageActive = true;

        moveUI.SetActive(false);
        dialogueUI.SetActive(true);

        MessageDisplay();
    }

    public void MessageDisplay()
    {
        Message messageToDisplay = currentMessage[activeMessage];
        messageText.text = messageToDisplay.message;

        Character charaToDisplay = currentChara[messageToDisplay.charaID];
        nameText.text = charaToDisplay.charaName;
        charaImage.sprite = charaToDisplay.charaSprite;
    }

    public void NextMessage()
    {
        activeMessage++;
        if (activeMessage < currentMessage.Length)
        {
            MessageDisplay();
        } else
        {
            isMessageActive = false;
            moveUI.SetActive(true);
            dialogueUI.SetActive(false);

            isfinished = true;
        }
    }
}

[System.Serializable]
public class Character
{
    public string charaName;
    public Sprite charaSprite;
}
