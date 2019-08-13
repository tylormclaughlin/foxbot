using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace foxbot
{
    public class CommandUtilities
    {
        public static async Task<string> CreatCommandAsync(string cmdName, string cmdContent, CommandService _commandService)
        {
            string result = "success";

            try
            {
                ModuleInfo module = await _commandService.CreateModuleAsync("", m =>
                {
                    m.AddCommand(cmdName, async (ctx, _, provider, _1) =>
                    {
                        await ctx.Channel.SendMessageAsync(cmdContent);
                    }, command => { });

                    m.Name = cmdName;
                });
            }
            catch (Exception e)
            {
                result = e.Message;
                Console.WriteLine(result);
            }

            return result;
        }

        /// <summary>
        /// Creates a new command.
        /// </summary>
        /// <param name="cmdName">Name of the new command</param>
        /// <param name="cmdContent">Body of the new command</param>
        /// <param name="_commandService">The command service the command should be added to</param>
        /// <returns></returns>
        public static async Task<string> AddCommandAsync(string cmdName, string cmdContent, CommandService _commandService)
        {
            string result = "";
            //Add new command info to commandPairs for persistence
            try
            {
                //Add the command only if a command by that name doesn't already exist
                if (DataStorage.customCommands.Any(x => x.commandName == cmdName))
                {
                    result = "Sorry, a command by that name already exists. Try !editcom if you wish to change it.";
                }
                else
                {
                    DataStorage.AddCustomCommand(cmdName, cmdContent);

                    result = await CreatCommandAsync(cmdName, cmdContent, _commandService);
                }
            }
            catch (Exception e)
            {
                result = e.Message;
                Console.WriteLine(e.Message);
            }

            return result;
        }

        /// <summary>
        /// Modifies an existing created command.
        /// </summary>
        /// <param name="cmdName">The name of the command to modify</param>
        /// <param name="cmdContent">The new body of the command being modified</param>
        /// <param name="_commandService">The command service the command belongs to</param>
        /// <returns></returns>
        public static async Task<string> ModifyCommandAsync(string cmdName, string cmdContent, CommandService _commandService)
        {
            //Delete the old command and create a new one with the same name
            string result = await DeleteCommandAsync(cmdName, _commandService);

            //Should probably come up with a better return value...
            if (result != $"!{cmdName} was deleted successfully")
            {
                return "Sorry, I can't find that command. Check !commands or consult this bot's administrator for more assistance.";
            }
            else
            {
                //Call AddCommand here instead
                return await AddCommandAsync(cmdName, cmdContent, _commandService);
            }
        }

        /// <summary>
        /// Deletes a created command
        /// </summary>
        /// <param name="cmdName">The name of the command to delete</param>
        /// <param name="_commandService">The command service the command should be removed from</param>
        /// <returns></returns>
        public static async Task<string> DeleteCommandAsync(string cmdName, CommandService _commandService)
        {
            ModuleInfo moduleToDelete = _commandService.Modules.FirstOrDefault(x => x.Name == cmdName);

            if (moduleToDelete == null)
            {
                return "Sorry, I couldn't find that command. Check !commands or consult this bot's administrator for more assistance.";
            }
            else
            {
                DataStorage.DeleteCustomCommand(cmdName);

                await _commandService.RemoveModuleAsync(moduleToDelete);
                return $"!{cmdName} was deleted successfully";
            }
        }
    }
}
