using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapWallView : MapEntityView , ICollideProcessor
{
    public void HandleCollideTo(ICollideProcessor tgtColliderProcessor)
    {
        Log.Logic(LogLevel.Info, "<color=gray>HandleCollideTo------------------</color>");
    }

    public Entity GetProcessorEntity()
    {
        return null;
    }

    private void Awake()
    {
        
    }





}
