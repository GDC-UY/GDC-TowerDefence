using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Game : MonoBehaviour
{
    private static Game instance;
    public GameObject cellSelected;
    Ray TouchRay => Camera.main.ScreenPointToRay(Input.mousePosition);
    public static Game Instance

    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Game>();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Game encarga de los inputs
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Click");

            RaycastHit2D hit = Physics2D.Raycast(TouchRay.origin, TouchRay.direction);

            if (hit.collider != null)
            {
                Debug.Log("Hit object: " + hit.collider.gameObject.name);
                GameObject selected = GridManager.Instance.getCell(hit.point);
                if (selected != null)
                {
                    cellSelected = selected;
                }
            }
        }
    }


}
