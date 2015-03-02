﻿using Sample.Helpers;
using Yaclops;

namespace Sample.Commands
{
    public class InitCommand : ISubCommand
    {
        public void Execute()
        {
            // Execute the command. For demo purposes, just dump out the parameters...
            this.Dump();
        }
    }
}
