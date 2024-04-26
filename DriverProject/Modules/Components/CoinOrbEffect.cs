using System.Reflection;
using R2API;
using UnityEngine;
using Unity;
using RoR2;
using UnityEngine.Events;
using RoR2.Orbs;

namespace DriverMod.Modules.Components
{
    public class CoinOrbEffect : OrbEffect
    {
        private EventFunctions eventFunctions;
        private void Awake()
        {
            eventFunctions = GetComponent<EventFunctions>();
            onArrival.AddListener(Event);
        }
        
        private void Event()
        {
            eventFunctions.UnparentTransform(base.transform.Find("Trail"));
        }
    }
}
