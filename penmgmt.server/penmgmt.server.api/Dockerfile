# ARG CONTAINER_VERSION 0.1.0
# ARG APP_FOLDER=/app

FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

# ARG APP_FOLDER
# RUN echo "$APP_FOLDER"
# RUN echo "${APP_FOLDER}"
# RUN echo "$(pwd)"
# RUN echo "$PWD"
# RUN pwd

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["penmgmt.server.api.csproj", "penmgmt.server.api/"]
RUN dotnet restore "penmgmt.server.api/penmgmt.server.api.csproj"
WORKDIR "/src/penmgmt.server.api"
COPY . .
RUN dotnet build "penmgmt.server.api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "penmgmt.server.api.csproj" -c Release -o /app

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

FROM base AS final
WORKDIR /app
RUN mkdir ./../var/lib/penmgmt
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "penmgmt.server.api.dll"]
