using Zenject;

public class InputInjector : MonoInstaller
{
    public override void InstallBindings()
    {   
        Container.Bind<IInputSystem>().To<KeyboardInput>().FromNew().AsSingle();
    }
}
