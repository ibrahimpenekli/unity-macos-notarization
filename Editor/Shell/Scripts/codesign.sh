set -e 
set -o pipefail

ENTITLEMENTS=$1
DEVELOPER_CERT_ID=$2
APP_PATH=$3

echo "Code signing..."
/bin/chmod -R a+xr "$APP_PATH"
/usr/bin/codesign --deep --force --verify --verbose --timestamp --options runtime --entitlements "$ENTITLEMENTS" --sign "$DEVELOPER_CERT_ID" "$APP_PATH"
