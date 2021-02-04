using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInputHandler : InputHandler
{
    public bool HasInputEnded()
    {
        return Input.touchCount == 0 ||
            Input.GetTouch(0).phase == TouchPhase.Ended ||
            Input.GetTouch(0).phase == TouchPhase.Canceled;
    }

    public bool IsInputDown()
    {
        return Input.touchCount > 0;
    }

    public Vector2 GetInputScreenPosition()
    {
        return Input.GetTouch(0).position;
    }
}
