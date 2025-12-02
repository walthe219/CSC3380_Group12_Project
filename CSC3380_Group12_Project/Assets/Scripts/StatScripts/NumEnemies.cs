using UnityEngine;
using TMPro;

public class NumEnemies : MonoBehaviour
{
    [SerializeField] TMP_Text enemiesText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RoomManager.Instance.PassEnemiesAlive += updateEnemies;
    }

    public void updateEnemies(int num)
    {
        enemiesText.text = "Enemies: " + num;
    }
}
