FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["/NetCore2-1/PasswordGenerator/WPA_MVC.csproj", "NetCore2-1_PasswordGeneratorWPA_MVC/"]
RUN dotnet restore "/NetCore2-1/PasswordGenerator/WPA_MVC.csproj"
COPY . .
WORKDIR "/src/NetCore2-1/PasswordGenerator"
RUN dotnet build "WPA_MVC.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "WPA_MVC.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "WPA_MVC.dll"]