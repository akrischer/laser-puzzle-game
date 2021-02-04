using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInputHandler : InputHandler
{
    public bool HasInputEnded()
    {
        return Input.GetMouseButtonUp(0);
    }

    public bool IsInputDown()
    {
        return Input.GetMouseButton(0);
    }

    public Vector2 GetInputScreenPosition()
    {
        return Input.mousePosition;
    }
}
