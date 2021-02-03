using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public int columns;
    public int rows;

    public Vector3 gridOriginWorldPos;
    public float gridCellDistance = .5f;

    public GameObject gridColliderPrefab;

    // [y][x]
    private GridCell[][] grid;

    private void Awake()
    {
        InitGrid();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void InitGrid()
    {
        grid = new GridCell[rows][];
        for (int r = 0; r < rows; r++)
        {
            grid[r] = new GridCell[columns];
            for (int c = 0; c < columns; c++)
            {
                GridCollider gridCollider = Instantiate(gridColliderPrefab, GetGridColliderWorldPos(r, c), Quaternion.identity).GetComponent<GridCollider>();
                gridCollider.transform.parent = gameObject.transform;
                gridCollider.SetPosition(r, c);
                grid[r][c] = new GridCell(gridCollider, null);
            }
        }
    }

    private Vector3 GetGridColliderWorldPos(int row, int column)
    {
        float newXPosition = gridOriginWorldPos.x + (column * gridCellDistance);
        float newYPosition = gridOriginWorldPos.y - (row * gridCellDistance);
        return new Vector3(newXPosition, newYPosition, gameObject.transform.position.z);
    }

    private class GridCell
    { 
        private GridCollider gridCollider;
        private Slidable slidable;
        public GridCell(GridCollider gridCollider, Slidable slidable)
        {
            this.gridCollider = gridCollider;
            this.slidable = slidable;
        }
    }
}
