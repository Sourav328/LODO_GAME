using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Dice_Controller : MonoBehaviour
{
    public string playerId;           
    public Image diceImage;           
    public Button diceButton;         
    public Sprite[] diceFaces;        

    public float rollDuration = 1f;   
    public float rollSpeed = 0.05f;  

    private bool canRoll = false;

    private void Awake()
    {

        diceButton.onClick.AddListener(OnDiceClick);
        EnableDice(false);
    } 

    public void EnableDice(bool enable)
    {
        canRoll = enable;
        diceButton.interactable = enable;
    }

    private void OnDiceClick()
    {
        if (!canRoll) return;

        EnableDice(false); 
        StartCoroutine(RollAnimation());
    }

    private IEnumerator RollAnimation()
    {
        float elapsed = 0f;
        int finalNumber = 1;

        while (elapsed < rollDuration)
        {
            finalNumber = Random.Range(1, 7); 
            diceImage.sprite = diceFaces[finalNumber - 1];
            elapsed += rollSpeed;
            yield return new WaitForSeconds(rollSpeed);
        }

     
        diceImage.sprite = diceFaces[finalNumber - 1];

      
        GameManager.Instance.OnDiceRolled(playerId, finalNumber);
    }
}
