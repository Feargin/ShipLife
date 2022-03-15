using DarkPower.Infrastructure;
using Enums;
using Fabric;
using UnityEngine;
using Zenject;

namespace ShipSimulator
{
    public class Bootstrapper : MonoInstaller
    {
        private IStateMachine _stateMachine;
        [SerializeField]
        private GameObject _fadeObject;

        private GameObject _fade;

        public override void InstallBindings()
        {
            _stateMachine = new GlobalStateMachine();

            Container.Bind<IStateMachine>()
                .WithId(StateMachines.Global)
                .FromInstance(_stateMachine)
                .AsSingle();

            // Container.Bind<IPrefabFactory>()
            //     .To<MonoFactory>()
            //     .AsSingle();
            
            Container.Bind<LevelLoader>()
                .FromNew()
                .AsSingle()
                .NonLazy();

            Container.Bind<FadeOnDestroy>()
                .FromNewComponentOnNewPrefab(_fadeObject)
                .AsSingle()
                .NonLazy();
        }

        private void Awake()
        {
            _stateMachine.SetState<BootstrapState>();
        }
    }
}