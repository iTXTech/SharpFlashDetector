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

using System.IO;
using System.Text;
using System.Threading.Tasks;
using EmbedIO;

namespace Server
{
    class HttpResponseSender
    {
        public static async Task SendStringAsync(
            IHttpContext context,
            string content,
            string contentType,
            Encoding encoding)
        {
            context.Response.ContentType = contentType;
            context.Response.ContentEncoding = encoding;
            await using var text = OpenResponseText(context, encoding);
            await text.WriteAsync(content).ConfigureAwait(false);
        }

        public static TextWriter OpenResponseText(IHttpContext context, Encoding encoding)
        {
            context.Response.ContentEncoding = encoding;
            return new StreamWriter(context.Response.OutputStream, encoding);
        }
    }
}