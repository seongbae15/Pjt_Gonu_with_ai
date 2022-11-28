using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ControllerManager : MonoBehaviour
{
    [SerializeField]
    private LayerMask pointLayerMask;
    [SerializeField]
    private LayerMask stoneLayerMask;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hitPoint = Physics2D.Raycast(worldPoint, Vector2.zero, Mathf.Infinity, pointLayerMask);
            RaycastHit2D hitSton = Physics2D.Raycast(worldPoint, Vector2.zero, Mathf.Infinity, stoneLayerMask);

            if (hitSton)
            {
                Debug.Log("Stone");
            }
            else if (hitPoint)
            {
                Debug.Log("Point");
            }
        }
    }
}
