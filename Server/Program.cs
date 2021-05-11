/*
 * iTXTech SharpFlashDetector
 *
 * Copyright (C) 2020-2021 iTX Technologies
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License as
 * published by the Free Software Foundation, either version 3 of the
 * License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Affero General Public License for more details.
 *
 * You should have received a copy of the GNU Affero General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 *
 */

using System;
using System.Reflection;
using System.Text;
using CommandLine;
using CommandLine.Text;
using EmbedIO;
using EmbedIO.WebApi;
using iTXTech.FlashDetector;
using Pchp.Core;
using Swan.Logging;

namespace Server
{
    class Program
    {
        private static readonly Context Context = Context.CreateEmpty();

        public static Context GetContext()
        {
            return Context;
        }

        private class Options
        {
            [Option('a', "address",
                Default = "http://127.0.0.1:8080/",
                Required = false,
                HelpText = "Set Server Address.")]
            public string Address { get; set; }


            [Option('v', "verbose",
                Required = false, HelpText = "Disable verbose mode.")]
            public bool Verbose { get; set; }
        }

        private static void PrintHeader()
        {
            Console.WriteLine("SharpFlashDetector Server " + Assembly.GetEntryAssembly()?
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion);
            Console.WriteLine("Copyright (C) 2020-2021 iTX Technologies\nLicensed under GNU AGPL v3\nhttps://github.com/iTXTech/SharpFlashDetector");
        }

        private static void Main(string[] args)
        {
            PrintHeader();
            var address = "";
            var result = new Parser(with => with.HelpWriter = null).ParseArguments<Options>(args);
            result.WithParsed(opts =>
                {
                    address = opts.Address;
                    if (opts.Verbose)
                    {
                        Logger.UnregisterLogger<ConsoleLogger>();
                    }
                })
                .WithNotParsed(err =>
                {
                    if (err.IsHelp())
                    {
                        Console.WriteLine(HelpText.AutoBuild(result, h =>
                        {
                            h.AdditionalNewLineAfterOption = false;
                            h.Heading = "";
                            h.Copyright = "";
                            return HelpText.DefaultParsingErrorsHandler(result, h);
                        }, e => e));
                    }

                    Environment.Exit(0);
                });

            PeachPieHelper.load(Context);
            FlashDetector.registerProcessor(Context, new SharpProcessor(Context));
            Console.WriteLine();
            Console.WriteLine("iTXTech FlashDetector version: " + Loader.getInstance(Context).getInfo().getVersion());
            Console.WriteLine("Starting server on " + address);
            Console.WriteLine("Press Enter to exit.");
            Console.WriteLine();

            using var server = CreateWebServer(address);
            server.RunAsync();
            Console.ReadLine();
        }

        private static WebServer CreateWebServer(string address)
        {
            var server = new WebServer(o => o.WithUrlPrefix(address).WithMode(HttpListenerMode.EmbedIO))
                .WithLocalSessionManager()
                .WithWebApi("/", m => m.WithController<Controller>())
                .HandleHttpException(async (ctx, ex) =>
                {
                    ctx.Response.StatusCode = ex.StatusCode;
                    await ctx.SendStringAsync(
                        "Powered by <a href=\"https://github.com/iTXTech/SharpFlashDetector\">SharpFlashDetector Server</a></br>" +
                        "by <a href=\"mailto:peratx@itxtech.org\">PeratX@iTXTech.org</a></br>" +
                        ex.StatusCode + " " + ctx.Response.StatusDescription,
                        "text/html", Encoding.UTF8);
                });

            return server;
        }
    }
}