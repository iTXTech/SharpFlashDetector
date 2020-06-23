#!/bin/sh

cd Library
chmod +R setup.sh
./setup.sh
cd ..
dotnet publish -c release
