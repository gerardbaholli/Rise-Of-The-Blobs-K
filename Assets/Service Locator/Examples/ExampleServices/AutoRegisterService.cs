using Attributes;
using Services;
using UnityEngine;

namespace Examples.ExampleServices
{
    /*
     * Marking a class with AutoRegisteredService attributes results in it
     * being automatically registered as a part of ServiceLocator's
     * [RuntimeInitializeOnLoadMethod] Initialize method.
     */
    [AutoRegisteredService]
    public class AutoRegisterService : IRegistrable
    {
        public void Work()
        {
            Debug.Log($"{GetType().Name} is performing action.");
        }
    }
}
