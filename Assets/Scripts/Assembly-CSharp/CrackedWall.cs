using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackedWall : MonoBehaviour
{
    public int currentCrack;
    public List<Material> cracks;
    public new MeshRenderer renderer;
    public GameControllerScript gc;
    
    /*private void Start()
    {
        Debug.Log(cracks.Count);
        for (int i = 0; i < 3; i++)
        {
            Crack();
        }
    }*/

    public void Crack()
    {
        currentCrack++;
        if (currentCrack <= cracks.Capacity)
            renderer.material = cracks[currentCrack-1];
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BSODA") && currentCrack == 2)
        {
            gc.CrackWall();
            Destroy(other.gameObject);
        }
    }
}
