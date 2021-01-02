//
//  From: https://unitylist.com/p/10ne/Unity-GUI-Dropdown
//  Removed some things I did not need
//
using UnityEngine;
using WraithGUI;

public static class GUIEx
{
    public class DropdownState
    {
        /// <summary>
        /// Index of selected option 
        /// </summary>
        public int Select = 0;
        /// <summary>
        /// Text of caption
        /// </summary>
        public string Caption = "";

        // Used internally
        public enum status
        {
            Closed,
            Opening,
            Opened,
            Closing,
        }
        public int nextSelect = 0;
        private status currentStatus_ = status.Closed;
        public status currentStatus
        {
            get
            {
                return currentStatus_;
            }
            set
            {
                currentStatus_ = value;
                currentStatusStartTime = Time.time;
            }
        }
        public float currentStatusStartTime { get; private set; }
    }


    public class DropdownStyles
    {
        /// <summary>
        /// GUIStyle for caption label
        /// </summary>
        public GUIStyle Caption;
        /// <summary>
        /// GUIStyle for options
        /// </summary>
        public GUIStyle Option;
        /// <summary>
        /// Color of arrow
        /// </summary>
        public Color ArrowColor = new Color(0.5f, 0.5f, 0.5f);
        /// <summary>
        /// Size of arrow
        /// </summary>
        public int ArrowSize = 16;
        /// <summary>
        /// Right margin of arrow
        /// </summary>
        public int ArrowMargin = 8;

        public DropdownStyles(GUIStyle caption, GUIStyle option)
        {
            this.Caption = caption;
            this.Option = option;
        }

        public DropdownStyles(GUIStyle caption, GUIStyle option, Color arrowColor, int arrowSize = 16, int arrowMargin = 8)
        {
            this.Caption = caption;
            this.Option = option;
            this.ArrowColor = arrowColor;
            this.ArrowSize = arrowSize;
            this.ArrowMargin = arrowMargin;
        }
    }

    static Texture2D arrowTexture_ = null;
    static GUISkin skin_ = null;
    static DropdownStyles defaultDropdownStyles_ = null;

    static void setup()
    {
        if (arrowTexture_ == null)
        {
            arrowTexture_ = Resources.Load<Texture2D>("arrow");
        }
        if (skin_ == null)
        {
            skin_ = Resources.Load<GUISkin>("Dropdown");
        }
        if (defaultDropdownStyles_ == null)
        {
            defaultDropdownStyles_ = new DropdownStyles(skin_.button, skin_.FindStyle("dropdown_option"));
        }
    }

    public static DropdownState Dropdown(Rect position, string[] options, DropdownState state, DropdownStyles styles = null)
    {
        //setup();

        if (styles == null)
        {
            styles = defaultDropdownStyles_;
        }

        switch (state.currentStatus)
        {
            case DropdownState.status.Closed:
                return closedDropdown(position, options, state, styles);
            case DropdownState.status.Opening:
                return openingDropdown(position, options, state, styles);
            case DropdownState.status.Opened:
                return openedDropdown(position, options, state, styles);
            case DropdownState.status.Closing:
                return closingDropdown(position, options, state, styles);
        }

        return state;
    }

    static DropdownState closedDropdown(Rect position, string[] options, DropdownState state, DropdownStyles styles)
    {
        if (drawCaption(position, options, state, styles))
        {
            state.currentStatus = DropdownState.status.Opening;
        }
        return state;
    }

    static DropdownState openingDropdown(Rect position, string[] options, DropdownState state, DropdownStyles styles)
    {
        const float fadeTime = 0.1f;

        float dt = Time.time - state.currentStatusStartTime;
        drawCaption(position, options, state, styles);

        var prevColor = GUI.color;
        GUI.color = new Color(1, 1, 1, dt / fadeTime);
        drawDropdownList(position, options, state, styles);
        GUI.color = prevColor;

        if (dt >= fadeTime)
        {
            state.currentStatus = DropdownState.status.Opened;
        }
        return state;
    }

    static DropdownState openedDropdown(Rect position, string[] options, DropdownState state, DropdownStyles styles)
    {
        if (drawCaption(position, options, state, styles))
        {
            state.currentStatus = DropdownState.status.Closing;
        }
        int newSelect = drawDropdownList(position, options, state, styles);
        if (newSelect >= 0)
        {
            state.nextSelect = newSelect;
            state.currentStatus = DropdownState.status.Closing;
        }
        return state;
    }

    static DropdownState closingDropdown(Rect position, string[] options, DropdownState state, DropdownStyles styles)
    {
        const float fadeTime = 0.1f;

        float dt = Time.time - state.currentStatusStartTime;
        drawCaption(position, options, state, styles);

        var prevColor = GUI.color;
        GUI.color = new Color(1, 1, 1, 1 - dt / fadeTime);
        drawDropdownList(position, options, state, styles);
        GUI.color = prevColor;

        if (dt >= fadeTime)
        {
            state.Select = state.nextSelect;
            state.currentStatus = DropdownState.status.Closed;
        }
        return state;
    }

    static bool drawCaption(Rect position, string[] options, DropdownState state, DropdownStyles styles)
    {
        if (0 <= state.Select && state.Select < options.Length)
        {
            state.Caption = options[state.Select];
        }

        // Caption
        bool pushed = GUI.Button(position, state.Caption, PhasmoGame.backgroundStyle);

        // Arrow
        var arrowPosition = new Rect(
            position.xMax - styles.ArrowMargin - styles.ArrowSize,
            position.center.y - styles.ArrowSize / 2,
            styles.ArrowSize, styles.ArrowSize);
        var prevColor = GUI.color;
        GUI.color = styles.ArrowColor;
        GUI.DrawTexture(arrowPosition, arrowTexture_);
        GUI.color = prevColor;

        return pushed;
    }

    static int drawDropdownList(Rect position, string[] options, DropdownState state, DropdownStyles styles)
    {
        int newSelect = -1;

        float offsetY = position.yMax;
        float totalOptionHeight = position.height * options.Length;
        if (offsetY + totalOptionHeight > Screen.height)
        {
            offsetY = position.yMin - totalOptionHeight;
        }

        for (int i = 0; i < options.Length; i++)
        {
            var optionPosition = position;
            optionPosition.y = offsetY + position.height * i;
            string text = string.Format("{0}{1}", i == state.Select ? " ✓ " : " 　 ", options[i]);
            if (GUI.Button(optionPosition, text, styles.Option))
            {
                newSelect = i;
            }
        }

        return newSelect;
    }

}