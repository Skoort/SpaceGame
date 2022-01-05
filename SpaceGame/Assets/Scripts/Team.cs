using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceGame
{ 
    public enum Team
    {
        HUMANS,
        PIRATES,  // The player will be moved to this team if they team kill too often. This team is hostile to all teams.
        ALIENS
    }
}
