#!/usr/bin/env bash
set -euo pipefail

# 初始化 MySQL 数据目录（首次）
if [ ! -d "/var/lib/mysql/mysql" ]; then
  echo "[entrypoint] Initializing MySQL data dir..."
  mysqld --initialize-insecure --user=mysql
fi

# 启动 MySQL 到后台
echo "[entrypoint] Starting MySQL..."
mysqld --user=mysql --daemonize --socket=/var/run/mysqld/mysqld.sock

# 等待 MySQL 就绪
echo -n "[entrypoint] Waiting for MySQL to be ready"
for i in {1..60}; do
  if mysqladmin ping --silent; then
    echo " - ready"
    break
  fi
  echo -n "."
  sleep 1
done

# 首次导入：如果还没有 adproject 库，则执行初始化 SQL
if ! mysql -uroot -e "USE adproject" >/dev/null 2>&1; then
  if [ -f "/docker-entrypoint-initdb.d/Add_new.sql" ]; then
    echo "[entrypoint] Importing initial SQL..."
    mysql -uroot < /docker-entrypoint-initdb.d/Add_new.sql
  else
    echo "[entrypoint] WARNING: /docker-entrypoint-initdb.d/Add_new.sql not found, skipping import."
  fi
fi

# 运行 ASP.NET
echo "[entrypoint] Starting ASP.NET app..."
exec dotnet ADProject.dll
