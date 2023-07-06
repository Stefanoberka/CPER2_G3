const { contextBridge, ipcRenderer } = require('electron')

contextBridge.exposeInMainWorld('electronAPI', {
  startSwimming: () => ipcRenderer.send('startSwimming'),
  stopSwimming: () => ipcRenderer.send('stopSwimming'),
  closeWindow: () => ipcRenderer.send('closeWindow'),
  minWindow: () => ipcRenderer.send('minimizeWindow')
})