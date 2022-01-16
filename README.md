# iTXTech SharpFlashDetector

Integrate `iTXTech FlashDetector` into `.NET` projects.

## Library

The `SharpFlashDetector Library` shows how to integrate `FlashDetector` into a `.NET` project.

Execute `setup.bat` to fetch necessary files.

## Server

The `SharpFlashDetector Server` uses [EmbedIO](https://github.com/unosquare/embedio) to wrap `SharpFlashDetector Library` into a high performance web server.

### Generate `ReadyToRun` Single Executable for Windows AMD64

`dotnet publish -c release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeAllContentForSelfExtract=true -p:PublishReadyToRun=true -p:PublishTrimmed=true`

### Generate Smaller Single Executable for Windows AMD64

`dotnet publish -c release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeAllContentForSelfExtract=true -p:PublishTrimmed=true`

## Build

### Requirements:

* [.NET 5](https://dotnet.microsoft.com/download/dotnet)
* [PHP 7.3+](https://www.php.net)

## License

    Copyright (C) 2020-2022 iTX Technologies

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU Affero General Public License as
    published by the Free Software Foundation, either version 3 of the
    License, or (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Affero General Public License for more details.

    You should have received a copy of the GNU Affero General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.

