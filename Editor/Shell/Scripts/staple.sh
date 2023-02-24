set -e 
set -o pipefail

APP_PATH=$1

echo "Stapling..."
/usr/bin/xcrun stapler staple -v "$APP_PATH"
/usr/sbin/spctl -a -v "$APP_PATH"
/usr/bin/codesign --test-requirement="=notarized" --verify --verbose "$APP_PATH"