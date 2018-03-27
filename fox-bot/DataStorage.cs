using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace foxbot
{
    class DataStorage
    {
        private static List<CustomCommand> commandPairs = new List<CustomCommand>();
        private const string commandFilename = "command_list.json";

        static DataStorage()
        {
            //Not sure I need this currently.
        }

        public static void AddCustomCommand(string cmdName, string cmdContent)
        {
            commandPairs.Add(new CustomCommand(cmdName, cmdContent));
            SaveCommandsToFile(commandFilename);
        }

        public static bool LoadCommandsFromFile(string filename)
        {
            if (!File.Exists(filename))
            {
                return false;
            }
            else
            {
                string json = File.ReadAllText(filename);
                commandPairs = JsonConvert.DeserializeObject<List<CustomCommand>>(json);

                return true;
            }
        }

        public static bool SaveCommandsToFile(string filename)
        {
            string json = JsonConvert.SerializeObject(commandPairs, Formatting.Indented);
            bool success = true;

            try
            {
                File.WriteAllText(filename, json);
            }
            catch (Exception e)
            {
                Console.WriteLine("Writing commands to file failed somehow.");
                Console.WriteLine(e.Message);

                success = false;
            }

            return success;
        }
    }
}
