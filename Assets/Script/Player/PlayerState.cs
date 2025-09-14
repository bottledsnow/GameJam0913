using UnityEngine;
using System;
using Movement;
public class PlayerState : MonoBehaviour
{
    [SerializeField] private ColorStateChange Up;
    [SerializeField] private ColorStateChange Down;
    [SerializeField] private ColorStateChange Left;
    [SerializeField] private ColorStateChange Right;

    private PlayerMovement playeymovement;
    //event
    

    public void ChangeDirection(Move move)
    {
        if(move ==Move.Left)
        {
            Left.ColorToGreen();
            Right.ColorToRed();
            Up.ColorToRed();
            Down.ColorToRed();
        }
        else if(move ==Move.Right)
        {
            Right.ColorToGreen();
            Left.ColorToRed();
            Up.ColorToRed();
            Down.ColorToRed();
        }
        else if(move ==Move.Up)
        {
            Up.ColorToGreen();
            Right.ColorToRed();
            Left.ColorToRed();
            Down.ColorToRed();
        }
        else if(move ==Move.Down)
        {
            Down.ColorToGreen();
            Right.ColorToRed();
            Up.ColorToRed();
            Left.ColorToRed();
        }
    }
}
