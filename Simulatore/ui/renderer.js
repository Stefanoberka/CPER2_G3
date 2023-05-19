document.getElementById('closeBtn').addEventListener('click', () => {
  window.electronAPI.closeWindow()
})

let toggle = document.getElementById('toggle')

toggle.addEventListener('click', () => {
  if (toggle.checked) {
    console.log('start')
    window.electronAPI.startSwimming()
  }
  else {
    console.log('stahp')
    window.electronAPI.stopSwimming()
  }
})