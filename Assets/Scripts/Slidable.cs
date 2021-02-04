using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slidable : MonoBehaviour
{
    private GridObject gridObject;
    private Grid ownerGrid;

    // Start is called before the first frame update
    void Start()
    {
        gridObject = GetComponent<GridObject>();

        if (gridObject == null)
        {
            Debug.LogError("Slidable does not have GridObject on same game object, which is required.", gameObject);
        }

        ownerGrid = GetOwnerGrid();

        if (ownerGrid == null)
        {
            Debug.LogError("Slidable could not find its owning grid", gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Grid GetOwnerGrid()
    {
        RaycastHit hit;
        // Bit shift the index of the layer to get a bit mask
        int layerMask = 1 << Constants.GRID_LAYER;
        if (Physics.Raycast(transform.position, gridObject.direction, out hit, Mathf.Infinity, layerMask))
        {
            return hit.collider.GetComponent<Grid>();
        }
        else
        {
            return null;
        }
    }

    public bool BelongsTo(Grid grid)
    {
        return ownerGrid.Equals(grid);
    }
}
