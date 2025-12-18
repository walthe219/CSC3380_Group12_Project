using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BubbleStatsInventory : MonoBehaviour
{
    public int NumberOfBubbleStats {get; private set;}

    public void BubbleStatCollected(){
        NumberOfBubbleStats++;
    }
}
