using System.Collections.Generic;
using UnityEngine;


namespace PVG
{
    [CreateAssetMenu(fileName = "VineSettings", menuName = "ScriptableObjects/Settings/VineSettings")]
    public class VineSettings : ScriptableObject
    {
        public List<ForceDefinition> forcesDef = new();
        public float maxLifeTime;
    }
}

