FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

# Set the working directory
WORKDIR /src

# Copy the .NET Core project file and restore dependencies
COPY *.csproj ./

RUN dotnet tool install --global dotnet-ef

RUN dotnet restore

# Copy the rest of the application files
COPY . ./

# Build the application
RUN dotnet build

# Set the PATH environment variable to include /root/.net/tools
ENV PATH="/root/.dotnet/tools:${PATH}"

RUN dotnet-ef migrations add "AddIpCameraTable" --context WebApiDbContext
#RUN dotnet run --self-contained false --no-restore -o /app

# FROM mcr.microsoft.com/dotnet/aspnet:7.0

WORKDIR /app

RUN mkdir -p /app/static

RUN cp -ar /src/bin/Debug/netcoreapp7.0/. /app/.

RUN cp -r /src/Static /app/Static

# Expose the port that the application listens on
EXPOSE 5000

# Start the application
ENTRYPOINT ["dotnet", "IntercomCameraWebApi.dll"]


