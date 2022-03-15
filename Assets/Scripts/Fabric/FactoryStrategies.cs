using System;
using ShipSimulator;
using UnityEngine;
using Object = System.Object;

namespace DarckPower.Strategy
{
    public class FactoryStrategies
    {
        private GameObject _returnedObject;

        public FactoryStrategies(GameObject returnedObject)
        {
            _returnedObject = returnedObject;
        }

        public PowerPointData GetPowerPoint(float sustenance, float size)
        {
            var pp = _returnedObject.GetComponent<PowerPointData>();
            // if(pp == null)
                // throw new NullReferenceException("PowerPoint is missing");
            pp.Size = size;
            pp.Sustenance = sustenance;
            return pp;
        }
            
        
    }
}