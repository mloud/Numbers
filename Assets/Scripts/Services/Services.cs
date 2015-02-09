using System;
using System.Collections.Generic;



namespace Srv
{
    public class Services
    {
        private List<Service> ServicesList { get; set; }

        public Services()
        {
            ServicesList = new List<Service>();
        }

        public void RegisterService(Service service)
        {
            Core.Dbg.Assert(ServicesList.Find(x => x.GetType() == service.GetType()) == null, "Services.RegisterService() service of this type already registered " + service.GetType());

            if (ServicesList.Find(x => x.GetType() == service.GetType()) == null)
            {
                ServicesList.Add(service);
            }
        }

        public void UnregisterService(Type type)
        {
            Core.Dbg.Assert(ServicesList.Find(x => x.GetType() == type) != null, "Services.RegisterService() service of this type not found " + type);

            ServicesList.RemoveAt(ServicesList.FindIndex(x => x.GetType() == type));
        }


        public T GetService<T>() where T : Service
        {
            return ServicesList.Find(x => x.GetType() == typeof(T)) as T;
        }
    }
}