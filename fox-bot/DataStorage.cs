using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace foxbot
{
    class DataStorage
    {
        public static List<CustomCommand> customCommands;
        private const string commandFilename = "command_list.json";

        static DataStorage()
        {
            //Not sure I need this currently.
        }

        public static void AddCustomCommand(string cmdName, string cmdContent)
        {
            customCommands.Add(new CustomCommand(cmdName, cmdContent));
            SaveCommandsToFile();
        }

        public static bool LoadCommandsFromFile()
        {
            if (!File.Exists(commandFilename))
            {
                customCommands = new List<CustomCommand>();
                string json = File.ReadAllText(commandFilename);
                customCommands = JsonConvert.DeserializeObject<List<CustomCommand>>(json);
                return false;
            }
            else
            {
                string json = File.ReadAllText(commandFilename);
                customCommands = JsonConvert.DeserializeObject<List<CustomCommand>>(json);

                return true;
            }
        }

        public static bool SaveCommandsToFile()
        {
            string json = JsonConvert.SerializeObject(customCommands, Formatting.Indented);
            bool success = true;

            try
            {
                File.WriteAllText(commandFilename, json);
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
