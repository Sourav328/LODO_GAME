using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public PlayerController redPlayer;
    public PlayerController yellowPlayer;

    private int currentPlayerIndex = 0; 
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        StartTurn();
    }

    private void DisableAllDice()
    {
        redPlayer?.dice.EnableDice(false);
        yellowPlayer?.dice.EnableDice(false);
    }

    private void StartTurn()
    {
        DisableAllDice();

        if (currentPlayerIndex == 0)
            redPlayer.StartTurn();
        else
            yellowPlayer.StartTurn();
    }

    public void OnDiceRolled(string playerId, int rolledNumber)
    {
        DisableAllDice();

        if (playerId == redPlayer.playerId)
        {
            redPlayer.HandleDiceRoll(rolledNumber, moved =>
            {
                StartCoroutine(AfterMoveRoutine(rolledNumber, moved));
            });
        }
        else if (playerId == yellowPlayer.playerId)
        {
            yellowPlayer.HandleDiceRoll(rolledNumber, moved =>
            {
                StartCoroutine(AfterMoveRoutine(rolledNumber, moved));
            });
        }
    }

    private IEnumerator AfterMoveRoutine(int rolledNumber, bool moved)
    {
        yield return new WaitForSeconds(0.15f);

       
        if (rolledNumber != 6 || !moved)
            currentPlayerIndex = 1 - currentPlayerIndex;

        StartTurn();
    }
}
