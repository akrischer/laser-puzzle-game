using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMovementController : MonoBehaviour
{
    public Vector3 raycastDirection;

    enum MovementState
    {
        Moving,
        Stopped
    }

    private MovementState movementState = MovementState.Stopped;
    private Slidable slidable;
    private GridCollider gridColliderUnderSlidable;

    private InputHandler inputHandler;
    private Grid grid;

    private ISet<Vector2> _debugValidMoveDirs = new HashSet<Vector2>();

    // Start is called before the first frame update
    void Start()
    {
        inputHandler = DependencyInjection.GetInputHandler();
        grid = GetComponent<Grid>();

        if (grid == null)
        {
            Debug.LogError("GridMovementController requires Grid script on same object", gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMoveState();
        DeltaMove();
        Cleanup();
    }

    private void OnDrawGizmos()
    {
        if (gridColliderUnderSlidable != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(gridColliderUnderSlidable.transform.position, .2f);
        }

        if (slidable != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(slidable.transform.position, raycastDirection);
        }

        if (slidable != null)
        {
            foreach (Vector2 validMoveDir in _debugValidMoveDirs)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(slidable.transform.position, new Vector3(validMoveDir.x, validMoveDir.y, slidable.transform.position.z) + slidable.transform.position);
            }
        }

    }

    private void UpdateMoveState()
    {
        //We check if we have more than one touch happening.
        //We also check if the first touches phase is Ended (that the finger was lifted)
        if (inputHandler.IsInputDown())
        {
            //We transform the touch position into word space from screen space and store it.
            Vector3 touchPosWorld = Camera.main.ScreenToWorldPoint(inputHandler.GetInputScreenPosition());

            RaycastHit slidableHit;
            // Does the ray intersect with any grid colliders?
            // Bit shift the index of the layer to get a bit mask
            int slidableLayerMask = 1 << Constants.SLIDABLE_LAYER;
            Physics.Raycast(touchPosWorld, raycastDirection, out slidableHit, float.MaxValue, slidableLayerMask);

            if (slidableHit.collider != null)
            {
                Slidable touchedSlidable = slidableHit.collider.GetComponent<Slidable>();

                // Early exit if THIS grid movement controller is not relevant.
                if (!touchedSlidable.BelongsTo(grid))
                {
                    return;
                }
                slidable = touchedSlidable;
                movementState = MovementState.Moving;
            }

            if (slidable != null)
            {
                // check if we should update our tracked grid collider
                int gridColliderLayerMask = 1 << Constants.GRID_COLLIDER_LAYER;
                RaycastHit gridColliderHit;
                Physics.Raycast(slidable.transform.position, raycastDirection, out gridColliderHit, float.MaxValue, gridColliderLayerMask);

                if (gridColliderHit.collider != null)
                {
                    gridColliderUnderSlidable = gridColliderHit.collider.GetComponent<GridCollider>();
                }
            }
        }
        if (inputHandler.HasInputEnded())
        {
            movementState = MovementState.Stopped;
            Debug.Log("input ended");
        }
    }

    private void DeltaMove()
    {
        if (movementState == MovementState.Moving && slidable != null)
        {
            Vector2 slidableScreenPos = Camera.main.WorldToScreenPoint(slidable.transform.position);
            Vector2 inputScreenPos = inputHandler.GetInputScreenPosition();
            // first find direction of movement
            Vector2 moveDirection = inputScreenPos - slidableScreenPos;

            // translate into up/down/left/right
            if (Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.y))
            {
                moveDirection = new Vector2(moveDirection.x, 0);
                moveDirection.Normalize();
            }
            else
            {
                moveDirection = new Vector2(0, moveDirection.y);
                moveDirection.Normalize();
            }

            // get valid moves
            ISet<Vector2> validMoveDirectionVectors = GetValidMoveDirectionVectors(gridColliderUnderSlidable.GetPosition);
            _debugValidMoveDirs = validMoveDirectionVectors;

            if (validMoveDirectionVectors.Contains(moveDirection))
            {
                Vector2 moveVector = moveDirection * .8f * Time.deltaTime;
                slidable.transform.Translate(moveVector);
            }
        }
        else if (movementState == MovementState.Stopped && slidable != null)
        {
            Vector3 gridColliderPos = gridColliderUnderSlidable.transform.position;
            slidable.transform.position = new Vector3(gridColliderPos.x, gridColliderPos.y, slidable.transform.position.z);
        }
    }

    private ISet<Vector2> GetValidMoveDirectionVectors(Vector2 gridPosition)
    {
        ISet<Vector2> result = new HashSet<Vector2>();
        ISet<Grid.Direction> validDirections = grid.GetValidMoveDirections((int)gridPosition.x, (int)gridPosition.y);
        foreach (Grid.Direction direction in validDirections)
        {
            switch (direction)
            {
                case Grid.Direction.Down:
                    result.Add(Vector2.down);
                    break;
                case Grid.Direction.Up:
                    result.Add(Vector2.up);
                    break;
                case Grid.Direction.Left:
                    result.Add(Vector2.left);
                    break;
                case Grid.Direction.Right:
                    result.Add(Vector2.right);
                    break;
            }
        }
        return result;
    }

    private void Cleanup()
    {
        if (movementState == MovementState.Stopped)
        {
            gridColliderUnderSlidable = null;
            slidable = null;
        }
    }

    private ISet<Vector3> ValidMoveDirections(GridObject currentGridObject)
    {
        return null;
    }
}
