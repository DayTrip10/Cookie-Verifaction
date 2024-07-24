using MelonLoader;
using UnityEngine;

[assembly: MelonInfo(typeof(Everyday_Blower.BuildInfo), Everyday_Blower.BuildInfo.Name, Everyday_Blower.BuildInfo.Version, Everyday_Blower.BuildInfo.Author)]
[assembly: MelonGame("Stress Level Zero", "BONELAB")]

namespace Everyday_Blower
{
    public static class BuildInfo
    {
        public const string Name = "Everyday Blower";
        public const string Author = "DayTrip";
        public const string Version = "1.0.0";
    }

    public class Class1 : MelonMod
    {
    }
}
