using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCollider : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private int column;
    [SerializeField]
    private int row;
    private bool hasPositionSet = false;

    public Vector2 GetPosition
    {
        get
        {
            return new Vector2(row, column);
        }
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPosition(int row, int column)
    {
        if (hasPositionSet)
        {
            Debug.LogWarning("Trying to set position of GridCollider which already has position set at " + new Vector2(row, column) + ".");
        }
        else
        {
            this.row = row;
            this.column = column;
        }
    }
}
