FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["WPA_WebFormNotHttps/WPA_WebFormNotHttps.csproj", "WPA_WebFormNotHttps/"]
RUN dotnet restore "WPA_WebFormNotHttps/WPA_WebFormNotHttps.csproj"
COPY . .
WORKDIR "/src/WPA_WebFormNotHttps"
RUN dotnet build "WPA_WebFormNotHttps.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "WPA_WebFormNotHttps.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "WPA_WebFormNotHttps.dll"]