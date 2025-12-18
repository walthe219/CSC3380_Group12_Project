using UnityEngine;
using TMPro;

public class RoomTestUI : MonoBehaviour
{
    [SerializeField] TMP_Text rewardText;
    [SerializeField] TMP_Text enemiesText;

    private void Start()
    {
        changeReward("None");
        updateEnemies(0);
        RoomManager.PassUpgradeId += changeReward;
        RoomManager.PassEnemiesAlive += updateEnemies;

    }
    public void changeReward(string ID)
    {
        rewardText.text = "Reward: " + ID;
    }

    public void updateEnemies(int num)
    {
        enemiesText.text = "Enemies: " + num;
    }
}
