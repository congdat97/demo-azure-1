# ===========================
# Stage 1: Build & Publish
# =========================== 
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution file (nếu có) để restore dependency
COPY *.sln ./

# Copy từng csproj (để cache restore nuget nhanh hơn)
COPY WebAPI/WebAPI.csproj WebAPI/
COPY Application/Application.csproj Application/
COPY Domain/Domain.csproj Domain/
COPY Infrastructure/Infrastructure.csproj Infrastructure/
COPY Utility/Utility.csproj Utility/

# Restore dependencies
RUN dotnet restore

# Copy toàn bộ source 
COPY . .

# Publish ứng dụng
WORKDIR /src/WebAPI
RUN dotnet publish -c Release -o /app/publish

# ===========================
# Stage 2: Runtime
# ===========================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy output từ build stage
COPY --from=build /app/publish .

# Mở port (nếu muốn explicit)
EXPOSE 8080

# Chạy app
ENTRYPOINT ["dotnet", "WebAPI.dll"]
