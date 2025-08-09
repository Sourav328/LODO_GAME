using UnityEngine;
using System;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public string playerId;
    public List<TokenMover> tokens;
    public Dice_Controller dice;

    private int rolledNumber;
    private Action<bool> moveCompleteCallback;

    private void Start()
    {
        foreach (var token in tokens)
            token.owner = this;
    }

    public void StartTurn()
    {
        rolledNumber = 0;
        dice.EnableDice(true);

        
        foreach (var token in tokens)
            token.SetSelectable(false);
    }

    public void HandleDiceRoll(int number, Action<bool> onComplete)
    {
        rolledNumber = number;
        moveCompleteCallback = onComplete;

        bool canMove = false;
        foreach (var token in tokens)
        {
            if (token.CanMove(rolledNumber))
            {
                token.SetSelectable(true);
                canMove = true;
            }
            else
                token.SetSelectable(false);
        }

        if (!canMove)
            moveCompleteCallback?.Invoke(false);
        
    }

    
    public void OnTokenSelected(TokenMover token)
    {
        
        if (!token.IsSelectable) return;

        
        foreach (var t in tokens)
            t.SetSelectable(false);

        if (token.IsHome && rolledNumber == 6)
            token.LeaveHome(OnMoveComplete);
        else
            token.MoveSteps(rolledNumber, OnMoveComplete);
    }

    private void OnMoveComplete()
    {
        moveCompleteCallback?.Invoke(true);
    }
}
