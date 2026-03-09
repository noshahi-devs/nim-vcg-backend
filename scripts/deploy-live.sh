#!/usr/bin/env bash
set -euo pipefail

MODE="${1:-all}"
DEPLOY_BRANCH="${DEPLOY_BRANCH:-main}"
SKIP_SYNC="${SKIP_SYNC:-0}"

FRONTEND_REPO="${FRONTEND_REPO:-/var/www/apps/nim-vcg-frontend}"
BACKEND_REPO="${BACKEND_REPO:-/var/www/apps/nim-vcg-backend}"
FRONTEND_LIVE="${FRONTEND_LIVE:-/var/www/visioncollegegojra/frontend}"
BACKEND_LIVE="${BACKEND_LIVE:-/var/www/visioncollegegojra/backend}"
BACKEND_BUILD_DIR="${BACKEND_BUILD_DIR:-$HOME/nim-vcg-backend-publish}"
FRONTEND_OWNER="${FRONTEND_OWNER:-www-data:www-data}"
BACKEND_OWNER="${BACKEND_OWNER:-administrator:administrator}"
SERVICE_NAME="${SERVICE_NAME:-nim-backend}"

log() {
  printf '[%s] %s\n' "$(date '+%F %T')" "$*"
}

run_sudo() {
  if sudo -n true 2>/dev/null; then
    sudo "$@"
  elif [ -n "${SUDO_PASSWORD:-}" ]; then
    printf '%s\n' "$SUDO_PASSWORD" | sudo -S "$@"
  else
    sudo "$@"
  fi
}

require_command() {
  if ! command -v "$1" >/dev/null 2>&1; then
    printf 'Missing required command: %s\n' "$1" >&2
    exit 1
  fi
}

sync_branch() {
  local repo="$1"
  cd "$repo"
  git fetch origin "$DEPLOY_BRANCH"
  git checkout "$DEPLOY_BRANCH"
  git reset --hard "origin/$DEPLOY_BRANCH"
  git clean -fd
}

deploy_frontend() {
  require_command git
  require_command npm
  require_command rsync

  log "Deploying frontend"
  run_sudo chown -R "$(id -un):$(id -gn)" "$FRONTEND_REPO"

  if [ "$SKIP_SYNC" != "1" ]; then
    sync_branch "$FRONTEND_REPO"
  fi

  cd "$FRONTEND_REPO"
  rm -rf node_modules dist
  npm ci
  npm run build

  local build_dir="dist/wowdash/browser"
  if [ ! -d "$build_dir" ]; then
    build_dir="dist/wowdash"
  fi

  run_sudo mkdir -p "$FRONTEND_LIVE"
  run_sudo rsync -a --delete "$build_dir"/ "$FRONTEND_LIVE"/
  run_sudo chown -R "$FRONTEND_OWNER" "$FRONTEND_LIVE"
}

deploy_backend() {
  require_command git
  require_command dotnet
  require_command rsync

  log "Deploying backend"
  run_sudo chown -R "$(id -un):$(id -gn)" "$BACKEND_REPO"

  if [ "$SKIP_SYNC" != "1" ]; then
    sync_branch "$BACKEND_REPO"
  fi

  cd "$BACKEND_REPO"
  rm -rf "$BACKEND_BUILD_DIR"
  mkdir -p "$BACKEND_BUILD_DIR"
  dotnet restore
  dotnet publish SchoolApiService/SchoolApiService.csproj -c Release -o "$BACKEND_BUILD_DIR"

  run_sudo mkdir -p "$BACKEND_LIVE"
  run_sudo rsync -a --delete "$BACKEND_BUILD_DIR"/ "$BACKEND_LIVE"/
  run_sudo chown -R "$BACKEND_OWNER" "$BACKEND_LIVE"
  run_sudo systemctl restart "$SERVICE_NAME"
  run_sudo systemctl is-active --quiet "$SERVICE_NAME"
}

verify_live() {
  require_command curl
  log "Verifying live site"
  curl -fsSI https://visioncollegegojra.com >/dev/null
}

usage() {
  cat <<'EOF'
Usage:
  deploy-live.sh all
  deploy-live.sh frontend
  deploy-live.sh backend
EOF
}

case "$MODE" in
  all)
    deploy_frontend
    deploy_backend
    ;;
  frontend)
    deploy_frontend
    ;;
  backend)
    deploy_backend
    ;;
  *)
    usage
    exit 1
    ;;
esac

verify_live
log "Deploy complete"
