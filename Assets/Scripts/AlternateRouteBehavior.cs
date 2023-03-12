using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternateRouteBehavior : MonoBehaviour
{
    public GameObject finaldoor;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerBehavior pb = collision.GetComponent<PlayerBehavior>();

        if (collision.CompareTag("Player") && pb.featherNumber == 12)
        {
            if (transform.CompareTag("Open"))
            {
                finaldoor.GetComponent<Animator>().SetBool("Open", true);
            }
            else if (transform.CompareTag("Close"))
            {
                finaldoor.GetComponent<Animator>().SetBool("Close", true);
            }

        }
    }

}
