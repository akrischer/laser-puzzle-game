using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{

    public Vector3 direction;

    [SerializeField]
    private int row;
    [SerializeField]
    private int column;

    public Vector2 Position
    {
        get; set;
    }

    // Start is called before the first frame update
    void Start()
    {
        GetMyGridPosition();
        Position = new Vector2(row, column);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GetMyGridPosition()
    {
        RaycastHit hit;
        // Does the ray intersect with any grid colliders?
        // Bit shift the index of the layer to get a bit mask
        int layerMask = 1 << Constants.GRID_COLLIDER_LAYER;
        if (Physics.Raycast(transform.position, direction, out hit, Mathf.Infinity, layerMask))
        {
            Vector2 position = hit.collider.gameObject.GetComponent<GridCollider>().GetPosition;
            this.row = (int)position.x;
            this.column = (int)position.y;
        }
        else
        {
            Debug.LogError("GridObject could not set its own grid position.", gameObject);
        }
    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, direction);
    }
}
