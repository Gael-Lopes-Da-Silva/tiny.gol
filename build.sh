# todo: build for all platforms https://github.com/gen2brain/raylib-go
GOOS=windows GOARCH=amd64  go build -ldflags="-H=windowsgui" .
