using Unity.Entities;

namespace ShipSimulator
{
    [GenerateAuthoringComponent]
    public struct MapData : IComponentData
    {
        public float SizeMapX;
        public float SizeMapY;
    }
}