namespace Enums
{
    public enum EInputAction
    {
        Move,
        Climb,
        Sprint,
        Jump,
        Crouch,
        Shoot,
        AimStickX,
        AimStickY,
        Aim,
        Normal,
        Blind,
        Paranoid
    }

    public enum EMovementState
    {
        Idle,
        Walking,
        Climbing,
        Jumping,
        Crouching,
        Sprinting
    }

    public enum EPlayerState
    {
        Normal,
        Shooting
    }
    
    public enum EArrowType
    {
        Normal,
        Blind,
        Paranoid
    }

    public enum EWayPointType
    {
        ResetTo0,
        GoBack
    }
}