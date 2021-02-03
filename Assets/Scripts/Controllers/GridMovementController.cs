using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMovementController : MonoBehaviour
{
    private Grid grid;
    private List<Slidable> slidableObjects;

    // Start is called before the first frame update
    void Start()
    {
        grid = GetComponent<Grid>();

        if (grid == null)
        {
            Debug.LogError("GridMovementController requires Grid script on same object", gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
