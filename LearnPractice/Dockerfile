﻿FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["LearnPractice/LearnPractice.csproj", "LearnPractice/"]
RUN dotnet restore "LearnPractice/LearnPractice.csproj"
COPY . .
WORKDIR "/src/LearnPractice"
RUN dotnet build "LearnPractice.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LearnPractice.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LearnPractice.dll"]
