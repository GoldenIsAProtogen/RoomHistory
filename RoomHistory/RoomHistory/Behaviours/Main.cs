using Photon.Pun;
using UnityEngine;

namespace RoomHistory.Behaviours
{
    internal class Main : MonoBehaviour
    {
        void Start() => PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "RoomHistory", Constants.Version } });
    }
}
