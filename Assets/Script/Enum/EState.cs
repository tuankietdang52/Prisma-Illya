namespace Assets.Script.Enum
{
    /// <summary>
    /// State of Live Object
    /// </summary>
    public enum EState
    {
        Free,
        IsAttack,
        IsUsingSkill,
        IsKnockBack,
        Sneak,
        Dead,
        /// <summary>
        /// Enemy Only
        /// </summary>
        GetHit
    }
}
