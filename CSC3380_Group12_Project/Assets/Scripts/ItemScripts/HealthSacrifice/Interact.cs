using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Interact : MonoBehaviour
{
    InteractEvent interact = new InteractEvent();
    Player player;

    public InteractEvent GetinteractEvent{
        get{
            if(interact == null) interact = new InteractEvent();
            return interact;
        }
    }

    public Player GetPlayer{
        get{
            return player;
        }
    }

    public void CallInteract(Player InteractedPlayer){
        player = InteractedPlayer;
        interact.CallInteractEvent();
    }
}

public class InteractEvent{
    public delegate void InteractHandler();

    public event InteractHandler HasInteracted;

    public void CallInteractEvent() => HasInteracted?.Invoke();
}