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
using iTXTech.FlashDetector;
using Pchp.Core;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = Context.CreateEmpty();
            PeachPieHelper.load(context);
            Console.WriteLine("iTXTech SharpFlashDetector Server");
            Console.WriteLine("Copyright (C) 2020 iTX Technologies");
            Console.WriteLine("https://github.com/iTXTech/SharpFlashDetector");
            Console.WriteLine();
            Console.WriteLine("iTXTech FlashDetector version: " + FlashDetector.getVersion(context));
            Console.WriteLine();

            Console.WriteLine(PeachPieHelper.decode(context, "GG", "GG", true, "NW702"));
        }
    }
}
