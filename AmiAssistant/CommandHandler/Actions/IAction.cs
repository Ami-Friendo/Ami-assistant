﻿using System;
using System.Collections.Generic;
using System.Text;

using AmiFriendo.CommandHandler.Arguments;

namespace AmiFriendo.CommandHandler.Actions
{
    using CommandContext = Dictionary<string, string>;
    public interface IAction
    {
        string Name { get; }
        string FriendlyName { get; set; }
        string Description { get; }
        IArgument[] InputArguments { get; }
        IArgument[] OutputArguments { get; }

        void Execute(ref CommandContext context);
        bool CanExecute();
        bool CanExecute(out string cause); // cause - причина невозможности выполнения,
                                           // если можно ваполнить cause = null
    }
}
