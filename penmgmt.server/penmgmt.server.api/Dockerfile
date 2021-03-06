# ARG CONTAINER_VERSION 0.1.0
# ARG APP_FOLDER=/app

# Download Runtime
FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
RUN mkdir ./../var/lib/penmgmt
WORKDIR /app
EXPOSE 80

# Metadata - http://label-schema.org/rc1/
LABEL org.label-schema.vendor="NGC1976" \
      org.label-schema.url="https://www.ngc1976.net" \
      org.label-schema.name="Pen Mgmt" \
      org.label-schema.description="Pen Mgmt Server REST API" \    
      org.label-schema.version="0.1.0" \
      org.label-schema.vcs-url=$VCS_URL \
      org.label-schema.vcs-ref=$VCS_REF \
      org.label-schema.build-date=$BUILD_DATE \
      org.label-schema.docker.schema-version="1.0"

# ARG APP_FOLDER
# RUN echo "$APP_FOLDER"
# RUN echo "${APP_FOLDER}"
# RUN echo "$(pwd)"
# RUN echo "$PWD"
# RUN pwd

# Build
FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["penmgmt.server.api.csproj", "penmgmt.server.api/"]
RUN dotnet restore "penmgmt.server.api/penmgmt.server.api.csproj"
WORKDIR "/src/penmgmt.server.api"
COPY . .
RUN dotnet build "penmgmt.server.api.csproj" -c Release -o /app

# Publish
FROM build AS publish
RUN dotnet publish "penmgmt.server.api.csproj" -c Release -o /app

# Setup EntryPoint
FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "penmgmt.server.api.dll"]
