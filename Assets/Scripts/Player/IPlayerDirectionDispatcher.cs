using System;

public delegate void PlayerDirectionEventHandler(IPlayerDirectionDispatcher sender, PlayerDirectionEventArgs e);

public class PlayerDirectionEventArgs : EventArgs
{
    public bool PlayerFacingRight;

    public PlayerDirectionEventArgs(bool playerFacingRight)
    {
        PlayerFacingRight = playerFacingRight;
    }
}

public interface IPlayerDirectionDispatcher
{
    event PlayerDirectionEventHandler PlayerDirectionChanged;
}