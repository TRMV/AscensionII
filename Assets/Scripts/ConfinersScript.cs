using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ConfinersScript : MonoBehaviour
{
    public CinemachineConfiner2D camConf;
    private Collider2D camFailsafe;

    public AudioSource aS;
    public AudioClip musicClip;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            camFailsafe = camConf.m_BoundingShape2D;
            camConf.m_BoundingShape2D = transform.GetComponent<PolygonCollider2D>();
            PlaysMusic();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            //camConf.m_BoundingShape2D = camFailsafe;
            //PlaysMusic();
        }
    }

    public void PlaysMusic()
    {
        if (aS.clip != musicClip)
        {
            aS.clip = musicClip;
            aS.loop = true;
            aS.Play();
        }
    }
}
