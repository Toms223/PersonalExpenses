function filterExpenses(input) {
    const query = input.value.trim().toLowerCase();
    const rows = document.querySelectorAll(".expense-list > div:nth-child(3n+1)");
    // every 1st cell of a 3-col row = expense name

    rows.forEach(cell => {
        const name = cell.textContent.trim().toLowerCase();
        const rowCells = [cell, cell.nextElementSibling, cell.nextElementSibling?.nextElementSibling];

        if (!query || name.includes(query)) {
            rowCells.forEach(c => c && (c.style.display = ""));
        } else {
            rowCells.forEach(c => c && (c.style.display = "none"));
        }
    });
}
function exportExpensesToCSV() {
    const rows = [];
    // Add header row
    rows.push(['Name', 'Date', 'Category', 'Amount']);

    const expenseList = document.querySelector('.expense-list');
    const divs = Array.from(expenseList.children);

    // Each expense uses 5 consecutive divs
    for (let i = 5; i < divs.length; i += 5) { // skip first 3 header divs
        const name = divs[i].querySelector('p')?.innerText.trim() || '';
        const date = divs[i+1].querySelector('p')?.innerText.trim() || '';
        const category = divs[i + 2].querySelector('label.selected')?.innerText.trim() || '';
        const amount = divs[i + 3].querySelector('p')?.innerText.trim() || '';
        rows.push([name, date, category, amount]);
    }

    // Convert to CSV string
    const csvContent = rows.map(e => e.map(v => `"${v}"`).join(',')).join('\n');

    // Trigger download
    const blob = new Blob([csvContent], { type: 'text/csv;charset=utf-8;' });
    const link = document.createElement('a');
    const url = URL.createObjectURL(blob);
    const today = new Date();
    const options = { month: "long", year: "numeric" };
    const monthYear = today.toLocaleString("en-US", options);
    link.setAttribute('href', url);
    link.setAttribute('download', `${monthYear} Expenses.csv`);
    link.style.visibility = 'hidden';
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}

function enableEdit(id) {
    const item = document.querySelector(`div[data-expense-id="${id}"]:has(button)`);
    const amount = item.previousElementSibling;
    const category = amount.previousElementSibling;
    const label = category.querySelector('.category-label');
    const changer = category.querySelector('.category-changer');
    const date = category.previousElementSibling;
    const name = date.previousElementSibling;
    const continuous = name.dataset.expenseContinuous;
    const fixed = name.dataset.expenseFixed;
    const period = name.dataset.expensePeriod;

    // Save original contents in data attributes
    item.dataset.original = item.innerHTML;
    amount.dataset.original = amount.innerHTML;
    category.dataset.original = category.innerHTML;
    name.dataset.original = name.innerHTML;
    date.dataset.original = date.innerHTML;

    // Replace item div with Apply and Cancel buttons
    item.innerHTML = `
        <div class="edit-action-buttons">
            <button class="button" onclick="applyEdit(${id})">Apply</button>
            <button class="button delete" onclick="cancelEdit(${id})">Cancel</button>
        </div>
    `;

    // Replace amount div with numeric input (cents allowed)
    const currentAmount = parseFloat(amount.textContent.replace(',', '.')) || 0;
    amount.innerHTML = `
        <input class="budget-input"  id="expense-edit-amount" type="number" step="0.01" value="${currentAmount.toFixed(2)}">
    `;

    if(label) {
        label.hidden = true;
        changer.hidden = false;
    } else {
        changer.hidden = false;
    }

    // Replace name div with text input
    const currentName = name.textContent.trim();
    name.innerHTML = `
        <div class="edit-expense">
            <div class="edit-expense-type">
                <label>Name:</label>
                <input class="budget-input" type="text" value="${currentName}">
            </div>
            <div class="edit-expense-type">
                <label>Period:</label>
                <input class="budget-input" type="number" step="1" value="${period}">
            </div>
            <div class="edit-expense-type">
                <label id="coninuousSwitch" for="continuous">Continuous:</label>
                <label id="continuousSwitch" class="switch">
                    <input type="checkbox" id="continuous" name="continuous" value="${continuous}" ${continuous === "True" ? "checked" : ""}>
                    <span class="slider"></span>
                </label>
                <label id="fixedLabel" for="fixedExpense">Fixed:</label>
                <label id="fixedSwitch" class="switch">
                    <input type="checkbox" id="fixedExpense" name="fixedExpense" value="${fixed}" ${fixed === "True" ? "checked" : ""}>
                    <span id="fixedSlider" class="slider"></span>
                </label>
            </div>
        </div>
    `;
    const currentDate = date.textContent.trim().replaceAll('/','-');
    date.innerHTML = `
        <input class="budget-input"  type="date" value=${dayjs(currentDate).format('YYYY-MM-DD')}>
    `
}

function applyEdit(id) {
    const item = document.querySelector(`div[data-expense-id="${id}"]:has(button)`);
    const amount = item.previousElementSibling;
    const category = amount.previousElementSibling;
    const date = category.previousElementSibling;
    const name = date.previousElementSibling;
    const changer = category.querySelector('.category-changer')
    const selectedLabel= changer.querySelector('label.selected');

    const selectedCategory = selectedLabel?.querySelector('input').value;

    const nameInput = name.querySelector('input[type="text"]').value;
    const dateInput = date.querySelector('input').value;
    const amountInput = amount.querySelector('input').value;
    const continuous = name.querySelector('#continuousSwitch > input').checked;
    const fixed = name.querySelector('#fixedSwitch > input').checked;
    const period = name.querySelector('input[type="number"]').value;


    fetch(`/Expenses/EditExpense?Id=${id}&Name=${nameInput}&Amount=${amountInput}&Date=${dateInput}&Continuous=${continuous}&FixedExpense=${fixed}&Period=${period}&CategoryId=${selectedCategory}`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
    })
        .then(response => {
            if (!response.ok) {
                throw new Error("Network response was not ok " + response.statusText);
            }
            // No need to read response body, just reload
            location.reload();
        })
        .catch(error => {
            console.error("Error:", error);
        });


}

function cancelEdit(id) {
    const item = document.querySelector(`div[data-expense-id="${id}"]:has(button)`);
    const amount = item.previousElementSibling;
    const category = amount.previousElementSibling;
    const date = category.previousElementSibling;
    const name = date.previousElementSibling;

    // Restore original content
    item.innerHTML = item.dataset.original;
    amount.innerHTML = amount.dataset.original;
    category.innerHTML = category.dataset.original;
    date.innerHTML = date.dataset.original;
    name.innerHTML = name.dataset.original;
}
document.addEventListener("DOMContentLoaded", () => {
    const expenseItems = [...document.querySelectorAll('.expense-list > div')].slice(5);
    document.querySelectorAll('.category-filter').forEach(option => {
        option.addEventListener('click', function(e) {
            e.preventDefault();

            const radioInput = this.querySelector('input[type="radio"]');

            if (this.classList.contains('selected')) {
                this.classList.remove('selected');
                radioInput.checked = false;
                expenseItems.forEach(item => {
                    item.hidden = false;
                });
            } else {
                document.querySelectorAll('.category-filter').forEach(opt => {
                    opt.classList.remove('selected');
                });

                this.classList.add('selected');

                radioInput.checked = true;
                expenseItems.forEach(item => {
                    if(item.dataset.expenseCategoryId !== radioInput.value) {
                        item.hidden = true;
                    }
                });
            }
        });
    });
    document.querySelectorAll('.category-changer').forEach(option => {
        option.addEventListener('click', function(e) {
            e.preventDefault();

            const radioInput = this.querySelector('div > input[type="radio"]');

            if (this.classList.contains('selected')) {
                this.classList.remove('selected');
                radioInput.checked = false;
            } else {
                document.querySelectorAll('.category-filter').forEach(opt => {
                    opt.classList.remove('selected');
                });

                this.classList.add('selected');

                radioInput.checked = true;
            }
        });
    });
    const toast = document.querySelector(".alert-success");
    
    if (toast && toast.textContent.trim() !== "") {
        toast.classList.add("show");
        setTimeout(() => {
            toast.classList.remove("show");
        }, 4000);
    }
});