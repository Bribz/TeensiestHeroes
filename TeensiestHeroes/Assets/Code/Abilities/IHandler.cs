using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHandler
{
    //These need to be more explicit
    void Initialize();
    void Activate();
    //

    void Cleanup();

    void Cancel();

    void Callback();

    
}
