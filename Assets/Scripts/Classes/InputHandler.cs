using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface InputHandler
{
    bool IsInputDown();
    bool HasInputEnded();
    Vector2 GetInputScreenPosition();
}
