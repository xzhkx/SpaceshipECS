
using UnityEngine;
using TMPro;
using Unity.Entities;

public class UIControlManager : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText;
    private MisileMovementSystem misileMovementSystem;

    private void OnEnable()
    {
        Debug.Log("ETgGE");
        misileMovementSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<MisileMovementSystem>();
        healthText.text = "30";
        misileMovementSystem.UpdatePlayerHealth += OnUpdatePlayerHealth;
    }

    private void OnDisable()
    {
        misileMovementSystem.UpdatePlayerHealth -= OnUpdatePlayerHealth;
    }

    private void OnUpdatePlayerHealth(int health)
    {
        healthText.text = health.ToString();
    }
}
