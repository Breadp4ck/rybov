using System;

namespace Units
{
    public interface IStunnable
    {
        TimeSpan StunDuration { get; }
        TimeSpan StaggerDuration { get; }
        
        void Stun(TimeSpan duration);
    }
}