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

using iTXTech.FlashDetector.Processor;
using Pchp.Core;

namespace Server
{
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
}
