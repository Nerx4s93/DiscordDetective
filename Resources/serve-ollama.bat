@echo off
echo [1/3] Closing all Ollama processes...
taskkill /f /im "ollama app.exe" >nul 2>&1
taskkill /f /im "ollama.exe" >nul 2>&1
echo Processes terminated

echo [2/3] Waiting for cleanup...
timeout /t 2 /nobreak >nul
echo Cleanup completed

echo [3/3] Starting Ollama server...
start /min "" "ollama" serve
timeout /t 5 /nobreak >nul
echo Ollama server started successfully!

echo.
echo Server is running on http://127.0.0.1:11434
echo You can now use models with: ollama run model_name
echo.

exit