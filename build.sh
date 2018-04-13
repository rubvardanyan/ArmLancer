#!/bin/bash
set -ev
dotnet restore
dotnet build
dotnet test ArmLancer.UnitTest --no-build
dotnet build -c Release