using System.Collections.Generic;
using System.Linq;
using GorillaInfoWatch.Attributes;
using GorillaInfoWatch.Models;
using GorillaInfoWatch.Models.Widgets;
using GorillaNetworking;
using Photon.Pun;
using UnityEngine;
using static RoomHistory.Behaviours.RoomInfo;

namespace RoomHistory.Screen
{
    internal class RoomHistory : MonoBehaviour
    {
        [ShowOnHomeScreen(DisplayTitle = "Room History")]
        public class RoomHistoryScreen : InfoWatchScreen
        {
            public override string Title => "Room History";

            public override string Description => (RoomLogging.Instance.JoinedRooms.Any()) ? $"Room's joined this session: {RoomLogging.Instance.JoinedRooms.Count}" : "No room's joined yet.";

            public override ScreenContent GetContent()
            {
                var lines = new LineBuilder();

                if (RoomLogging.Instance != null && RoomLogging.Instance.JoinedRooms.Any())
                {
                    for (int i = 0; i < RoomLogging.Instance.JoinedRooms.Count; i++)
                    {
                        string _RoomName = RoomLogging.Instance.JoinedRooms[i];
                        string _GameMode = RoomLogging.Instance.RoomGameModes[i];
                        string _TimeStamp = RoomLogging.Instance.Times[i];

                        lines.Add($"{_RoomName} : {_GameMode} : Joined at: {_TimeStamp}",
                            new List<Widget_Base> { new Widget_PushButton(() => SendPlayerToThatRoom(_RoomName)) });
                    }
                }
                else
                {
                    lines.AppendLine("");
                }

                return lines;
            }

            void SendPlayerToThatRoom(string _RoomCode)
            {
                PhotonNetworkController.Instance.AttemptToJoinSpecificRoom(_RoomCode, 0);
            }
        }
    }
}
