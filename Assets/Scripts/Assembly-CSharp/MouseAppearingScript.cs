using System;
using UnityEngine;
using UnityEngine.UI;

public class MouseAppearingScript : MonoBehaviour
{
    public GameControllerScript gc;
    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
        RaycastHit raycastHit;
        bool rayBool = Physics.Raycast(ray, out raycastHit);
        if (rayBool && (raycastHit.collider.tag == "Door" && Vector3.Distance(this.playerTransform.position, raycastHit.transform.position) <= 15f))
        {
            this.MouseCursor.SetActive(true);
        }
        else if (rayBool && (raycastHit.collider.tag == "Item" && Vector3.Distance(this.playerTransform.position, raycastHit.transform.position) <= 10f))
        {
            this.MouseCursor.SetActive(true);
        }
        else if (rayBool && (raycastHit.collider.tag == "Notebook" && Vector3.Distance(this.playerTransform.position, raycastHit.transform.position) <= 10f))
        {
            this.MouseCursor.SetActive(true);
        }
        else if (gc.item[gc.itemSelected] == 11 && rayBool && (raycastHit.collider.tag == "Window" && Vector3.Distance(this.playerTransform.position, raycastHit.transform.position) <= 10f))
        {
            this.MouseCursor.SetActive(true);
        }
        else if (gc.item[gc.itemSelected] == 11 && rayBool && (raycastHit.collider.gameObject == gc.wall.gameObject && Vector3.Distance(this.playerTransform.position, raycastHit.transform.position) <= 10f))
        {
            if (gc.wall.currentCrack == 1)
                this.MouseCursor.SetActive(true);
        }
        else if (gc.item[gc.itemSelected] == 4 && rayBool && (raycastHit.collider.gameObject == gc.wall.gameObject && Vector3.Distance(this.playerTransform.position, raycastHit.transform.position) <= 10f))
        {
            if (gc.wall.currentCrack == 2)
                this.MouseCursor.SetActive(true);
        }
        else if (gc.item[gc.itemSelected] == 8 && rayBool && (raycastHit.collider.gameObject == gc.wall.gameObject && Vector3.Distance(this.playerTransform.position, raycastHit.transform.position) <= 10f))
        {
            if (gc.wall.currentCrack == 3)
                this.MouseCursor.SetActive(true);
        }
        else
        {
            this.MouseCursor.SetActive(false);
        }
    }
   
    public GameObject MouseCursor;

    public Transform playerTransform;
}