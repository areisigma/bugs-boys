using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

namespace Mirror
{
    public class NetworkGameManager : MonoBehaviour
    {
        public int team = 1; // Red = 1 ; Blue = 0
        public int offsetX, offsetY;
        GameObject spawnRed, spawnBlue;

        void Start()
        {
            if (SceneManager.GetSceneByName("Game").buildIndex == 1)
            {
                Debug.Log("Scene is Game");


            }
        }

        void Update()
        {
            if (SceneManager.GetSceneByName("Game").buildIndex == 1)
            {

            }
        }

        void OnGUI()
        {
            if (SceneManager.GetSceneByName("Game").buildIndex == 1)
            {
                GUILayout.BeginArea(new Rect(10 + offsetX, 140 + offsetY, 215, 9999));

                if (GUILayout.Button("RED"))
                {
                    team = 1;
                }

                if (GUILayout.Button("BLUE"))
                {
                    team = 0;
                }

                GUILayout.TextField("Team: " + team.ToString());

                GUILayout.EndArea();
            }
        }
    }
}
