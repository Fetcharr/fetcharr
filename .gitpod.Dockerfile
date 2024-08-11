FROM gitpod/workspace-dotnet:2024-07-14-17-19-51

RUN dotnet tool install Nuke.GlobalTool --global 2>&1

ENV PATH="/home/gitpod/.dotnet/tools:${PATH}"