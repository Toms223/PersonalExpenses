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

function categoryForm(button) {
    const form = document.createElement("form");
    form.method = "post";
    form.action = "/Category/CreateCategory"; // adjust this
    form.classList.add("budget-form");
    
    const nameInput = document.createElement("input");
    nameInput.type = "text";
    nameInput.name = "Name";
    nameInput.placeholder = "Category name";
    nameInput.classList.add("budget-input");
    nameInput.required = true;
    
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

    const saveBtn = document.createElement("button");
    saveBtn.type = "submit";
    saveBtn.textContent = "Save";
    saveBtn.classList.add("budget-save-button");
    
    const cancelBtn = document.createElement("button");
    cancelBtn.type = "button";
    cancelBtn.textContent = "Cancel";
    cancelBtn.classList.add("budget-save-button","delete");
    cancelBtn.onclick = function () {
        form.replaceWith(button);
    };
    
    form.appendChild(nameInput);
    form.appendChild(colorDiv);
    form.appendChild(hiddenColor);
    form.appendChild(saveBtn);
    form.appendChild(cancelBtn);
    
    button.replaceWith(form);
}

function editCategory(button) {
    const id = button.querySelector("input").value;
    const editDiv = document.createElement("div");
    editDiv.classList.add("edit");
    const editBox = document.createElement("div");
    editBox.classList.add("box", "edit-box");
    const form = document.createElement("form");
    form.method = "post";
    form.action = "/Category/EditCategory";
    form.classList.add("budget-form");
    
    const nameInput = document.createElement("input");
    nameInput.type = "text";
    nameInput.name = "Name";
    nameInput.value = button.innerText.trim();
    nameInput.classList.add("budget-input");
    nameInput.required = true;

    const colorDiv = document.createElement("div");
    colorDiv.classList.add("color-picker-wrapper");
    const colorLabel = document.createElement("label");
    colorLabel.classList.add("color-picker-label");
    colorLabel.textContent = "Category Color";
    colorLabel.for = "categoryColor";

    const color = getComputedStyle(button).getPropertyValue("--category-color").trim();
    const colorInput = document.createElement("input");
    colorInput.type = "color";
    colorInput.value = color;
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
    
    const hiddenId = document.createElement("input");
    hiddenId.type = "hidden";
    hiddenId.name = "Id";
    hiddenId.value = id;
    
    const saveBtn = document.createElement("button");
    saveBtn.type = "submit";
    saveBtn.textContent = "Save";
    saveBtn.classList.add("budget-save-button");

    const cancelBtn = document.createElement("button");
    cancelBtn.type = "button";
    cancelBtn.textContent = "Cancel";
    cancelBtn.classList.add("budget-save-button","delete");
    cancelBtn.onclick = function () {
        document.body.removeChild(editDiv);
    };
    
    form.appendChild(nameInput);
    form.appendChild(colorDiv);
    form.appendChild(hiddenColor);
    form.appendChild(hiddenId);
    form.appendChild(saveBtn);
    form.appendChild(cancelBtn);
    
    const deleteButton = document.createElement('button');
    deleteButton.classList.add('button','delete');
    deleteButton.textContent = "Delete";
    deleteButton.addEventListener('click', function(){
        deleteCategory(id)
    })
    
    editBox.appendChild(form)
    editBox.appendChild(deleteButton);
    editDiv.appendChild(editBox);
    document.body.appendChild(editDiv);
}


function deleteCategory(id) {
    fetch(`/Category/DeleteCategory?Id=${id}`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
    })
        .then(response => {
            if (!response.ok) {
                throw new Error("Network response was not ok " + response.statusText);
            }
            location.reload();
        })
        .catch(error => {
            console.error("Error:", error);
        });
}

document.addEventListener("DOMContentLoaded", () => {
    document.querySelectorAll('.category-edit').forEach(option => {
        option.addEventListener('click', function(e) {
            e.preventDefault();
            editCategory(option)
        });
    });
});