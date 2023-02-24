set -e 
set -o pipefail

APPLE_ID=$1
APPLE_APP_PASS=$2
APPLE_TEAM_ID=$3
APP_PATH=$4

echo "Archiving for uploading to Notary service..."
/usr/bin/ditto -c -k --sequesterRsrc --keepParent "$APP_PATH" "$APP_PATH.zip"
echo "$APP_PATH.zip has been created."

echo "Submitting to Notary service..."
/usr/bin/xcrun notarytool submit "$APP_PATH.zip" --apple-id "$APPLE_ID" --password "$APPLE_APP_PASS" --team-id "$APPLE_TEAM_ID" --wait
