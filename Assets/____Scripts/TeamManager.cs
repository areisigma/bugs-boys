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

        private void Start()
        {
            nt = GetComponent<NetworkTeam>();
            nt.teamId = "red";

            spawnRed = NetworkManager.startPositions[0];
            spawnBlue = NetworkManager.startPositions[1];
        }

        private void Update()
        {
            if (NetworkClient.teamId == "red" && teamChoice == true)
            {
                nt.teamId = "red";

                PlayerTeamModel(1); // Red Model

                // Set position and rotation of spawn
                playerPosition.position = spawnRed.position;
                playerPosition.rotation = spawnRed.rotation;

                teamChoice = false;
            }
            if (NetworkClient.teamId == "blue" && teamChoice == true)
            {
                nt.teamId = "blue";

                PlayerTeamModel(0); // Blue Model

                // Set position and rotation of spawn
                playerPosition.position = spawnBlue.position;
                playerPosition.rotation = spawnBlue.rotation;

                teamChoice = false;
            }

            if (NetworkClient.teamId != nt.teamId)
                teamChoice = !teamChoice;
        }

        void PlayerTeamModel(int k)
        {
            switch (k)
            {
                case 1:
                    skinRed.SetActive(true);
                    skinBlue.SetActive(false);
                    break;
                case 0:
                    skinRed.SetActive(false);
                    skinBlue.SetActive(true);
                    break;
            }
        }
    }
}