using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoRegistrable
{

    private void Awake()
    {
        ServiceLocator.Register(this);
    }

}
