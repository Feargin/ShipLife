using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace ShipSimulator
{
    [Serializable]
    [GenerateAuthoringComponent]
    public struct PowerPointData : IComponentData
    {
        public float Sustenance;
        public float Size;
        public float2 Position;
    }
}