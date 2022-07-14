using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mirror
{
    public class TeamManager : NetworkBehaviour
    {
        [Header("Networking")]
        NetworkTeam nt;
        Transform spawnRed;
        Transform spawnBlue;
        bool teamChoice;

        [Header("Player")]
        public Transform playerPosition;
        public GameObject skinRed;
        public GameObject skinBlue;

        [SyncVar(hook = nameof(OnModelChange))]
        string skin;
        

        private void Start()
        {
            nt = GetComponent<NetworkTeam>();

            spawnRed = NetworkManager.startPositions[0];
            spawnBlue = NetworkManager.startPositions[1];
        }

        private void Update()
        {
            if (!hasAuthority) return;


            if (NetworkClient.teamId == "red" && teamChoice)
            {
                nt.teamId = "red";
                teamChoice = false;
                NetworkClient.sTeamChoice = false;

                Debug.Log("kupa");

                CmdModelChange();

                // Set position and rotation of spawn
                playerPosition.position = spawnRed.position;
                playerPosition.rotation = spawnRed.rotation;
            }
            if (NetworkClient.teamId == "blue" && teamChoice)
            {
                nt.teamId = "blue";
                teamChoice = false;
                NetworkClient.sTeamChoice = false;

                Debug.Log("kupa2");

                CmdModelChange();

                // Set position and rotation of spawn
                playerPosition.position = spawnBlue.position;
                playerPosition.rotation = spawnBlue.rotation;
            }

            // On teamId change above code will run only once. Inside if statements teamChoice is set to false
            if (NetworkClient.sTeamChoice == true)
                teamChoice = true;
        }

        [Command]
        void CmdModelChange()
        {
            skin = nt.teamId;
        }

        void OnModelChange(string _old, string _new)
        {
            //if (!isClient) return;
            
            switch (_new)
            {
                case "red":
                    skinRed.SetActive(true);
                    skinBlue.SetActive(false);
                    break;
                case "blue":
                    skinRed.SetActive(false);
                    skinBlue.SetActive(true);
                    break;
            }
        }
    }
}