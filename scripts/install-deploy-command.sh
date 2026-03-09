#!/usr/bin/env bash
set -euo pipefail

TARGET="${1:-/usr/local/bin/nim-vcg-deploy}"
SCRIPT_DIR="$(cd -- "$(dirname -- "${BASH_SOURCE[0]}")" && pwd)"
SOURCE="$SCRIPT_DIR/deploy-live.sh"

run_sudo() {
  if sudo -n true 2>/dev/null; then
    sudo "$@"
  else
    sudo "$@"
  fi
}

run_sudo ln -sf "$SOURCE" "$TARGET"
run_sudo chmod +x "$SOURCE" "$TARGET"

printf 'Installed deploy command at %s\n' "$TARGET"
