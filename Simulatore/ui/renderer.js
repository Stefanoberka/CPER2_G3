document.getElementById('closeBtn').addEventListener('click', () => {
  window.electronAPI.closeWindow()
})
document.getElementById('minBtn').addEventListener('click', () => {
  window.electronAPI.minWindow()
})

let toggle = document.getElementById('toggle')
let on = document.getElementById("on");
let off = document.getElementById("off");

toggle.addEventListener('click', () => {
  if (toggle.checked) {
    console.log('start')
    window.electronAPI.startSwimming()
    off.style.display = "none";
    on.style.display = "block";
  }
  else {
    console.log('stahp')
    window.electronAPI.stopSwimming()
    on.style.display = "none";
    off.style.display = "block";
  }
})
