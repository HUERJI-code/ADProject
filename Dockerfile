# ============ build stage ============
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ADProject.sln ./
COPY ADProject/ ADProject/
RUN dotnet restore
RUN dotnet publish ADProject/ADProject.csproj -c Release -o /app/publish

# ============ runtime stage ============
FROM mcr.microsoft.com/dotnet/aspnet:8.0

# 装 MySQL 8
# 安装工具并添加 MySQL APT 源
RUN apt-get update && \
    DEBIAN_FRONTEND=noninteractive apt-get install -y wget gnupg lsb-release && \
    wget https://dev.mysql.com/get/mysql-apt-config_0.8.32-1_all.deb && \
    DEBIAN_FRONTEND=noninteractive dpkg -i mysql-apt-config_0.8.32-1_all.deb && \
    rm -f mysql-apt-config_0.8.32-1_all.deb && \
    apt-get update && \
    DEBIAN_FRONTEND=noninteractive apt-get install -y mysql-server && \
    rm -rf /var/lib/apt/lists/*


# 准备 MySQL 数据目录
RUN mkdir -p /var/run/mysqld && chown -R mysql:mysql /var/run/mysqld \
    && rm -rf /var/lib/mysql && mkdir -p /var/lib/mysql && chown -R mysql:mysql /var/lib/mysql

# 拷贝应用
WORKDIR /app
COPY --from=build /app/publish/ ./

# 拷贝初始化 SQL
COPY db/Add_new.sql /docker-entrypoint-initdb.d/Add_new.sql

# 启动脚本：第一次启动初始化DB并导入SQL，随后起 ASP.NET
COPY docker/entrypoint.sh /entrypoint.sh
RUN chmod +x /entrypoint.sh

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["/entrypoint.sh"]
