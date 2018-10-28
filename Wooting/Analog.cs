using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace WootingNet
{
    public static class Analog
    {
        public delegate void AnalogUpdate(Key key, float value);
        public static event AnalogUpdate OnAnalogUpdate;

        [DllImport(@"lib\wooting-analog-sdk.dll", EntryPoint = "wooting_kbd_connected")]
        private static extern bool wooting_kbd_connected();

        [DllImport(@"lib\wooting-analog-sdk.dll", EntryPoint = "wooting_read_analog")]
        private static extern uint wooting_read_analog(uint row, uint col);

        private static bool isRunning = false;
        
        private static Dictionary<Key, float> lastKeyValues = new Dictionary<Key, float>();

        /// <summary>
        /// Check if the keyboard is connected
        /// </summary>
        /// <returns>False if disconnected</returns>
        public static bool IsConnected()
        {
            return wooting_kbd_connected();
        }

        /// <summary>
        /// Manually read a value from a key's position
        /// </summary>
        /// <param name="row">The row to read from</param>
        /// <param name="col">The column to read from</param>
        /// <returns></returns>
        public static float ReadValue(int row, int col)
        {
            return wooting_read_analog((uint)row, (uint)col) / 255f;
        }

        /// <summary>
        /// Manually read a value from a key
        /// </summary>
        /// <param name="key">The key to read</param>
        /// <returns></returns>
        public static float ReadValue(Key key)
        {
            var pos = Keys.GetPosition(key);
            if (!Keys.IsValid(pos.row, pos.col))
            {
                return 0;
            }

            return ReadValue(pos.row, pos.col);
        }

        /// <summary>
        /// Start listening for analog updates
        /// </summary>
        public static void Start()
        {
            if (isRunning)
                return;

            isRunning = true;

            Task.Run(() =>
            {
                while (isRunning)
                {
                    foreach (var kvp in Keys.WootingKey)
                    {
                        if (!Keys.IsValid(kvp.Value.row, kvp.Value.col))
                            continue;

                        if (!lastKeyValues.ContainsKey(kvp.Key))
                            lastKeyValues.Add(kvp.Key, 0);

                        var value = ReadValue(kvp.Key);

                        if (lastKeyValues[kvp.Key] != value)
                        {
                            lastKeyValues[kvp.Key] = value;
                            OnAnalogUpdate?.Invoke(kvp.Key, value);
                        }
                    }
                }
            }).ConfigureAwait(false);
           
        }

        /// <summary>
        /// Stop listening for analog updates
        /// </summary>
        public static void Stop()
        {
            isRunning = false;
        }
    }
}
