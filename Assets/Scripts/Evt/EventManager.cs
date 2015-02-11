using System;
using System.Collections.Generic;

namespace Evt
{
    public class EventManager
    {
        private Action<Event> Listeners { get; set; }

        public EventManager()
        {}

        public void Register(Action<Event> action)
        {
            Listeners += action;
        }
        
        public void Unregister(Action<Event> action)
        {
            Listeners -= action;
        }

        public void Trigger(Event evt)
        {
            if (Listeners != null)
                Listeners(evt);
        }
    }
}
