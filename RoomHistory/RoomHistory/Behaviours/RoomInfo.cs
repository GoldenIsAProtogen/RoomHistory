using System;
using System.Collections.Generic;
using GorillaNetworking;
using Photon.Pun;

namespace RoomHistory.Behaviours
{
    internal class RoomInfo
    {
        public class RoomLogging : MonoBehaviourPunCallbacks
        {
            public static RoomLogging Instance;
            public List<string> JoinedRooms = new List<string>();
            public List<string> RoomGameModes = new List<string>();
            public List<string> Times = new List<string>();

            void Start() => Instance = this;

            public override void OnJoinedRoom()
            {
                string _TimeStampJoin = DateTime.Now.ToString("HH:mm");
                string _Code = NetworkSystem.Instance.RoomName;
                string _CurrentGameMode = GorillaComputer.instance.currentGameMode.ToString();
                string _FinalGameMode = "";

                _FinalGameMode = $"{(_CurrentGameMode.Contains("MODDED") ? $"Modded({_CurrentGameMode[7]})" : _CurrentGameMode)}";
                RoomGameModes.Add(_FinalGameMode);
                JoinedRooms.Add(_Code);
                Times.Add(_TimeStampJoin);
            }
        }
    }
}
