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
    }

    void UpdateMoveState()
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

    void DeltaMove()
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

            slidable.transform.Translate(moveDirection * .8f * Time.deltaTime);
        }
        else if (movementState == MovementState.Stopped && slidable != null)
        {
            Debug.Log("resetting position");
            Vector3 gridColliderPos = gridColliderUnderSlidable.transform.position;
            slidable.transform.position = new Vector3(gridColliderPos.x, gridColliderPos.y, slidable.transform.position.z);
        }
    }

    void Cleanup()
    {
        if (movementState == MovementState.Stopped)
        {
            gridColliderUnderSlidable = null;
            slidable = null;
        }
    }
}
