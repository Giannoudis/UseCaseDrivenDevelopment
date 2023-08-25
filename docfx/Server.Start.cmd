@echo off
rem start doc server
docfx --serve --port 5865
rem open in browser
start "" http://localhost:5865/