FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["PaswordGeneratorWPA/PaswordGeneratorWPA.csproj", "PaswordGeneratorWPA/"]
RUN dotnet restore "PaswordGeneratorWPA/PaswordGeneratorWPA.csproj"
COPY . .
WORKDIR "/src/PaswordGeneratorWPA"
RUN dotnet build "PaswordGeneratorWPA.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "PaswordGeneratorWPA.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "PaswordGeneratorWPA.dll"]