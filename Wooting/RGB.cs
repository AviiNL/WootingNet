using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;

namespace WootingNet
{
    public static class RGB
    {
        [DllImport(@"lib\wooting-rgb-sdk.dll", EntryPoint = "wooting_rgb_kbd_connected")]
        private static extern bool wooting_rgb_kbd_connected();

        [DllImport(@"lib\wooting-rgb-sdk.dll", EntryPoint = "wooting_rgb_reset")]
        private static extern bool wooting_rgb_reset();

        [DllImport(@"lib\wooting-rgb-sdk.dll", EntryPoint = "wooting_rgb_direct_set_key")]
        private static extern bool wooting_rgb_direct_set_key(uint row, uint col, uint r, uint g, uint b);

        [DllImport(@"lib\wooting-rgb-sdk.dll", EntryPoint = "wooting_rgb_direct_reset_key")]
        private static extern bool wooting_rgb_direct_reset_key(uint row, uint col);

        /// <summary>
        /// Check if the keyboard is connected
        /// </summary>
        /// <returns>False if disconnected</returns>
        public static bool IsConnected()
        {
            return wooting_rgb_kbd_connected();
        }

        /// <summary>
        /// Reset the keyboard
        /// </summary>
        /// <returns></returns>
        public static bool Reset()
        {
            return wooting_rgb_reset();
        }

        /// <summary>
        /// Set a key to a specific color
        /// </summary>
        /// <param name="key">The key to change</param>
        /// <param name="color">The color to set</param>
        /// <returns></returns>
        public static bool SetKey(Key key, Color color)
        {
            var pos = Keys.GetPosition(key);

            return SetKey(pos.row, pos.col, color);
        }

        /// <summary>
        /// Set the color of a key with manual position
        /// </summary>
        /// <param name="row">Which row the key is on</param>
        /// <param name="col">Which column the key is on</param>
        /// <param name="color">The color to set</param>
        /// <returns></returns>
        public static bool SetKey(int row, int col, Color color)
        {
            return SetKey(row, col, color.R, color.G, color.B);
        }

        /// <summary>
        /// Set the color of a key with manual position and color
        /// </summary>
        /// <param name="row">Which row the key is on</param>
        /// <param name="col">Which column the key is on</param>
        /// <param name="R">Red (0-255)</param>
        /// <param name="G">Green (0-255)</param>
        /// <param name="B">Blue (0-255)</param>
        /// <returns></returns>
        public static bool SetKey(int row, int col, int R, int G, int B)
        {
            if (!Keys.IsValid(row, col))
                return false;

            return wooting_rgb_direct_set_key((uint)row, (uint)col, (uint)R, (uint)G, (uint)B);
        }

        /// <summary>
        /// Reset a key to it's default color
        /// </summary>
        /// <param name="key">The key to reset</param>
        /// <returns></returns>
        public static bool ResetKey(Key key)
        {
            var pos = Keys.GetPosition(key);

            return ResetKey(pos.row, pos.col);
        }

        /// <summary>
        /// Reset a key to it's default color with manual position
        /// </summary>
        /// <param name="row">Which row the key is on</param>
        /// <param name="col">Which column the key is on</param>
        /// <returns></returns>
        public static bool ResetKey(int row, int col)
        {
            if (!Keys.IsValid(row, col))
                return false;

            return wooting_rgb_direct_reset_key((uint)row, (uint)col);
        }

        

    }
}
