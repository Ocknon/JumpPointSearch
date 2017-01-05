using UnityEngine;
using System.Collections;

public class Events { }

public class Event { }

#region Movement Events
public class JumpEvent : Event { }

public class GroundedEvent : Event { }

public class ForwardMovementEvent : Event { }

public class BackMovementEvent : Event { }

public class LeftMovementEvent : Event { }

public class RightMovementEvent : Event { }

public class SprintMovementEvent : Event { }

public class SprintEndEvent : Event { }

public class CrouchMovementEvent : Event { }

public class CrouchEndEvent : Event { }

public class NoInputEvent : Event { }
#endregion

public class PathTimerEvent : Event
{
    public int time;
    public PathTimerEvent(int ms)
    {
        time = ms;
    }
}