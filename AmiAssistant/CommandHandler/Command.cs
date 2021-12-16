﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

using AmiFriendo.CommandHandler.Actions;

namespace AmiFriendo.CommandHandler
{
    using CommandContext = Dictionary<string, string>;
    public class Command
    {
        public List<string> Commands { get; } = new List<string>();
        public List<IAction> Actions { get; } = new List<IAction>();

        public string Execute(CommandContext cc)
        {
            _context = cc;

            foreach (var action in Actions)
            {
                action.Execute(ref _context);
            }

            try
            {
                return _context["return"];
            }
            catch
            {
                return null;
            }
        }
        public string Execute()
        {
            _context = new CommandContext();
            return Execute(_context);
        }

        private CommandContext _context;
    }
}
