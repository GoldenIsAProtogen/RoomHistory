using System;
using System.Collections;
using System.Collections.Generic;
using GorillaInfoWatch.Behaviours;
using GorillaNetworking;
using Photon.Pun;
using UnityEngine;
using RoomHistory.Screens;
using GorillaInfoWatch.Models;

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
            private Coroutine _DurationTimer;
            private DateTime _JoinTime;
            void Start() => Instance = this;

            public override void OnJoinedRoom()
            {
                _JoinTime = DateTime.Now;
                string _Code = NetworkSystem.Instance.RoomName;
                string _CurrentGameMode = GorillaComputer.instance.currentGameMode.ToString();
                string _FinalGameMode = "";

                _FinalGameMode = $"{(_CurrentGameMode.Contains("MODDED") ? $"Modded({_CurrentGameMode[7]})" : _CurrentGameMode)}";
                RoomGameModes.Add(_FinalGameMode);
                JoinedRooms.Add(_Code);
                Times.Add("00:00:00");

                _DurationTimer = StartCoroutine(UpdateDuration());
            }

            public override void OnLeftRoom()
            {
                StopCoroutine(UpdateDuration());
            }

            private IEnumerator UpdateDuration()
            {
                while (PhotonNetwork.InRoom)
                {
                    TimeSpan TimeInRoom = DateTime.Now - _JoinTime;
                    int hours = (int)TimeInRoom.TotalHours;
                    int minutes = TimeInRoom.Minutes;
                    int seconds = TimeInRoom.Seconds;
                    int last = Times.Count - 1;
                    if (last >= 0)
                    {
                        Times[last] = $"{hours:D2}:{minutes:D2}:{seconds:D2}";
                    }
                    yield return new WaitForSeconds(1f);
                }
            }

        }
    }
}
