using Game.Pool;
using Scripts.Game.Controllers;
using Zenject;

namespace Scripts.Game.Installers
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<SelectionController>().AsSingle().NonLazy();
            Container.Bind<Spawner>().FromComponentInHierarchy().AsSingle();
            GameConstants.Initialize();
        }
    }
}