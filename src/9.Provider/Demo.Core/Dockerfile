#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM microsoft/dotnet:2.1-aspnetcore-runtime-nanoserver-1803 AS base
WORKDIR /app
EXPOSE 53206
EXPOSE 44365

FROM microsoft/dotnet:2.1-sdk-nanoserver-1803 AS build
WORKDIR /src
COPY ["src/Demo.Core/Demo.Core.csproj", "src/Demo.Core/"]
RUN dotnet restore "src/Demo.Core/Demo.Core.csproj"
COPY . .
WORKDIR "/src/src/Demo.Core"
RUN dotnet build "Demo.Core.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Demo.Core.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Demo.Core.dll"]