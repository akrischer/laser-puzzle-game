using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    public int columns;
    public int rows;

    public Vector3 gridOriginWorldPos;
    public float gridCellDistance = .5f;

    public GameObject gridColliderPrefab;
    public Vector3 gridObjectDirection;

    // [y][x]
    private GridCell[][] grid;

    private void Awake()
    {
        InitGrid();
    }

    
    public ISet<Direction> GetValidMoveDirections(int row, int column)
    {
        ISet<Direction> result = new HashSet<Direction>();

        ISet<Vector2> gridCellsToCheck = new HashSet<Vector2>();
        Vector2 left = new Vector2(row, column - 1);
        Vector2 right = new Vector2(row, column + 1);
        Vector2 up = new Vector2(row - 1, column);
        Vector2 down = new Vector2(row + 1, column);
        gridCellsToCheck.Add(left);
        gridCellsToCheck.Add(right);
        gridCellsToCheck.Add(up);
        gridCellsToCheck.Add(down);

        foreach (Vector2 gridCellToCheck in gridCellsToCheck)
        {
            if (gridCellToCheck.x < 0 || gridCellToCheck.x >= rows || gridCellToCheck.y < 0 || gridCellToCheck.y >= columns)
            {
                continue;
            }
            GridCell gridCell = grid[(int)gridCellToCheck.x][(int)gridCellToCheck.y];
            if (gridCell.gridObject != null)
            {
                if (gridCellToCheck == left)
                {
                    result.Add(Direction.Left);
                }
                else if(gridCellToCheck == right)
                {
                    result.Add(Direction.Right);
                }
                else if(gridCellToCheck == down)
                {
                    result.Add(Direction.Down);
                }
                else if(gridCellToCheck == up)
                {
                    result.Add(Direction.Up);
                }
            }
        }
        
        return result;
    }

    private void InitGrid()
    {
        grid = new GridCell[rows][];
        for (int r = 0; r < rows; r++)
        {
            grid[r] = new GridCell[columns];
            for (int c = 0; c < columns; c++)
            {
                Vector3 gridColliderPos = GetGridColliderWorldPos(r, c);
                GridCollider gridCollider = Instantiate(gridColliderPrefab, gridColliderPos, Quaternion.identity).GetComponent<GridCollider>();
                gridCollider.transform.parent = gameObject.transform;
                gridCollider.SetPosition(r, c);
                // Raycast to hit GridObject
                GridObject gridObject = GetGridObject(gridColliderPos);

                grid[r][c] = new GridCell(gridCollider, gridObject);
            }
        }
    }

    public GridObject GetGridObject(Vector3 origin)
    {
        RaycastHit hit;
        int gridObjectLayer = 1 << Constants.GRID_OBJECT_LAYER;
        Physics.Raycast(origin, gridObjectDirection, out hit, float.MaxValue, gridObjectLayer);
        return hit.collider == null ? null : hit.collider.GetComponent<GridObject>();
    }

    private Vector3 GetGridColliderWorldPos(int row, int column)
    {
        float newXPosition = gridOriginWorldPos.x + (column * gridCellDistance);
        float newYPosition = gridOriginWorldPos.y - (row * gridCellDistance);
        return new Vector3(newXPosition, newYPosition, gameObject.transform.position.z);
    }

    private class GridCell
    { 
        public GridCollider gridCollider;
        public GridObject gridObject;
        public GridCell(GridCollider gridCollider, GridObject gridObject)
        {
            this.gridCollider = gridCollider;
            this.gridObject = gridObject;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (gridObjectDirection * 3));
    }
}
