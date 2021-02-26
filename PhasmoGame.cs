// WraithGUI by Karma Kitten
// For Phasmophobia build #5635423, manifest #4488252479481708564

using MelonLoader;
using PhasmoTestMod;
using UnityEngine;
using UnityEngine.InputSystem;

namespace WraithGUI
{
    public sealed class PhasmoGame : MelonMod
    {
        private static readonly GUIStyle GUIStyle = new GUIStyle();
        private GUIEx.DropdownState ghostDropdownState = new GUIEx.DropdownState();
        private GUIEx.DropdownState roomDropdownState = new GUIEx.DropdownState();
        public static GUIEx.DropdownState primaryDropdownState = new GUIEx.DropdownState();
        public static GUIEx.DropdownState secondaryDropdownState = new GUIEx.DropdownState();
        public static readonly GUIStyle backgroundStyle = new GUIStyle();

        private bool menuIsEnabled = false;
        private bool mainMenuIsEnabled = false;
        private bool playerMenuIsEnabled = false;
        private bool ghostMenuIsEnabled = false;
        private bool levelMenuIsEnabled = false;
        private bool listGhostsIsEnabled = false;
        private bool ghostsAlwaysVisible = false;
        private bool spawnMenuIsEnabled = false;
        private bool colorMenuIsEnabled = false;

        private string ghostNameEdit = "";
        private int levelNum;
        private float playerSpeed = 1.2f; // Default player speed

        private string[] roomNames;
        private static string[] mapNames = { "Opening Scene", "Lobby", "Tanglewood Street", "Asylum", "Edgefield Street House", "Ridgeview Road House", "Brownstone High School", "Bleasdale Farmhouse" , "Grafton Farmhouse" };
        private static string[] ghostTypeStrings = { "None", "Spirit", "Wraith", "Phantom", "Poltergeist", "Banshee", "Jinn", "Mare", "Revenant", "Shade", "Demon", "Yurei", "Oni"};
        private static GhostTraits.Type[] ghostTypesArr = { GhostTraits.Type.none, GhostTraits.Type.Spirit, GhostTraits.Type.Wraith, GhostTraits.Type.Phantom, GhostTraits.Type.Poltergeist, GhostTraits.Type.Banshee, GhostTraits.Type.Jinn, GhostTraits.Type.Mare, GhostTraits.Type.Revenant, GhostTraits.Type.Shade, GhostTraits.Type.Demon, GhostTraits.Type.Yurei, GhostTraits.Type.Oni };

        public static LevelController levelController;
        public static GhostController ghostController;
        public static Player player;
        public static SetupPhaseController setupPhaseController;

        public override void OnApplicationStart()
        {
            MelonLogger.Log($"Successfully loaded v{typeof(PhasmoGame).Assembly.GetName().Version}. Enjoy!");
            GUIStyle.alignment = TextAnchor.UpperCenter;
            GUIStyle.fontSize = 16;
            GUIStyle.fontStyle = FontStyle.Bold;
            backgroundStyle.alignment = TextAnchor.MiddleCenter;
            backgroundStyle.normal.textColor = Color.white;

            // Default menu colors. 3 = cyan, 5 = magenta
            primaryDropdownState.Select = 3;
            secondaryDropdownState.Select = 5;
        }
        public override void OnLevelWasInitialized(int level)
        {
            levelNum = level; // Save this for checks later.
            MelonLogger.Log($"{mapNames[level]} was initialized");
            InitializeGameObjects(); // By calling this here, it will ensure the level is never null

            if (level == 1)
            {
                GUIMenu.ghostText = "";
            }
            else if (level > 1)
            {
                roomNames = GetRoomStrings();
            }
        }
        public override void OnUpdate()
        {
            // Open Menu (F1)
            if (Keyboard.current.f1Key.wasPressedThisFrame && levelNum > 1)
                menuIsEnabled = !menuIsEnabled;

            GUIMenu.CycleColors(GUIStyle);
            GUIMenu.CycleColors(backgroundStyle, true);

            // This is for the speed slider, each frame set the applied speed from the slider
            if (levelNum > 1)
                player.firstPersonController.m_WalkSpeed = playerSpeed;

            // Always visible ghosts
            if (ghostsAlwaysVisible && levelNum > 1 && levelController.currentGhost != null)
            {
                foreach (GhostAI ghost in CustomGhostController.ghosts)
                {
                    if (!ghost.ghostIsAppeared)
                    {
                        ghost.Appear(true);
                    }
                }
            }
        }
        public override void OnGUI()
        {
            if (menuIsEnabled)
            {
                GUI.Box(new Rect(0, 0, Screen.width, 30), "");
                GUI.Box(new Rect(0, 5, Screen.width, 25), "WraithGUI", GUIStyle);

                if (GUI.Button(new Rect(5, 0, 50, 25), "Main", backgroundStyle))
                    mainMenuIsEnabled = !mainMenuIsEnabled;

                if (GUI.Button(new Rect(65, 0, 60, 25), "Maps", backgroundStyle)) { }

                if (GUI.Button(new Rect(135, 0, 65, 25), "Colors", backgroundStyle))
                    colorMenuIsEnabled = !colorMenuIsEnabled;

                if (GUI.Button(new Rect(210, 0, 65, 25), "Players", backgroundStyle)) { }

                if (mainMenuIsEnabled)
                {
                    playerMenuIsEnabled = false;
                    levelMenuIsEnabled = false;
                    ghostMenuIsEnabled = false;
                    colorMenuIsEnabled = false;

                    GUI.Box(new Rect(0, 31, 150, 250), "");
                    if (GUI.Button(new Rect(20, 40, 100, 25), "Player Menu", backgroundStyle))
                        playerMenuIsEnabled = !playerMenuIsEnabled;

                    if (GUI.Button(new Rect(20, 70, 100, 25), "Ghost Menu", backgroundStyle))
                        ghostMenuIsEnabled = !ghostMenuIsEnabled;
                    
                    if (GUI.Button(new Rect(20, 100, 100, 25), "Level Menu", backgroundStyle))
                        levelMenuIsEnabled = !levelMenuIsEnabled;
                }

                if (playerMenuIsEnabled)
                {
                    mainMenuIsEnabled = false;
                    ghostMenuIsEnabled = false;
                    levelMenuIsEnabled = false;
                    colorMenuIsEnabled = false;

                    GUI.Box(new Rect(0, 31, 150, 250), "");
                    GUI.Label(new Rect(20, 40, 100, 25), "Speed Slider:");

                    playerSpeed = GUI.HorizontalSlider(new Rect(20, 70, 100, 30), playerSpeed, 0.0F, 10.0F);
                }

                if (colorMenuIsEnabled)
                {
                    var styles = new GUIEx.DropdownStyles("button", "box", Color.white, 24, 8);
                    playerMenuIsEnabled = false;
                    levelMenuIsEnabled = false;
                    ghostMenuIsEnabled = false;

                    GUI.Box(new Rect(0, 31, 200, 150), "");
                    GUI.Label(new Rect(20, 40, 100, 25), "Primary Color");
                    GUI.Label(new Rect(20, 110, 100, 25), "Secondary Color");
                    secondaryDropdownState = GUIEx.Dropdown(new Rect(20, 135, 150, 30), GUIMenu.colorStrings, secondaryDropdownState, styles);
                    primaryDropdownState = GUIEx.Dropdown(new Rect(20, 65, 150, 30), GUIMenu.colorStrings, primaryDropdownState, styles);
                }

                if (ghostMenuIsEnabled)
                {
                    mainMenuIsEnabled = false;
                    playerMenuIsEnabled = false;
                    levelMenuIsEnabled = false;

                    GUI.Box(new Rect(0, 31, 150, 250), "");
                    if (GUI.Button(new Rect(20, 40, 100, 25), "List Ghosts", backgroundStyle))
                        listGhostsIsEnabled = !listGhostsIsEnabled;

                    if (GUI.Button(new Rect(20, 70, 100, 25), "Spawn Menu", backgroundStyle))
                        spawnMenuIsEnabled = !spawnMenuIsEnabled;

                    if (GUI.Button(new Rect(20, 100, 100, 25), "Ghosts Visible", backgroundStyle))
                        ghostsAlwaysVisible = !ghostsAlwaysVisible;

                    if (GUI.Button(new Rect(20, 130, 100, 25), "Start Hunting", backgroundStyle))
                    {
                        setupPhaseController.ForceEnterHuntingPhase();
                        foreach (GhostAI ghost in CustomGhostController.ghosts)
                        {
                            ghost.state = GhostAI.States.hunting;
                            ghost.isHunting = true;
                            ghost.isChasingPlayer = true;
                        }
                    }

                    if (listGhostsIsEnabled)
                        GUI.TextArea(new Rect(0, 300, 300, 300), GUIMenu.ghostText);

                    if (spawnMenuIsEnabled)
                    {
                        GUI.Box(new Rect(200, 31, 250, 250), "");
                        GUI.Box(new Rect(200, 33, 250, 250), "Spawn Menu", GUIStyle);
                        GUI.Label(new Rect(220, 60, 300, 250), "Type:");
                        GUI.Label(new Rect(220, 90, 300, 250), "Name:");
                        GUI.Label(new Rect(220, 120, 300, 250), "Room:");

                        if (GUI.Button(new Rect(280, 250, 100, 25), "Spawn", backgroundStyle))
                        {
                            if (ghostNameEdit == "")
                            {
                                if (roomDropdownState.Caption == "Random")
                                {
                                    MelonLogger.Log($"{mapNames[levelNum]} has {levelController.rooms.Length} rooms");
                                    ghostController.SpawnGhost(ghostTypesArr[ghostDropdownState.Select]);
                                    GUIMenu.ghostText += GhostMethods.GhostToString();
                                }
                                else
                                {
                                    ghostController.SpawnGhost(ghostTypesArr[ghostDropdownState.Select], roomDropdownState.Select);
                                    GUIMenu.ghostText += GhostMethods.GhostToString();
                                }
                            }
                            else
                            {
                                if (roomDropdownState.Caption == "Random")
                                {
                                    ghostController.SpawnGhost(ghostTypesArr[ghostDropdownState.Select], ghostNameEdit);
                                    GUIMenu.ghostText += GhostMethods.GhostToString();
                                }
                                else
                                {
                                    ghostController.SpawnGhost(ghostTypesArr[ghostDropdownState.Select], ghostNameEdit, roomDropdownState.Select);
                                    GUIMenu.ghostText += GhostMethods.GhostToString();
                                }
                            }
                        }

                        if (GUI.Button(new Rect(280, 220, 100, 25), "Spawn Random", backgroundStyle))
                        {
                            ghostController.SpawnGhost();
                            GUIMenu.ghostText += GhostMethods.GhostToString();
                        }

                        // Unity renders this last so it will be on top
                        var styles = new GUIEx.DropdownStyles("button", "box", Color.white, 24, 8);
                        ghostDropdownState = GUIEx.Dropdown(new Rect(290, 57, 150, 30), ghostTypeStrings, ghostDropdownState, styles);
                        roomDropdownState = GUIEx.Dropdown(new Rect(290, 120, 150, 30), roomNames, roomDropdownState, styles);
                        ghostNameEdit = GUI.TextField(new Rect(290, 92, 140, 20), ghostNameEdit, 25);
                    }
                }

                if (levelMenuIsEnabled)
                {
                    mainMenuIsEnabled = false;
                    playerMenuIsEnabled = false;
                    ghostMenuIsEnabled = false;
                    colorMenuIsEnabled = false;

                    GUI.Box(new Rect(0, 31, 150, 250), "");
                    if (GUI.Button(new Rect(20, 40, 100, 25), "INCOMPLETE", backgroundStyle))
                    {

                    }
                }
            }
        }

        private string[] GetRoomStrings()
        {
            string[] roomStrings;

            // Skip high school and asylum until I find a better way to do it
            if (!(levelNum == 3 || levelNum == 6))
            {
                roomStrings = new string[levelController.rooms.Length + 1];
                roomStrings[0] = "Random";

                for (int i = 1; i < levelController.rooms.Length; i++)
                {
                    roomStrings[i] = levelController.rooms[i].roomName;
                }
            }
            else
            {
                roomStrings = new string[] { "Random" };
            }

            return roomStrings;
        }
        private void InitializeGameObjects()
        {
            levelController = UnityEngine.Object.FindObjectOfType<LevelController>();
            ghostController = UnityEngine.Object.FindObjectOfType<GhostController>();
            player = UnityEngine.Object.FindObjectOfType<Player>();
            setupPhaseController = UnityEngine.Object.FindObjectOfType<SetupPhaseController>();
        }

    }
}