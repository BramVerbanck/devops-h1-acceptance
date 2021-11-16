#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
USER root
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["HoutKunst.Web/Server/HoutKunst.Web.Server.csproj", "HoutKunst.Web/Server/"]
COPY ["HoutKunst.Web/Client/HoutKunst.Web.Client.csproj", "HoutKunst.Web/Client/"]
COPY ["HoutKunst.Web/Shared/HoutKunst.Web.Shared.csproj", "HoutKunst.Web/Shared/"]
COPY ["HoutKunst.Web/Domain/Domain.csproj", "HoutKunst.Web/Domain/"]
RUN dotnet restore "HoutKunst.Web/Server/HoutKunst.Web.Server.csproj"
COPY . .
WORKDIR "/src/HoutKunst.Web/Server"
RUN dotnet build "HoutKunst.Web.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HoutKunst.Web.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HoutKunst.Web.Server.dll"]
