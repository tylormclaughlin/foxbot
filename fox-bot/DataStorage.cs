using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace foxbot
{
    class DataStorage
    {
        public static List<CustomCommand> customCommands;
        private const string commandFilename = "command_list.json";
        private const string nicknamesFilename = "instinct_nicknames.txt";

        static DataStorage()
        {
            //Not sure I need this currently.
        }

        public static void AddCustomCommand(string cmdName, string cmdContent)
        {
            customCommands.Add(new CustomCommand(cmdName, cmdContent));
            SaveCommandsToFile();
        }

        public static void DeleteCustomCommand(string cmdName)
        {
            CustomCommand cmdToDelete = customCommands.FirstOrDefault(x => x.commandName == cmdName);
            customCommands.Remove(cmdToDelete);

            SaveCommandsToFile();
        }

        public static bool LoadCommandsFromFile()
        {
            if (!File.Exists(commandFilename))
            {
                return false;
            }
            else
            {
                //Make sure the file has no duplicate command names before adding to the command list.
                string json = File.ReadAllText(commandFilename);
                List<CustomCommand> temp = JsonConvert.DeserializeObject<List<CustomCommand>>(json);

                foreach (CustomCommand cmd in temp)
                {
                    if (temp.Where(x => x.commandName == cmd.commandName).Count() > 1)
                    {
                        return false;
                    }
                }

                customCommands = temp;

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

        public static bool SaveUsernamesToFile(List<string> usernames)
        {
            bool result = true;

            if(File.Exists(nicknamesFilename))
            {
                File.Delete(nicknamesFilename);
            }

            try
            {
                foreach (string user in usernames)
                {
                    File.AppendAllText(nicknamesFilename, (user + Environment.NewLine));
                }
            }
            catch (Exception e)
            {
                result = false;
            }

            return result;
        }
    }
}
