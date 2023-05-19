// Questo deve sempre stare in cima per caricare le variabili d'ambiente all'avvio:
require('dotenv').config()

// costanti:
const clockID = process.env.CLOCK_ID ?? 'placeholder-id (should be set in .env)';
const { Data, swim } = require('./swimLib')

// MAIN
console.log('Activities for clock:', clockID)
const { app, BrowserWindow, ipcMain } = require('electron')
const path = require('path');
let swimInstance

function createWindow() {
    const window = new BrowserWindow({
        width: 800,
        height: 900,
        frame: false,
        transparent: true,
        webPreferences: {
            nodeIntegration: true,
            // enableRemoteModule: true,
            preload: (path.join(__dirname, 'preload.js'))
        },
    })

    ipcMain.on('startSwimming', (event, args) => {
        console.log('start')
        swimInstance = swim(clockID)
    })
    ipcMain.on('stopSwimming', (event, args) => {
        console.log('stop')
        clearInterval(swimInstance)
    })
    ipcMain.on('closeWindow', (event, args) => window.close())

    window.loadFile(path.join(__dirname, 'ui/arm.html'))
}

app.whenReady().then(() => {
    // ipcMain.on('start-swimming', handleStartSwimming)
    createWindow()

    app.on('activate', function () {
        if (BrowserWindow.getAllWindows().length === 0) createWindow()
    })
})

app.on('window-all-closed', function () {
    if (process.platform !== 'darwin') {
        app.quit()
        process.exit()
    }
})