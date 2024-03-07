FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app



FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY src/Core/Chatter.Application/*.csproj Core/Chatter.Application/
COPY src/Core/Chatter.Domain/*.csproj Core/Chatter.Domain/
COPY src/Core/Chatter.Common/*.csproj Core/Chatter.Common/
COPY src/Infrastructure/Chatter.Persistence/*.csproj Infrastructure/Chatter.Persistence/
COPY src/Presentation/Chatter.WebApp/Chatter.WebApp.csproj Presentation/Chatter.WebApp/

RUN dotnet restore Presentation/Chatter.WebApp/*.csproj 

COPY . .
WORKDIR "src/Presentation/Chatter.WebApp"

RUN apt-get update && \
    apt-get install -yq wget && \
    apt-get install -yq gnupg2 && \
    wget -qO- https://deb.nodesource.com/setup_16.x | bash - && \
    apt-get install -yq build-essential nodejs 

RUN npm install


RUN dotnet build "Chatter.WebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Chatter.WebApp.csproj" -c Release -o /app/publish 
RUN cp -R node_modules /app/publish/



FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet Chatter.WebApp.dll