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

using iTXTech.FlashDetector.Processor;
using Pchp.Core;

namespace Server
{
    internal class SharpProcessor : Processor
    {
        public SharpProcessor(Context ctx) : base(ctx)
        {
        }

        public override bool index(string query, string remote, string ua, string name, PhpAlias c)
        {
            c.Value.Array.Add("server", "SharpFlashDetector Server");
            return true;
        }
    }
}