// Questo deve sempre stare in cima per caricare le variabili d'ambiente all'avvio:
require('dotenv').config()

// costanti:
const clockID = process.env.CLOCK_ID ?? 'placeholder-id (should be set in .env)';
const { swim } = require('./lib/swimLib')

// MAIN
console.log('Activities for clock:', clockID)
const { app, BrowserWindow, ipcMain } = require('electron')
const path = require('path');
let swimInstance;

function createWindow() {
    const window = new BrowserWindow({
        width: 600,
        height: 420,
        frame: false,
        transparent: true,
        resizable: false,
        webPreferences: {
            nodeIntegration: true,
            preload: (path.join(__dirname, 'lib/preload.js'))
        }
    });

    ipcMain.on('startSwimming', (event, args) => {
        console.log('Started swimming...')
        clearInterval(swimInstance)
        swimInstance = swim(clockID)
    });
    ipcMain.on('stopSwimming', (event, args) => {
        console.log('Stopped.')
        clearInterval(swimInstance)
    });
    ipcMain.on('closeWindow', (event, args) => window.close());
    ipcMain.on('minimizeWindow', (event, args) => window.minimize());

    window.loadFile(path.join(__dirname, 'ui/arm.html'));
};

app.whenReady().then(() => {
    createWindow();

    app.on('activate', function () {
        if (BrowserWindow.getAllWindows().length === 0) createWindow();
    });
});

app.on('window-all-closed', function () {
    if (process.platform !== 'darwin') {
        console.log('Exiting...');
        app.quit();
        process.exit();
    }
});
