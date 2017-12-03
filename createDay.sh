#!/usr/bin/env bash
DAY=$1
echo "Creating ${DAY}"
mkdir $DAY
cd $DAY
dotnet new sln
dotnet new console -lang f# -o $DAY
dotnet new xunit -lang f# -o test
dotnet add test/test.fsproj reference $DAY/$DAY.fsproj
dotnet add test/test.fsproj package fsunit.xunit
dotnet sln add $DAY/$DAY.fsproj
dotnet sln add test/test.fsproj
cat << END
    Voeg dit manueel toe aan test/test.fsproj
    <ItemGroup>
        <DotNetCliToolReference Include="Microsoft.DotNet.Watcher.Tools" Version="1.0.0" />
    </ItemGroup>
END