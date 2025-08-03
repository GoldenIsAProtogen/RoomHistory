using BepInEx;
using GorillaInfoWatch.Attributes;
using GorillaInfoWatch.Behaviours;
using GorillaInfoWatch.Models;
using Photon.Pun;
using UnityEngine;
using static RoomHistory.Behaviours.RoomInfo;

[assembly: InfoWatchCompatible]
namespace RoomHistory
{
    [BepInPlugin(Constants.GUID, Constants.Name, Constants.Version)]
    public class Plugin : BaseUnityPlugin
    {
        
        void Start()
        {
           GorillaTagger.OnPlayerSpawned(() => new GameObject(Constants.Name, typeof(Screens.RoomHistory), typeof(RoomLogging), typeof(Main), typeof(Notification)));
        }
    }
}
