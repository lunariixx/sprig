using DSharpPlus;
using Sprig.Utils.Logging;
using System;
using System.Linq;
using System.Reflection;

namespace Sprig.Modules
{
    public static class EventLoader
    {
        public static void Load(DiscordClient client)
        {
            var eventTypes = Assembly.GetExecutingAssembly().GetTypes()
                                     .Where(t => t.IsClass && t.IsAbstract && t.IsSealed
                                                 && t.Namespace == "Sprig.Events");

            foreach (var type in eventTypes)
            {
                foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Static))
                {
                    var parameters = method.GetParameters();

                    if (parameters.Length == 2 && parameters[0].ParameterType == typeof(DiscordClient))
                    {
                        var eventName = method.Name;
                        var evt = client.GetType().GetEvent(eventName);

                        if (evt?.EventHandlerType != null)
                        {
                            var del = Delegate.CreateDelegate(evt.EventHandlerType, method);
                            evt.AddEventHandler(client, del);
                            Logger.LogDebug(Logger.EventIds.ModuleLoader, $"Hooked event {eventName} from {type.FullName}.{method.Name}");
                        }
                        else
                        {
                            Logger.LogError(Logger.EventIds.ModuleLoader, $"Event {eventName} not found or has null handler type in {type.FullName}.{method.Name}");
                        }
                    }
                    else
                    {
                        Logger.LogWarn(Logger.EventIds.ModuleLoader, $"Skipped {type.FullName}.{method.Name}: parameter count/signature mismatch");
                    }
                }
            }
        }
    }
}
