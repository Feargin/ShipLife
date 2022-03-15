using Unity.Entities;
using UnityEngine;

namespace ShipSimulator
{
    public class Ship : MonoBehaviour, IConvertGameObjectToEntity
    {
        public MoveData MoveData;
        public SearchData SearchData;
        public TargetData TargetData;
        public CapacityData CapacityData;
        private DirectionData _directionData;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, MoveData);
            dstManager.AddComponentData(entity, SearchData);
            dstManager.AddComponentData(entity, _directionData);
            dstManager.AddComponentData(entity, TargetData);
            dstManager.AddComponentData(entity, CapacityData);
        }
    }
}