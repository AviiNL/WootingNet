using System;
using System.Collections.Generic;
using System.Linq;

namespace WootingNet
{
    public enum Key
    {
        ESC,
        UNUSED1,
        F1,
        F2,
        F3,
        F4,
        F5,
        F6,
        F7,
        F8,
        F9,
        F10,
        F11,
        F12,
        PRINTSCREEN,
        PAUSE,
        SRC_MODE,
        A1,
        A2,
        A3,
        MODE,
        Tilde,
        N1,
        N2,
        N3,
        N4,
        N5,
        N6,
        N7,
        N8,
        N9,
        N0,
        MINUS,
        EUQALS,
        BACKSPACE,
        INSERT,
        HOME,
        PAGEUP,
        NUMLOCK,
        NUMSLASH,
        NUMMULTIPLY,
        NUMMINUS,
        TAB,
        Q, W, E, R, T, Y, U, I, O, P,
        LEFTBRACKET,
        RIGHTBRACKET,
        BACKSLASH,
        DELETE,
        END,
        PAGEDOWN,
        NUM7,
        NUM8,
        NUM9,
        NUMPLUS,
        CAPSLOCK,
        A, S, D, F, G, H, J, K, L,
        SEMICOLON,
        QUOTE,
        ISO1,
        RETURN,
        UNUSED2,
        UNUSED3,
        UNUSED4,
        NUM4,
        NUM5,
        NUM6,
        UNUSED5,
        LEFTSHIFT,
        ISO2,
        Z, X, C, V, B, N, M,
        COMMA,
        PERIOD,
        FORWARDSLASH,
        UNUSED6,
        RIGHTSHIFT,
        UNUSED7,
        UP,
        UNUSED8,
        NUM1,
        NUM2,
        NUM3,
        NUMRETURN,
        LEFTCONTROL,
        LEFTWIN,
        LEFTALT,
        UNUSED9,
        UNUSED10,
        UNUSED11,
        SPACE,
        UNUSED12,
        UNUSED13,
        UNUSED14,
        RIGHTALT,
        RIGHTWIN,
        FUNCTION,
        RIGHTCONTROL,
        LEFT,
        DOWN,
        RIGHT,
        UNUSED15,
        NUM0,
        NUMDEL,
        UNUSED16
    }

    internal struct Position
    {
        public int col;
        public int row;

        public Position(int col, int row)
        {
            this.col = col;
            this.row = row;
        }
    };

    public static class Keys
    {
        internal static Dictionary<Key, Position> WootingKey = new Dictionary<Key, Position>();

        static Keys()
        {
            var l = Enum.GetValues(typeof(Key)).Cast<Key>();

            int x = 0;
            int y = 0;
            foreach (var k in l)
            {
                WootingKey.Add(k, new Position(x, y));
                x++;
                if (x > 20)
                {
                    y++;
                    x = 0;
                }
            }
        }

        internal static Key GetKey(int row, int col)
        {
            var a = WootingKey.FirstOrDefault(x => x.Value.col == col && x.Value.row == row);
            return a.Key;
        }

        internal static Position GetPosition(Key key)
        {
            return WootingKey[key];
        }

        /// <summary>
        /// Check if a key is valid
        /// </summary>
        /// <param name="row">Which row the key is on</param>
        /// <param name="col">Which column the key is on</param>
        /// <returns></returns>
        public static bool IsValid(int row, int col)
        {
            try
            {
                var a = WootingKey.First(x => x.Value.col == col && x.Value.row == row);
                return a.Key.ToString().StartsWith("UNUSED") ? false : true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }

        }

    }
}
