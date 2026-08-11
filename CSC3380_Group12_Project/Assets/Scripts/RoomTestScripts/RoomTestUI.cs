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

        rewardText.gameObject.SetActive(false);
        enemiesText.gameObject.SetActive(false);

        RoomManager.PassUpgradeId += changeReward;
        RoomManager.PassEnemiesAlive += updateEnemies;
        RoomManager.RoomCleared += hideText;

    }
    public void changeReward(string ID)
    {
        rewardText.gameObject.SetActive(true);
        rewardText.text = "Reward: " + ID;
    }

    public void updateEnemies(int num)
    {
        enemiesText.gameObject.SetActive(true);
        enemiesText.text = "Enemies: " + num;
    }

    public void hideText()
    {
        enemiesText.gameObject.SetActive(false);
    }
}
