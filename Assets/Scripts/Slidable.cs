using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slidable : MonoBehaviour
{

    private GridObject gridObject;

    // Start is called before the first frame update
    void Start()
    {
        gridObject = GetComponent<GridObject>();

        if (gridObject == null)
        {
            Debug.LogError("Slidable does not have GridObject on same game object, which is required.", gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
