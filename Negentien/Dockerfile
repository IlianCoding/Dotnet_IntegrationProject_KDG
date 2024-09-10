FROM mcr.microsoft.com/dotnet/sdk:7.0
COPY UI-MVC /src/UI-MVC
COPY DAL /src/DAL
COPY BL /src/BL
COPY Domain /src/Domain
COPY service-account-key.json /app/service-account-key.json
COPY integratieproject-team19-12a263e2b620.json /app/integratieproject-team19-12a263e2b620.json
WORKDIR /src
RUN apt-get update \
    && apt-get install -y curl gnupg2 \
    && curl -fsSL https://deb.nodesource.com/setup_18.x | bash -
RUN apt-get install -y nodejs
RUN dotnet publish UI-MVC --output /app

FROM mcr.microsoft.com/dotnet/aspnet:7.0
COPY --from=0 /app /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://%2A:8080/
USER 1000
WORKDIR /app
ENTRYPOINT ["dotnet", "UI-MVC.dll"]
                      