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
using iTXTech.FlashDetector.Processor;
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
                HelpText = "Set Server Address of SharpFlashDetector Server.")]
            public string Address { get; set; }


            [Option('v', "verbose",
                Required = false, HelpText = "Enable verbose mode.")]
            public bool Verbose { get; set; }
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
            var address = "";
            var result = new Parser(with => with.HelpWriter = null).ParseArguments<Options>(args);
            result.WithParsed(opts =>
                {
                    address = opts.Address;
                    if (!opts.Verbose)
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
            FlashDetector.registerProcessor(context, new SharpProcessor(context));
            Console.WriteLine();
            Console.WriteLine("iTXTech FlashDetector version: " + Loader.getInstance(context).getInfo().getVersion());
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
                        ex.StatusCode + " " + ctx.Response.StatusDescription,
                        "text/html", Encoding.UTF8);
                });

            return server;
        }
    }

    internal class SharpProcessor : Processor
    {
        public SharpProcessor(Context ctx) : base(ctx)
        {
        }

        public override bool index(string query, string remote, string name, PhpAlias c)
        {
            c.Value.Array.Add("server", "SharpFlashDetector Server");
            return true;
        }
    }

    internal class Controller : WebApiController
    {
        private const string CONTENT_TYPE = "application/json; charset=utf-8";

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
                CONTENT_TYPE, Encoding.UTF8);
        }

        [Route(HttpVerbs.Get, "/info")]
        public async Task Info()
        {
            ProcessResponse();
            await HttpContext.SendStringAsync(PeachPieHelper.info(Program.GetContext(), GetQuery(), GetRemote()),
                CONTENT_TYPE, Encoding.UTF8);
        }

        [Route(HttpVerbs.Get, "/summary")]
        public async Task Summary([QueryField] string lang, [QueryField] string pn)
        {
            ProcessResponse();
            await HttpContext.SendStringAsync(
                PeachPieHelper.summary(Program.GetContext(), GetQuery(), GetRemote(), lang, pn),
                CONTENT_TYPE, Encoding.UTF8);
        }

        [Route(HttpVerbs.Get, "/decode")]
        public async Task Decode([QueryField] string lang, [QueryField] string pn)
        {
            ProcessResponse();
            await HttpContext.SendStringAsync(
                PeachPieHelper.decode(Program.GetContext(), GetQuery(), GetRemote(), lang, pn),
                CONTENT_TYPE, Encoding.UTF8);
        }

        [Route(HttpVerbs.Get, "/searchId")]
        public async Task SearchId([QueryField] string lang, [QueryField] string id)
        {
            ProcessResponse();
            await HttpContext.SendStringAsync(
                PeachPieHelper.searchId(Program.GetContext(), GetQuery(), GetRemote(), lang, id),
                CONTENT_TYPE, Encoding.UTF8);
        }

        [Route(HttpVerbs.Get, "/searchPn")]
        public async Task SearchPn([QueryField] string lang, [QueryField] string pn)
        {
            ProcessResponse();
            await HttpContext.SendStringAsync(
                PeachPieHelper.searchPn(Program.GetContext(), GetQuery(), GetRemote(), lang, pn),
                CONTENT_TYPE, Encoding.UTF8);
        }
    }
}