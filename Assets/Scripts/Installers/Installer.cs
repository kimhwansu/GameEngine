using UnityEngine;
using UJ.DI;

namespace UJ.Installers
{
    public abstract class Installer : MonoBehaviour
    {
        protected virtual void Awake()
        {
            var container = new DIContainer();
            DIContainer.AddContainer(container);
            InstallBindings(container);
        }

        protected abstract void InstallBindings(DIContainer container);

        protected virtual void OnDestroy()
        {
            DIContainer.RemoveContainer(DIContainer.diContainers[0]);
        }

        protected void RegisterComponent<T>(T component, string key = "") where T : class
        {
            if (component != null)
            {
                var container = new DIContainer();
                container.Regist(component, key);
                DIContainer.AddContainer(container);
            }
        }
    }
}
