using System;
using System.Collections.Generic;
using UnityEngine;


/*
 * Defines list of events that for unlocking abilities
 * 
 * To subscribe to one of these events in another script simply do:
 *  UnlockFunctions.exampleEvent += myFunc;
 *  
 * If you want to envoke one of these events with code in another script do: 
 *  UnlockFunctions.exampleEvent();
 * Alternatively use getAction(PossibelUpgrade) with the corresponding enum to get the event (for ScriptableObjects) 
 * 
 * To create a new Unlock event, ex. Roll ability
 * 1.) create a new field for the event:
 *  public static even Action Roll;
 * 2.) add the event to the Unlockable enum
 *  Dash, Slide, Roll
 * 3.) add case in getAction:
 *  case Unlockable.Roll:
 *  return Dash:
 */
public static class UnlockFunctions
{

    public static event Action UnlockDashEvent;
    public static event Action UnlockSlideEvent;
    public static event Action UnlockGrappleEvent;
    public static event Action UnlockWallRunEvent;

    public enum Unlockable
    {
        DASH, SLIDE, GRAPPLE, WALLRUN
    }

    public static Action getAction(Unlockable u)
    {
        switch (u)
        {
            case Unlockable.DASH:
                return UnlockDashEvent;
            case Unlockable.SLIDE:
                return UnlockSlideEvent;
            case Unlockable.GRAPPLE:
                return UnlockGrappleEvent;
            case Unlockable.WALLRUN:
                return UnlockWallRunEvent;
            default:
                Debug.LogError($"Unlockable case {u} not defined");
                return null;
        }
    }

    public static void callAction(Unlockable u) { 
        Action action = getAction(u);
        if (action != null)
        {
            action?.Invoke();
        }
    }
}
