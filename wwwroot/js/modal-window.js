// Get the modal
var modal = document.getElementById('myModal')

// Get the image and insert it inside the modal
var img = document.getElementById('yourImageElementId')
var modalImg = document.getElementById('imgModal')

img.onclick = function () {
    modal.style.display = 'block'
    modalImg.src = this.src
}

// Close the modal with the close button
function closeModal() {
    modal.style.display = 'none'
}

// Close the modal if clicked outside of the image
window.onclick = function (event) {
    if (event.target === modal) {
        modal.style.display = 'none'
    }
}
