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

# 安装 SSH 与 mysql 客户端（便于容器内测试）
RUN apt-get update && \
    DEBIAN_FRONTEND=noninteractive apt-get install -y --no-install-recommends openssh-server mysql-client && \
    mkdir -p /var/run/sshd && \
    echo "root:TempStrongP@ssw0rd" | chpasswd && \
    sed -i 's/^#\?Port .*/Port 2222/' /etc/ssh/sshd_config && \
    sed -i 's/^#\?PasswordAuthentication .*/PasswordAuthentication yes/' /etc/ssh/sshd_config && \
    sed -i 's/^#\?PermitRootLogin .*/PermitRootLogin yes/' /etc/ssh/sshd_config && \
    sed -i 's@session\s\+required\s\+pam_loginuid.so@session optional pam_loginuid.so@g' /etc/pam.d/sshd && \
    rm -rf /var/lib/apt/lists/*

# 拷贝应用
WORKDIR /app
COPY --from=build /app/publish/ ./

# Web 端口与 SSH 端口
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080 2222

# 同时启动 SSH 与 ASP.NET 应用
CMD /usr/sbin/sshd -D & dotnet ADProject.dll
