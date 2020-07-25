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

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using iTXTech.FlashDetector;
using iTXTech.SimpleFramework;

namespace Server
{
    internal class Controller : WebApiController
    {
        private const string ContentType = "application/json; charset=utf-8";

        private async Task SendResponse(string resp)
        {
            Response.Headers.Add("Access-Control-Allow-Origin: *");
            Response.Headers.Add("Access-Control-Allow-Headers: *");
            Response.Headers.Add("Server: SharpFlashDetector");
            Response.Headers.Add("X-SimpleFramework: " + Framework.PROG_VERSION);
            await HttpResponseSender.SendStringAsync(HttpContext, resp, ContentType, Encoding.UTF8);
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

        private string GetUserAgent()
        {
            return Request.Headers.Get("User-Agent") ?? "Undefined";
        }

        [Route(HttpVerbs.Get, "/")]
        public async Task Index()
        {
            await SendResponse(PeachPieHelper.index(Program.GetContext(), GetQuery(), GetRemote(), GetUserAgent()));
        }

        [Route(HttpVerbs.Get, "/info")]
        public async Task Info()
        {
            await SendResponse(PeachPieHelper.info(Program.GetContext(), GetQuery(), GetRemote(), GetUserAgent()));
        }

        [Route(HttpVerbs.Get, "/summary")]
        public async Task Summary([QueryField] string lang, [QueryField] string pn)
        {
            await SendResponse(PeachPieHelper.summary(Program.GetContext(), GetQuery(), GetRemote(), GetUserAgent(),
                lang, pn));
        }

        [Route(HttpVerbs.Get, "/decode")]
        public async Task Decode([QueryField] string lang, [QueryField] string pn)
        {
            await SendResponse(PeachPieHelper.decode(Program.GetContext(), GetQuery(), GetRemote(), GetUserAgent(),
                lang, pn));
        }

        [Route(HttpVerbs.Get, "/searchId")]
        public async Task SearchId([QueryField] string lang, [QueryField] string id)
        {
            await SendResponse(PeachPieHelper.searchId(Program.GetContext(), GetQuery(), GetRemote(), GetUserAgent(),
                lang, id));
        }

        [Route(HttpVerbs.Get, "/searchPn")]
        public async Task SearchPn([QueryField] string lang, [QueryField] string pn, [QueryField] int limit)
        {
            await SendResponse(PeachPieHelper.searchPn(Program.GetContext(), GetQuery(), GetRemote(), GetUserAgent(),
                lang, pn, limit));
        }
    }
}