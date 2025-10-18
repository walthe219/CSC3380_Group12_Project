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

    public static event Action Dash;
    public static event Action Slide;
    public static event Action Grapple;
    public static event Action WallRun;

    public enum Unlockable
    {
        Dash, Slide, Grapple, WallRun
    }

    public static Action getAction(Unlockable u)
    {
        switch (u)
        {
            case Unlockable.Dash:
                return Dash;
            case Unlockable.Slide:
                return Slide;
            case Unlockable.Grapple:
                return Grapple;
            case Unlockable.WallRun:
                return WallRun;
            default:
                Debug.LogError("");
                return null;
        }
    }
}
