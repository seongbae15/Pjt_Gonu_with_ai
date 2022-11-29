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


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hitPoint = Physics2D.Raycast(worldPoint, Vector2.zero, Mathf.Infinity, pointLayerMask);
            RaycastHit2D hitStone = Physics2D.Raycast(worldPoint, Vector2.zero, Mathf.Infinity, stoneLayerMask);

            if (hitStone)
            {
                Debug.Log("Try Again other places");
            }
            else if (hitPoint)
            {
                Transform stonePlaceTransform = hitPoint.transform;
                GameManager.Instance.PlaceStone(stonePlaceTransform);
            }
        }
    }
}
