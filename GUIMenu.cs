using UnityEngine;
using Color = UnityEngine.Color;

namespace WraithGUI
{
    public sealed class GUIMenu
    {
        public static Color[] allColors = { Color.red, Color.yellow, Color.green, Color.cyan, Color.blue, Color.magenta, Color.white, Color.grey, Color.black, };
        private static float tValue;
        public static Color RandomColor()
        {
            return allColors[UnityEngine.Random.Range(0, allColors.Length)];
        }
        public static void CycleColors(GUIStyle guiStyle, bool background =  false)
        {
            tValue = (Mathf.Sin(Time.time * 1.5f) + 1) / 2.0f;
            if (!background)
            {
                guiStyle.normal.textColor = Color.Lerp(Color.cyan, Color.magenta, tValue);
            }
            else
            {
                guiStyle.normal.background = MakeTex(51, 26, Color.Lerp(Color.cyan, Color.magenta, tValue));
            }
        }
        private static Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; ++i)
            {
                pix[i] = col;
            }
            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }
    }
}