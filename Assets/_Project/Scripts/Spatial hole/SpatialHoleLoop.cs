using System.Collections.Generic;
using UnityEngine;
using System;

namespace Game.SpatialHole
{
    [Serializable]
    public struct SpatialHoleIteration
    {
        public SpatialHoleTrigger trigger;
        public SpatialHoleRoom roomHole;
        public SpatialHole hole;
    }
    
    public class SpatialHoleLoop : MonoBehaviour
    {
        [SerializeField] private SpatialHoleIteration[] spatialHoles;
        
        private Dictionary<SpatialHoleTrigger,int> _spatialHoles;

        private void Start()
        {
            var index = 0;
            _spatialHoles = new(spatialHoles.Length);
            foreach (var spatialHoleIteration in spatialHoles)
            {
                var trigger = spatialHoleIteration.trigger;
                trigger.OnTrigger += Active;
                _spatialHoles[trigger] = index;
                index++;
            }
        }

        private void Active(SpatialHoleTrigger trigger)
        {
            trigger.OnTrigger -= Active;
            var index = _spatialHoles[trigger];
            _spatialHoles.Remove(trigger);
            spatialHoles[index].roomHole.SetHole(spatialHoles[index].hole);
        }
    }
}