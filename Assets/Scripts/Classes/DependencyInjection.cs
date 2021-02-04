using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DependencyInjection
{
    public static InputHandler GetInputHandler()
    {
        return new MouseInputHandler();
    }
}
