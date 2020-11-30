FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["WPA_WebForm/WPA_WebForm.csproj", "WPA_WebForm/"]
RUN dotnet restore "WPA_WebForm/WPA_WebForm.csproj"
COPY . .
WORKDIR "/src/WPA_WebForm"
RUN dotnet build "WPA_WebForm.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "WPA_WebForm.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "WPA_WebForm.dll"]