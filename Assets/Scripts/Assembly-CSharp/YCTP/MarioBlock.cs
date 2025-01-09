using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MarioBlock : MonoBehaviour
{
    public Image image;
    public Sprite hit;
    public bool wasHit;
    public UnityEvent onHit;
    public bool normal;
    public Vector2 specific;
    public bool isTrigger;
    public bool canHit;

    void OnHit(Collision2D collision, Collider2D collider)
    {
        if (!canHit) return;
        if (!wasHit)
        {
            if ((collision != null && collision.gameObject.CompareTag("Mario")) || collider.gameObject.CompareTag("Mario"))
            {
                if ((collision != null && collision.GetContact(0).normal == specific) || !normal)
                {
                    image.sprite = hit;
                    //Debug.Log("sgut");
                    wasHit = true;
                    onHit.Invoke();
                }
            }
        }
    }

    public void ChangeCan(bool _can)
    {
        canHit = _can;
    }    

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isTrigger) return;
        OnHit(collision, null);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isTrigger) return;
        OnHit(null, collision);
    }
}
