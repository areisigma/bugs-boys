// vis2k: GUILayout instead of spacey += ...; removed Update hotkeys to avoid
// confusion if someone accidentally presses one.
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mirror
{
    /// <summary>Shows NetworkManager controls in a GUI at runtime.</summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Network/Network Manager HUD")]
    [RequireComponent(typeof(NetworkManager))]
    [HelpURL("https://mirror-networking.gitbook.io/docs/components/network-manager-hud")]
    public class NetworkManagerHUD : MonoBehaviour
    {
        [Header("Networking")]
        NetworkManager manager;
        GameObject player;

        [Header("Visual")]
        public int offsetX;
        public int offsetY;
        [Space]
        public int offsetXt = Screen.width / 2; // Team HUD offset
        public int offsetYt = Screen.height / 2; // 

        public bool isPaused = false;
        public bool teamChoice = true;


        void Awake()
        {
            manager = GetComponent<NetworkManager>();
            //Cursor.lockState = CursorLockMode.None;
        }

        private void Update()
        {
            if (NetworkClient.isConnected)
                player = NetworkClient.localPlayer.gameObject;

            // isPause -> pause menu
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                isPaused = !isPaused;
                teamChoice = false;

                if(isPaused)
                    FreeCursor();
                else
                    LockCursor();
            }

            // NetworkClient.teamChoice -> choosing team menu
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                teamChoice = !teamChoice;

                if (teamChoice)
                    FreeCursor();
                else
                    LockCursor();
            }

            NetworkClient.isPaused = isPaused || teamChoice;
        }

        void OnGUI()
        {
            NetworkHUD();
            TeamHUD();
        }

        void NetworkHUD()
        {
            GUILayout.BeginArea(new Rect(10 + offsetX, 40 + offsetY, 215, 9999));
            if (!NetworkClient.isConnected && !NetworkServer.active && !isPaused)
            {
                StartButtons();
            }
            else
            {
                if (isPaused)
                    StatusLabels();
            }

            // client ready
            if (NetworkClient.isConnected && !NetworkClient.ready)
            {
                if (GUILayout.Button("Client Ready"))
                {
                    NetworkClient.Ready();
                    if (NetworkClient.localPlayer == null)
                    {
                        NetworkClient.AddPlayer();
                    }
                }
            }

            if (isPaused)
                StopButtons();

            GUILayout.EndArea();
        }

        void StartButtons()
        {
            if (!NetworkClient.active)
            {
                // Server + Client
                if (Application.platform != RuntimePlatform.WebGLPlayer)
                {
                    if (GUILayout.Button("Host (Server + Client)"))
                    {
                        manager.StartHost();
                    }
                }

                // Client + IP
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Client"))
                {
                    manager.StartClient();
                }
                // This updates networkAddress every frame from the TextField
                manager.networkAddress = GUILayout.TextField(manager.networkAddress);
                GUILayout.EndHorizontal();

                // Server Only
                if (Application.platform == RuntimePlatform.WebGLPlayer)
                {
                    // cant be a server in webgl build
                    GUILayout.Box("(  WebGL cannot be server  )");
                }
                else
                {
                    if (GUILayout.Button("Server Only")) manager.StartServer();
                }
            }
            else
            {
                // Connecting
                GUILayout.Label($"Connecting to {manager.networkAddress}..");
                if (GUILayout.Button("Cancel Connection Attempt"))
                {
                    manager.StopClient();
                }
            }
        }

        void StatusLabels()
        {
            // host mode
            // display separately because this always confused people:
            //   Server: ...
            //   Client: ...
            if (NetworkServer.active && NetworkClient.active)
            {
                GUILayout.Label($"<b>Host</b>: running via {Transport.activeTransport}");
            }
            // server only
            else if (NetworkServer.active)
            {
                GUILayout.Label($"<b>Server</b>: running via {Transport.activeTransport}");
            }
            // client only
            else if (NetworkClient.isConnected)
            {
                GUILayout.Label($"<b>Client</b>: connected to {manager.networkAddress} via {Transport.activeTransport}");
            }
        }

        void StopButtons()
        {
            // stop host if host mode
            if (NetworkServer.active && NetworkClient.isConnected)
            {
                if (GUILayout.Button("Stop Host"))
                {
                    manager.StopHost();
                }
            }
            // stop client if client-only
            else if (NetworkClient.isConnected)
            {
                if (GUILayout.Button("Stop Client"))
                {
                    manager.StopClient();
                }
            }
            // stop server if server-only
            else if (NetworkServer.active)
            {
                if (GUILayout.Button("Stop Server"))
                {
                    manager.StopServer();
                }
            }
        }
        
        void TeamHUD()
        {
            if (teamChoice && NetworkClient.isConnected)
            {
                GUILayout.BeginArea(new Rect(10 + offsetXt, 200 + offsetYt, 375, 9999));

                GUILayout.BeginHorizontal();
                GUILayout.Box("Red Team");
                GUILayout.Box("Blue Team");
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Join Red"))
                {
                    NetworkClient.teamId = "red";

                    teamChoice = false;
                    isPaused = false;

                    LockCursor();
                }

                if (GUILayout.Button("Join Blue"))
                {
                    NetworkClient.teamId = "blue";

                    teamChoice = false;
                    isPaused = false;

                    LockCursor();
                }
                GUILayout.EndHorizontal();

                GUILayout.EndArea();
            }
        }
        
        void FreeCursor() // Using HUD
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        void LockCursor() // Playing Game
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

    }
}
