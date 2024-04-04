using Inputs;
using Zenject;

public class InputSystemInstaller : MonoInstaller
{
    public override void InstallBindings()
    {   
        Container.Bind<IInputSystem>().To<KeyboardInput>().AsSingle();
    }
}
