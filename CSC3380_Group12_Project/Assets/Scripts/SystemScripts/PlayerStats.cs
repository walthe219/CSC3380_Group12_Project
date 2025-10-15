using UnityEngine;

public class PlayerStats : MonoBehaviour
{


    public int baseHealth = 100;
    public int baseStamina = 100;
    public int baseMagSize = 30;
    public int baseAmmo;

    public int currentHealth;
    public int currentStamina;
    public int currentMagSize;
    public int currentAmmo;

    public enum upgradeTypesToApply {HealthUp, AmmoUp, MagUp, StamUp}

    public UpgradeData1 HealthUp;
    public UpgradeData1 AmmoUp;
    public UpgradeData1 MagUp;
    public UpgradeData1 StamUp;

    public void applyUpgradeSimpleStats(upgradeTypesToApply upgrade){
        switch(upgrade){
            case upgradeTypesToApply.HealthUp:
                currentHealth += HealthUp.deltaHealth;
                break;
            case upgradeTypesToApply.StamUp:
                currentStamina += StamUp.deltaStamina;
                break;
            case upgradeTypesToApply.MagUp:
                currentMagSize += MagUp.deltaMag;
                break;
            case upgradeTypesToApply.AmmoUp:
                currentAmmo += AmmoUp.deltaAmmo;
                break;
            default:
                Debug.LogWarning(upgrade + " is not a valid upgrade.");
                break;

        }
    }

    public void applyEventUpgrdDeltaVal(){
        //Put this into a event that will invoke all functions associated to an upgrade depending on what upgrade it is
            //You can ivoke the upgradetype and the deltaval and then playerstats will process what stat accords to the upgrade and add the deltaval to it

    }
    

    public void Start(){
        currentHealth = baseHealth;
        currentStamina = baseStamina;
    }

    private void OnEnable(){
        
    }

    private void OnDisable(){
       
    }

    private void PlayerStatUpgrdMngr(string upgrade){
        

        
    }
}
