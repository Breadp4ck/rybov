namespace Units.Hitting
{
    public enum HitType
    {
        /// <summary>
        /// hot enough power to snap
        /// </summary>
        None,
        
        /// <summary>
        /// Lowest possible hit
        /// </summary>
        Slap,
        
        /// <summary>
        /// Last possible hit before kick out the game field
        /// </summary>
        Snap,
        
        /// <summary>
        /// Kick out the game field
        /// </summary>
        GigaSnap
    }
}