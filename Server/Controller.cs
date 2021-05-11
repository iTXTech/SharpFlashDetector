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
            return Request.Url.ToString();
        }

        private string GetUserAgent()
        {
            return Request.UserAgent;
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