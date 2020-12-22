// WraithGUI by Karma Kitten
// For Phasmophobia build #5635423, manifest #4488252479481708564

using MelonLoader;
using UnityEngine;
using UnityEngine.InputSystem;

namespace WraithGUI
{
    public sealed class PhasmoGame : MelonMod
    {
        private static readonly GUIStyle GUIStyle = new GUIStyle();
        private static readonly GUIStyle backgroundStyle = new GUIStyle();
        private bool menuIsEnabled = false;
        private bool mainMenuIsEnabled = false;
        private bool playerMenuIsEnabled = false;
        private bool ghostMenuIsEnabled = false;
        private bool levelMenuIsEnabled = false;
        private int levelNum;
        private static string[] mapNames = { "Opening Scene", "Lobby", "Tanglewood Street", "Ridgeview Road House", "Edgefield Street House", "Asylum", "Brownstone High School", "Bleasdale Farmhouse" , "Grafton Farmhouse" };

        public static LevelController levelController;
        public override void OnApplicationStart()
        {
            MelonLogger.Log($"Successfully loaded v{typeof(PhasmoGame).Assembly.GetName().Version}. Enjoy!");
            GUIStyle.alignment = TextAnchor.MiddleCenter;
            GUIStyle.fontSize = 16;
            GUIStyle.fontStyle = FontStyle.Bold;
            backgroundStyle.alignment = TextAnchor.MiddleCenter;
            backgroundStyle.normal.textColor = Color.white;
        }
        public override void OnLevelWasInitialized(int level)
        {
            levelNum = level; // Save this for checks later.
            MelonLogger.Log($"{mapNames[level]} was initialized");
            InitializeGameObjects(); // This will ensure the level is never null
        }
        public override void OnUpdate()
        {
            // Open Menu (F1)
            if (Keyboard.current.f1Key.wasPressedThisFrame)
            {
                menuIsEnabled = !menuIsEnabled;
            }

            GUIMenu.CycleColors(GUIStyle);
            GUIMenu.CycleColors(backgroundStyle, true);
        }
        public override void OnGUI()
        {
            if (menuIsEnabled)
            {
                GUI.Box(new Rect(0, 0, Screen.width, 30), "");
                GUI.Box(new Rect(0, 0, Screen.width, 25), "WraithGUI", GUIStyle);

                if (GUI.Button(new Rect(5, 0, 50, 25), "Main", backgroundStyle))
                {
                    mainMenuIsEnabled = !mainMenuIsEnabled;
                }

                if (GUI.Button(new Rect(65, 0, 60, 25), "Maps", backgroundStyle))
                {
                    
                }

                if (GUI.Button(new Rect(135, 0, 65, 25), "Colors", backgroundStyle))
                {

                }

                if (GUI.Button(new Rect(210, 0, 65, 25), "Players", backgroundStyle))
                {

                }

                if (mainMenuIsEnabled)
                {
                    playerMenuIsEnabled = false;
                    levelMenuIsEnabled = false;
                    ghostMenuIsEnabled = false;
                    GUI.Box(new Rect(0, 31, 150, 250), "");

                    if (GUI.Button(new Rect(20, 40, 100, 25), "Player Menu", backgroundStyle)) { playerMenuIsEnabled = !playerMenuIsEnabled; }

                    if (GUI.Button(new Rect(20, 70, 100, 25), "Ghost Menu", backgroundStyle)) { ghostMenuIsEnabled = !ghostMenuIsEnabled; }
                    
                    if (GUI.Button(new Rect(20, 100, 100, 25), "Level Menu", backgroundStyle)) { levelMenuIsEnabled = !levelMenuIsEnabled; }
                }

                if (playerMenuIsEnabled)
                {
                    mainMenuIsEnabled = false;
                    ghostMenuIsEnabled = false;
                    levelMenuIsEnabled = false;
                    GUI.Box(new Rect(0, 31, 150, 250), "");
                    if (GUI.Button(new Rect(20, 40, 100, 25), "player test", backgroundStyle)) {  }
                }

                if (ghostMenuIsEnabled)
                {
                    mainMenuIsEnabled = false;
                    playerMenuIsEnabled = false;
                    levelMenuIsEnabled = false;
                    GUI.Box(new Rect(0, 31, 150, 250), "");
                    if (GUI.Button(new Rect(20, 40, 100, 25), "ghost test", backgroundStyle)) {  }
                }

                if (levelMenuIsEnabled)
                {
                    mainMenuIsEnabled = false;
                    playerMenuIsEnabled = false;
                    ghostMenuIsEnabled = false;
                    GUI.Box(new Rect(0, 31, 150, 250), "");
                    if (GUI.Button(new Rect(20, 40, 100, 25), "level test", backgroundStyle)) {  }
                }
            }
        }

        private void InitializeGameObjects()
        {
            levelController = UnityEngine.Object.FindObjectOfType<LevelController>();
        }

    }
}