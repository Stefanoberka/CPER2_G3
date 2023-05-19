const startSwimmingBtn = document.getElementById('startSwimming')
startSwimmingBtn.addEventListener('click', () => {
  window.electronAPI.startSwimming()
});

const stopSwimmingBtn = document.getElementById('stopSwimming')
stopSwimmingBtn.addEventListener('click', () => {
  window.electronAPI.stopSwimming()
});

document.getElementById('closeBtn').addEventListener('click', () => {
  window.electronAPI.closeWindow()
})