using Examples.ExampleServices;
using Services;
using UnityEngine;

namespace Examples
{
    public class UsageExample : MonoBehaviour
    {
        private MonoBehaviourBasedService _monoBehaviourBasedService;
        private ManuallyRegisteredMonoService _manuallyRegisteredMonoService;
        private void Start()
        {
            //Get registered service
            if (ServiceLocator.IsRegistered<MonoBehaviourBasedService>())
            {
                _monoBehaviourBasedService = ServiceLocator.Get<MonoBehaviourBasedService>();
                Debug.Log($"{nameof(MonoBehaviourBasedService)} has been registered and received!");
            }
            
            //Get service which you are not sure was registered and handle failure yourself
            try
            {
                _manuallyRegisteredMonoService = ServiceLocator.Get<ManuallyRegisteredMonoService>();
            }
            catch (ServiceLocatorException)
            {
                Debug.Log($"Oh no, {nameof(ManuallyRegisteredMonoService)} wasn't registered :(!");
                //Do some action
            }
            
            //Get service and if it doesn't exist create one. Use it carefully as some services may
            //require some configuration. Especially true for MonoBehaviour based services. 
            
            _manuallyRegisteredMonoService = ServiceLocator.Get<ManuallyRegisteredMonoService>(true);
            Debug.Log($"I don't care give me {nameof(ManuallyRegisteredMonoService)}!");
            
            //Then use your services
            _monoBehaviourBasedService.Work();
            _manuallyRegisteredMonoService.Work();
        }
    }
}
