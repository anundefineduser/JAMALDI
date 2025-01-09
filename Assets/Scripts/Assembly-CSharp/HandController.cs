using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public static HandController instance;
    private void Awake()
    {
        instance = this;
    }

    public PlayerScript player;
    public Animator hand;
    public SpriteRenderer item;

    private void Update()
    {
        //Debug.Log(player.moveDirection.magnitude > 0.01f);
        hand.SetBool("Walking", player.cc.velocity.magnitude > 0.1f);
        float speed = (player.playerSpeed / Time.deltaTime) / 20f;
        hand.speed = speed <= 1f ? 1f : speed;
        item.sprite = player.gc.itemSprites[player.gc.item[player.gc.itemSelected]];
    }

    public static void DoorAnimation()
    {
        if (instance != null)
            instance.hand.SetTrigger("Door");
    }
}
