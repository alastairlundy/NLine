/*
    BasisBox - NLine
    Copyright (C) 2024 Alastair Lundy

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, version 3 of the License.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

using System.Reflection;

using AlastairLundy.Extensions.System;

using NLine.Cli.Commands;
using NLine.Cli.Localizations;

using Spectre.Console.Cli;

CommandApp commandApp = new CommandApp();

commandApp.Configure(config =>
{
    config.AddCommand<LineNumberingCommand>("")
        .WithAlias("nl")
        .WithDescription(Resources.App_Description)
        .WithExample("-v 0")
        .WithExample("-w 5");

    config.SetApplicationVersion(Assembly.GetExecutingAssembly().GetName().Version.ToFriendlyVersionString());

});

commandApp.SetDefaultCommand<LineNumberingCommand>();

return commandApp.Run(args);