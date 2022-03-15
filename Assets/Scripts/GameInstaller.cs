using ShipSimulator;
using UnityEngine.Serialization;
using Zenject;

public class GameInstaller : MonoInstaller
{
    // [FormerlySerializedAs("FoodCreator")] public PowerPointCreator powerPointCreator;
    public override void InstallBindings()
    {
        // Container.BindInstance(powerPointCreator)
        //     .AsSingle()
        //     .NonLazy();
    }
}