﻿using Miki.Localization;
using Miki.Localization.Exceptions;

namespace Miki.Modules.CustomCommands.Exceptions
{
    public class CommandNullException : LocalizedException
    {
        public override IResource LocaleResource 
            => new LanguageResource("error_command_null", commandName);

        private readonly string commandName;

        public CommandNullException(string commandName)
        {
            this.commandName = commandName;
        }
    }
}
