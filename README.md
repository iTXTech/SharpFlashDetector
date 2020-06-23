# iTXTech SharpFlashDetector

Integrate `iTXTech FlashDetector` into `.NET` projects.

## Library

The `SharpFlashDetector Library` shows how to integrate `FlashDetector` into a `.NET` project.

Execute `setup.bat` to fetch necessary files.

## Server

The `SharpFlashDetector Server` uses [EmbedIO](https://github.com/unosquare/embedio) to wrap `SharpFlashDetector Library` into a high performance web server.

Execute `dotnet publish -c release -r win-x64 /p:PublishSingleFile=true` to build single executable file.

## Build

### Requirements:

* [.NET Core 3.1+](https://dotnet.microsoft.com/download/dotnet-core)
* [PHP 7.2+](https://www.php.net)

## License

    Copyright (C) 2020 iTX Technologies

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at

        http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.
