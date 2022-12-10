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
        if (!GameManager.Instance.isGameEnd)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hitPoint = Physics2D.Raycast(worldPoint, Vector2.zero, Mathf.Infinity, pointLayerMask);
                RaycastHit2D hitStone = Physics2D.Raycast(worldPoint, Vector2.zero, Mathf.Infinity, stoneLayerMask);

                if (hitStone)
                {
                    GameManager.Instance.MoveStone(hitStone.transform);
                }
                else if (hitPoint)
                {
                    GameManager.Instance.PlaceStone(hitPoint.transform);
                }
            }
        }
    }
}
