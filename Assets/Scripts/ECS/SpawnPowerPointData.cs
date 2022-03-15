using NaughtyAttributes;
using Unity.Entities;
using Unity.Properties.UI;
using UnityEngine;

namespace ShipSimulator
{
    [GenerateAuthoringComponent]
    public struct SpawnPowerPointData : IComponentData
    {
        public Entity PowerPoint;
        [MinMaxSlider(0, 3)]
        public Vector2 Delay;
    }
}