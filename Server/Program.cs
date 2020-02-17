/*
 * iTXTech SharpFlashDetector
 *
 * Copyright (C) 2020 iTX Technologies
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;
using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using iTXTech.FlashDetector;
using iTXTech.SimpleFramework;
using Pchp.Core;
using Swan.Logging;

namespace Server
{
    class Program
    {
        private static Context context;

        public static Context GetContext()
        {
            return context;
        }

        private class Options
        {
            [Option('a', "address",
                Default = "http://127.0.0.1:8080/",
                Required = false,
                HelpText = "Server Address of SharpFlashDetector Server")]
            public string Address { get; set; }


            [Option('n', "disable-logging",
                Required = false, HelpText = "Disable logging of EmbedIO.")]
            public bool DisableLogging { get; set; }
        }

        private static void PrintHeader()
        {
            Console.WriteLine("SharpFlashDetector Server " + Assembly.GetEntryAssembly()?
                                  .GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion);
            Console.WriteLine("Copyright (C) 2020 iTX Technologies\nhttps://github.com/iTXTech/SharpFlashDetector");
        }

        static void Main(string[] args)
        {
            PrintHeader();
            var addr = "";
            var result = new Parser(with => with.HelpWriter = null).ParseArguments<Options>(args);
            result.WithParsed(opts =>
                {
                    addr = opts.Address;
                    if (opts.DisableLogging)
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

            context = Context.CreateEmpty();
            PeachPieHelper.load(context);
            Console.WriteLine();
            Console.WriteLine("iTXTech FlashDetector version: " + FlashDetector.getVersion(context));
            Console.WriteLine("Starting server on " + addr);
            Console.WriteLine("Press Enter to exit.");
            Console.WriteLine();

            using var server = CreateWebServer(addr);
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
                        ex.StatusCode + " " + ctx.Response.StatusDescription,
                        "text/html", Encoding.UTF8);
                });

            return server;
        }
    }

    internal sealed class Controller : WebApiController
    {
        private void ProcessResponse()
        {
            Response.Headers.Add("Access-Control-Allow-Origin: *");
            Response.Headers.Add("Access-Control-Allow-Headers: *");
            Response.Headers.Add("Server: SharpFlashDetector");
            Response.Headers.Add("X-SimpleFramework: " + Framework.PROG_VERSION);
        }

        private string GetRemote()
        {
            return Request.Headers.Get("X-Real-IP") ?? Request.RemoteEndPoint.ToString();
        }

        private string GetQuery()
        {
            return string.Join("&",
                Request.QueryString.AllKeys.Select(key => key + "=" + Request.QueryString[key]).ToArray());
        }

        [Route(HttpVerbs.Get, "/")]
        public async Task Index()
        {
            ProcessResponse();
            await HttpContext.SendStringAsync(PeachPieHelper.index(Program.GetContext(), GetQuery(), GetRemote()),
                "application/json; charset=utf-8", Encoding.UTF8);
        }

        [Route(HttpVerbs.Get, "/info")]
        public async Task Info()
        {
            ProcessResponse();
            await HttpContext.SendStringAsync(PeachPieHelper.info(Program.GetContext(), GetQuery(), GetRemote()),
                "application/json; charset=utf-8", Encoding.UTF8);
        }

        [Route(HttpVerbs.Get, "/summary")]
        public async Task Summary([QueryField] string pn)
        {
            ProcessResponse();
            await HttpContext.SendStringAsync(PeachPieHelper.summary(Program.GetContext(), GetQuery(), GetRemote(), pn),
                "application/json; charset=utf-8", Encoding.UTF8);
        }

        [Route(HttpVerbs.Get, "/decode")]
        public async Task Decode([QueryField] string pn, [QueryField] int trans)
        {
            ProcessResponse();
            await HttpContext.SendStringAsync(
                PeachPieHelper.decode(Program.GetContext(), GetQuery(), GetRemote(), trans != 0, pn),
                "application/json; charset=utf-8", Encoding.UTF8);
        }

        [Route(HttpVerbs.Get, "/searchId")]
        public async Task SearchId([QueryField] string id, [QueryField] int trans)
        {
            ProcessResponse();
            await HttpContext.SendStringAsync(
                PeachPieHelper.searchId(Program.GetContext(), GetQuery(), GetRemote(), trans != 0, id),
                "application/json; charset=utf-8", Encoding.UTF8);
        }

        [Route(HttpVerbs.Get, "/searchPn")]
        public async Task SearchPn([QueryField] string pn, [QueryField] int trans)
        {
            ProcessResponse();
            await HttpContext.SendStringAsync(
                PeachPieHelper.searchPn(Program.GetContext(), GetQuery(), GetRemote(), trans != 0, pn),
                "application/json; charset=utf-8", Encoding.UTF8);
        }
    }
}