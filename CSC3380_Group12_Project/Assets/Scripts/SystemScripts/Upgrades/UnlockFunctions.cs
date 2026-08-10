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
    public static event Action UnlockOmniDashEvent;
    public static event Action UnlockGrappleEvent;
    public static event Action UnlockWallRunEvent;
    public static event Action UnlockAutoFireEvent;
    public static event Action UnlockLifeStealEvent;
    public static event Action UnlockCthulhuEvent;
    public static event Action UnlockExplosiveRounds;
    public static event Action UnlockGrenade;

    public enum Unlockable
    {
        DASH, OMNIDASH, SLIDE, GRAPPLE, WALLRUN, AUTOFIRE, LIFESTEAL, BETTERFIRERATE, CTHULHU, EXPLOSIVEROUNDS, GRENADE
    }

    public static Action getAction(Unlockable u)
    {
        switch (u)
        {
            case Unlockable.DASH:
                return UnlockDashEvent;
            case Unlockable.OMNIDASH:
                return UnlockOmniDashEvent;
            case Unlockable.SLIDE:
                return UnlockSlideEvent;
            case Unlockable.GRAPPLE:
                return UnlockGrappleEvent;
            case Unlockable.WALLRUN:
                return UnlockWallRunEvent;
            case Unlockable.AUTOFIRE:
                return UnlockAutoFireEvent;
            case Unlockable.LIFESTEAL:
                return UnlockLifeStealEvent;
            case Unlockable.CTHULHU:
                return UnlockCthulhuEvent;
            case Unlockable.EXPLOSIVEROUNDS:
                return UnlockExplosiveRounds;
            case Unlockable.GRENADE:
                return UnlockGrenade;
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
