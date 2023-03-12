using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBlocks : MonoBehaviour
{
    private Sprite cursprite;
    public Sprite midsprite;
    public GameObject collisionChild;

    void Start()
    {
        cursprite = GetComponent<SpriteRenderer>().sprite;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Touched());
        }
    }

    IEnumerator Touched()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.GetComponent<SpriteRenderer>().sprite = midsprite;


        yield return new WaitForSeconds(0.5f);
        gameObject.gameObject.GetComponent<SpriteRenderer>().sprite = midsprite;
        gameObject.gameObject.GetComponent<Collider2D>().enabled = false;
        collisionChild.SetActive(false);


        yield return new WaitForSeconds(2f);
        gameObject.gameObject.GetComponent<Collider2D>().enabled = true;
        gameObject.gameObject.GetComponent<SpriteRenderer>().sprite = cursprite;
        collisionChild.SetActive(true);
    }
}
