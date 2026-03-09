# Backend Auto Deploy Setup

This repository now has a GitHub Actions workflow at `.github/workflows/deploy.yml`.

## 1) One-time VPS setup (Ubuntu)

Run on your VPS:

```bash
sudo apt update
sudo apt install -y git curl
wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
sudo apt update
sudo apt install -y dotnet-sdk-8.0 aspnetcore-runtime-8.0
```

Clone the repo on VPS (choose your own path):

```bash
mkdir -p /var/www/apps
cd /var/www/apps
git clone https://github.com/noshahi-devs/nim-vcg-backend.git
```

## 2) Systemd service example

Create service file:

```bash
sudo nano /etc/systemd/system/nim-backend.service
```

Example content:

```ini
[Unit]
Description=NIM VCG Backend API
After=network.target

[Service]
WorkingDirectory=/var/www/visioncollegegojra/backend
ExecStart=/usr/bin/dotnet /var/www/visioncollegegojra/backend/SchoolApiService.dll
Restart=always
RestartSec=5
SyslogIdentifier=nim-backend
User=administrator
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=ASPNETCORE_URLS=http://127.0.0.1:5000

[Install]
WantedBy=multi-user.target
```

Enable service:

```bash
sudo systemctl daemon-reload
sudo systemctl enable nim-backend
sudo systemctl start nim-backend
```

This example assumes nginx proxies `/api/` to `http://127.0.0.1:5000`. `launchSettings.json` is only for local development and is not used by `systemd` when you run the published DLL.

Allow deploy user to restart service without password:

```bash
sudo visudo
```

Add:

```text
deployuser ALL=(ALL) NOPASSWD:/bin/systemctl restart nim-backend,/bin/systemctl is-active nim-backend
```

## 3) GitHub repository secrets (backend repo)

Go to: `Settings -> Secrets and variables -> Actions -> New repository secret`

Add:

- `VPS_HOST` = VPS IP (example `93.127.141.27`)
- `VPS_PORT` = `22`
- `VPS_USER` = VPS SSH username (recommended: non-root deploy user)
- `VPS_SSH_PRIVATE_KEY` = private key content of deploy user
- `BACKEND_APP_PATH` = VPS clone path (example `/var/www/apps/nim-vcg-backend`)
- `BACKEND_PUBLISH_PATH` = publish output path (example `/var/www/visioncollegegojra/backend`)
- `BACKEND_SERVICE_NAME` = systemd service name (example `nim-backend`)
- `DEPLOY_BRANCH` = `main`

## 4) Deploy flow

You push via GitHub Desktop -> workflow runs -> VPS pulls latest code -> publishes API -> restarts systemd service -> live API updates.
