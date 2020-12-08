using System;
using System.Collections.Generic;
using System.Text;
using Prism.Events;

namespace ControllerProgrammer.ProgramForm.Internal {
    public class USBConnectedEvent : PubSubEvent { }
    public class USBDisconnectedEvent : PubSubEvent { }
    public class RecieveLogEvent : PubSubEvent<string> { }
    public class RecieveRecipeEvent : PubSubEvent<string> { }
    public class RecieveProgrammedEvent : PubSubEvent<string> { }
    public class RefreshDataEvent : PubSubEvent { }
}
