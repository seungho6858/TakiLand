public enum BetPreset
{
    Reset = 0,
    Bet1 = 1,
    Bet10 = 10,
    Bet100 = 100,
    Bet1000 = 1000,
}

public enum Team
{
    Red,
    Blue,
    None
}

public enum SpecialAction
{
    None,
    Explosion,
    Taunt,
    Invisibility,
    Rage,
    Greed,
    Split,
    Fear,
    CounterAttack,
    Immobility,
    SpeedBoost
}

public enum GameState
{
    Ready,
    
    Battle,
    
    End
}