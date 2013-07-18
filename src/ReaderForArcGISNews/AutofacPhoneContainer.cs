namespace ReaderForArcGISNews
{
    using System;
    using Caliburn.Micro;

    public class AutofacPhoneContainer : IPhoneContainer
    {
        public event Action<object> Activated;

        public void RegisterWithAppSettings(Type service, string appSettingsKey, Type implementation)
        {
        }

        public void RegisterWithPhoneService(Type service, string phoneStateKey, Type implementation)
        {
        }
    }
}
