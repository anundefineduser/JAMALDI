using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WindowScript : MonoBehaviour
{
    public Collider collider;
    public MeshRenderer renderer;
    public Material normal;
    public Material broken;
    public NavMeshObstacle obstacle;
    public AudioSource source;
    public bool isBroken;
    // Start is called before the first frame update
    void Start()
    {
        //Break();
        Debug.Log(broken);
    }

    public bool Break()
    {
        if (isBroken) return false;
        isBroken = true;
        renderer.material = broken;
        collider.enabled = false;
        obstacle.enabled = false;
        source.Stop();
        source.Play();
        return true;
    }

    public bool Reenable()
    {
        if (isBroken) return false;
        isBroken = false;
        renderer.material = normal;
        collider.enabled = true;
        obstacle.enabled = true;
        return true;
    }
}
