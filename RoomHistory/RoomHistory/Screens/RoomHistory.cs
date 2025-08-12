using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GorillaInfoWatch.Models.Attributes;
using GorillaInfoWatch.Models;
using GorillaInfoWatch.Models.Widgets;
using GorillaNetworking;
using Photon.Pun;
using UnityEngine;
using static RoomHistory.Behaviours.RoomInfo;
using System;

namespace RoomHistory.Screens
{
    internal class RoomHistory : MonoBehaviour
    {
        [ShowOnHomeScreen(DisplayTitle = "Room History")]
        public class RoomHistoryScreen : GorillaInfoWatch.Models.Screen
        {
            public override string Title => "Room History";

            public string TimeNotInRoom;
            private string lastTimeOut = "";
            private TimeSpan totalTimeNotInRoom = TimeSpan.Zero;
            private DateTime? currentOutOfRoomStart = null;

            public override string Description => (RoomLogging.Instance.JoinedRooms.Any()) ? $"Room's joined this session: {RoomLogging.Instance.JoinedRooms.Count}" : "No room's joined yet.";

            public override ScreenContent GetContent()
            {
                var lines = new LineBuilder();

                lines.Add($"Overall time not in room: {lastTimeOut}", new List<Widget_Base>());

                if (RoomLogging.Instance != null && RoomLogging.Instance.JoinedRooms.Any())
                {
                    for (int i = 0; i < RoomLogging.Instance.JoinedRooms.Count; i++)
                    {
                        string _RoomName = RoomLogging.Instance.JoinedRooms[i];
                        string _GameMode = RoomLogging.Instance.RoomGameModes[i];
                        string _TimeStamp = RoomLogging.Instance.Times[i];

                        lines.Add($"{_RoomName} : {_GameMode} : (Duration: {_TimeStamp})",
                            new List<Widget_Base> { new Widget_PushButton(() => SendPlayerToThatRoom(_RoomName)) });
                    }
                }
                else
                {
                    lines.AppendLine("");
                }

                return lines;
            }
            void Start()
            {
                StartCoroutine(CheckIfInRoom());
            }
            void SendPlayerToThatRoom(string _RoomCode)
            {
                PhotonNetworkController.Instance.AttemptToJoinSpecificRoom(_RoomCode, 0);
            }

            private IEnumerator CheckIfInRoom()
            {
                while (true)
                {
                    if (!PhotonNetwork.InRoom)
                    {
                        if (!currentOutOfRoomStart.HasValue)
                        {
                            currentOutOfRoomStart = DateTime.Now;
                        }
                        TimeSpan currentDuration = DateTime.Now - currentOutOfRoomStart.Value;
                        TimeSpan displayTime = totalTimeNotInRoom + currentDuration;
                        lastTimeOut = $"{(int)displayTime.TotalHours:D2}:{displayTime.Minutes:D2}:{displayTime.Seconds:D2}";
                    }
                    else
                    {
                        if (currentOutOfRoomStart.HasValue)
                        {
                            totalTimeNotInRoom += DateTime.Now - currentOutOfRoomStart.Value;
                            currentOutOfRoomStart = null;
                        }
                        lastTimeOut = $"{(int)totalTimeNotInRoom.TotalHours:D2}:{totalTimeNotInRoom.Minutes:D2}:{totalTimeNotInRoom.Seconds:D2}";
                    }

                    yield return new WaitForSeconds(1f);
                }
            }
        }
    }
}
