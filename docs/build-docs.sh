#!/usr/bin/env bash
set -euo pipefail
ROOT="$(cd "$(dirname "$0")/.." && pwd)"
MDWEB_CLI="${MDWEB_CLI:-$ROOT/../MDWeb/src/MDWeb.Cli}"
DOC_DIR="$ROOT/docs"
BUILD_DIR="$DOC_DIR/_build"

if [ ! -e "$MDWEB_CLI" ]; then
  echo "MDWeb CLI not found." >&2
  echo "Clone MDWeb as a sibling repo (../MDWeb) or set MDWEB_CLI to your MDWeb.Cli project path." >&2
  exit 1
fi

dotnet run --project "$MDWEB_CLI" -- \
  --source "$DOC_DIR/pages" \
  --output "$BUILD_DIR" \
  --theme "$DOC_DIR/theme" \
  --title "PillowNet" \
  --description "C# wrapper for Pillow via CSnakes" \
  --footer "<p>PillowNet · MIT License · <a href=\"https://github.com/hoihky/pillownet\">GitHub</a> · Generated with <a href=\"https://github.com/hoihky/MDWeb\">MDWeb</a></p>"

for html in "$BUILD_DIR"/*.html; do
  [ -f "$html" ] || continue
  name="$(basename "$html")"
  if [ "$name" != "index.html" ]; then
    cp "$html" "$DOC_DIR/$name"
  fi
done

echo "Built HTML pages in docs/"
