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

        //Add Command
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

        //Modify Command
        public static async Task<string> ModifyCommandAsync(string cmdName, string cmdContent, CommandService _commandService)
        {
            string result = await DeleteCommandAsync(cmdName, _commandService);

            //Should probably come up with a better return value...
            if (result != $"!{cmdName} was deleted successfully")
            {
                return "Sorry, I can't find that command. Check !commands or consult this bot's administrator for more assistance.";
            }
            else
            {
                return await CreatCommandAsync(cmdName, cmdContent, _commandService);
            }
        }

        //Delete Command
        public static async Task<string> DeleteCommandAsync(string cmdName, CommandService _commandService)
        {
            ModuleInfo moduleToDelete = _commandService.Modules.FirstOrDefault(x => x.Name == cmdName);

            //return $"Found module with name {moduleToDelete.Name}";

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
