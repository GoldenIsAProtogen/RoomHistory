using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx;
using GorillaInfoWatch.Attributes;
using GorillaInfoWatch.Models;
using GorillaInfoWatch.Models.Widgets;
using GorillaNetworking;
using HarmonyLib;
using Photon.Pun;
using UnityEngine;

[assembly: InfoWatchCompatible]


namespace RoomHistory
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {

        void Start()
        {
            GorillaTagger.OnPlayerSpawned(GameStartFunction);
        }

        void GameStartFunction()
        {
            if (RoomLogging.Instance == null)
            {
                GameObject RoomManagerObj = new GameObject("RoomHistoryManager");
                RoomManagerObj.AddComponent<RoomLogging>();
            }
        }
    }

    public class RoomLogging : MonoBehaviourPunCallbacks
    {
        public static RoomLogging Instance;

        public List<string> JoinedRooms = new List<string>();
        public List<string> RoomGameModes = new List<string>();
        public List<string> Times = new List<string>();

        void Start()
        {
            Instance = this;
        }

        public override void OnJoinedRoom()
        {
            string timethelobbywasjoined = DateTime.Now.ToString("HH:mm");
            string code = NetworkSystem.Instance.RoomName;
            string currentgamemode = GorillaComputer.instance.currentGameMode.ToString();
            string gamemodetoadd = "";

             gamemodetoadd = $"{(currentgamemode.Contains("MODDED") ? $"({currentgamemode[7]})Modded" : currentgamemode)}";
             RoomGameModes.Add(gamemodetoadd);
             JoinedRooms.Add(code);
             Times.Add(timethelobbywasjoined);
        }
    }

    [ShowOnHomeScreen(DisplayTitle = "Room History")]
    public class RoomHistoryScreen : InfoWatchScreen
    {
        public override string Title => "Room History";

        public override string Description => (RoomLogging.Instance.JoinedRooms.Any()) ? $"{RoomLogging.Instance.JoinedRooms.Count} lobby / lobbies have been joined this session" : "No rooms joined yet.";

        public override ScreenContent GetContent()
        {
            var lines = new LineBuilder();

            if (RoomLogging.Instance != null && RoomLogging.Instance.JoinedRooms.Any())
            {
                for (int i = 0; i < RoomLogging.Instance.JoinedRooms.Count; i++)
                {
                    string room = RoomLogging.Instance.JoinedRooms[i];
                    string gamemode = RoomLogging.Instance.RoomGameModes[i];
                    string timestamp = RoomLogging.Instance.Times[i];

                    lines.Add($"{room} : {gamemode} : JOINED AT: {timestamp}",
                        new List<Widget_Base> { new Widget_PushButton(() => SendPlayerToThatRoom(room, gamemode)) });
                }
            }
            else
            {
                lines.AppendLine("");
            }

            return lines;
        }

        void SendPlayerToThatRoom(string roomcode, string gamemode)
        {
            Debug.Log($"SendingPlayerToRoom:");
            if (PhotonNetwork.InRoom)
                PhotonNetwork.LeaveRoom();
            PhotonNetworkController.Instance.AttemptToJoinSpecificRoom(roomcode, 0);
        }
    }
}
