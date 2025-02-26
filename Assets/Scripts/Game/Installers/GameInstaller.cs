using Game.Pool;
using Scripts.Game.Controllers;
using Scripts.User;
using Zenject;

namespace Scripts.Game.Installers
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<SelectionController>().AsSingle().NonLazy();
            Container.Bind<UserProgressDataManager>().AsSingle().NonLazy();
            Container.Bind<Spawner>().FromComponentInHierarchy().AsSingle();
            Container.Bind<EnemyController>().FromComponentInHierarchy().AsSingle();
            GameConstants.Initialize();
        }
    }
}