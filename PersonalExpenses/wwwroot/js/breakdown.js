function toggleDropdown() {
    const menu = document.getElementById("partialContainer");
    menu.classList.toggle("show");
    const toggleButton = document.getElementById("toggleAdd");
    setTimeout(() => {window.scrollTo({
        top: document.body.scrollHeight,
        behavior: "smooth" // use "auto" if you don't want animation
    })
        if(toggleButton.innerText === "Add Expense") {
            toggleButton.innerText = "Close";
        } else {
            toggleButton.innerText = "Add Expense";
        }
    }, 250)

}