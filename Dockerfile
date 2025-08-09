# ============ build stage ============
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 只拷贝 Web 项 csproj 并还原（利用层缓存）
COPY ADProject/ADProject.csproj ADProject/
RUN dotnet restore ADProject/ADProject.csproj

# 再拷贝剩余源码并发布
COPY ADProject/ ADProject/
RUN dotnet publish ADProject/ADProject.csproj -c Release -o /app/publish /p:UseAppHost=false

# ============ runtime stage ============
FROM mcr.microsoft.com/dotnet/aspnet:8.0

# 装 MySQL 8
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

# 拷贝初始化 SQL（若无此文件可注释掉）
COPY db/Add_new.sql /docker-entrypoint-initdb.d/Add_new.sql

# 启动脚本：初始化 DB 后启动 ASP.NET
COPY docker/entrypoint.sh /entrypoint.sh
RUN chmod +x /entrypoint.sh

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["/entrypoint.sh"]
