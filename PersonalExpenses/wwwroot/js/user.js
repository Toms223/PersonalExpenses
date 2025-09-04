function replaceLimitWithForm() {
    const limitElement = document.getElementById("limit-display");

    if (!limitElement) return;

    // Create a form element
    const form = document.createElement("form");
    form.method = "post";
    form.action = "/User/EditBudgetLimit";
    form.classList.add("budget-form");
    
    const input = document.createElement("input");
    input.type = "number";
    input.name = "Limit";
    input.classList.add("budget-input");
    input.value = limitElement.innerText.replace('€','');
    
    const saveButton = document.createElement("button");
    saveButton.type = "submit";
    saveButton.classList.add("budget-save-button");
    saveButton.textContent = "Save";

    const cancelButton = document.createElement("button");
    cancelButton.type = "button";
    cancelButton.classList.add("budget-save-button","delete");
    cancelButton.textContent = "Cancel";
    cancelButton.onclick = function () {
        form.replaceWith(limitElement);
    }
    
    form.appendChild(input);
    form.appendChild(saveButton);
    form.appendChild(cancelButton);
    
    limitElement.replaceWith(form);
}

function categoryForm(buttonEl) {
    // Create the form
    const form = document.createElement("form");
    form.method = "post";
    form.action = "/Category/CreateCategory"; // adjust this
    form.classList.add("budget-form");

    // Input for category name
    const nameInput = document.createElement("input");
    nameInput.type = "text";
    nameInput.name = "Name";
    nameInput.placeholder = "Category name";
    nameInput.classList.add("budget-input");
    nameInput.required = true;

    // Color picker input
    const colorDiv = document.createElement("div");
    colorDiv.classList.add("color-picker-wrapper");
    const colorLabel = document.createElement("label");
    colorLabel.classList.add("color-picker-label");
    colorLabel.textContent = "Category Color";
    colorLabel.for = "categoryColor";
    
    const colorInput = document.createElement("input");
    colorInput.type = "color";
    colorInput.value = "#000000"; // default black
    colorInput.name = "categoryColor";
    colorInput.oninput = () => {
        hiddenColor.value = colorInput.value.replace("#", "");
    };
    
    colorDiv.appendChild(colorLabel)
    colorDiv.appendChild(colorInput);
    
    const hiddenColor = document.createElement("input");
    hiddenColor.type = "hidden";
    hiddenColor.name = "Color";
    hiddenColor.value = colorInput.value.replace("#", "");

    // Save button
    const saveBtn = document.createElement("button");
    saveBtn.type = "submit";
    saveBtn.textContent = "Save";
    saveBtn.classList.add("budget-save-button");

    // Cancel button
    const cancelBtn = document.createElement("button");
    cancelBtn.type = "button";
    cancelBtn.textContent = "Cancel";
    cancelBtn.classList.add("budget-save-button","delete");
    cancelBtn.onclick = function () {
        form.replaceWith(buttonEl);
    };

    // Build the form
    form.appendChild(nameInput);
    form.appendChild(colorDiv);
    form.appendChild(hiddenColor);
    form.appendChild(saveBtn);
    form.appendChild(cancelBtn);

    // Replace the button with the form
    buttonEl.replaceWith(form);
}