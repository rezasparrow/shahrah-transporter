FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY bin/Release/net6.0/linux-x64/publish/ .
ENTRYPOINT ["dotnet", "Shahrah.Transporter.Worker.dll"]