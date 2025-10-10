using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeData1", menuName = "Scriptable Objects/UpgradeData1")]
public class UpgradeData1 : ScriptableObject
{
    public UpgradeData1 HealthUp;

    public int deltaHealth;
    public int deltaAmmo;
    public int deltaStamina;

    //When player passes through 3D circle event listener where the event is just printing something and then the subscription function
    // is adding HealthUp.deltaHealth to current health. Pass deltaHealth through the invoke
}
