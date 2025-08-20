document.addEventListener('DOMContentLoaded', function() {
    // Auto-hide success messages after 4 seconds
    setTimeout(() => {
        const alert = document.querySelector('.alert-success');
        if (alert) {
            alert.style.display = 'none';
        }
    }, 4000);

    // Handle checkbox form submissions
    const autoSubmitCheckboxes = document.querySelectorAll('input[type="checkbox"][onchange*="submit"]');
    autoSubmitCheckboxes.forEach(checkbox => {
        checkbox.addEventListener('change', function() {
            this.form.submit();
        });
    });

    // Handle fixed expense checkbox updates in edit forms
    const fixedCheckboxes = document.querySelectorAll('input[id*="editFixed-"]');
    fixedCheckboxes.forEach(checkbox => {
        checkbox.addEventListener('change', function() {
            const hiddenField = document.getElementById(this.id.replace('editFixed-', 'editFixedHidden-'));
            if (hiddenField) {
                hiddenField.value = this.checked ? 'true' : 'false';
            }
        });
    });
});

document.addEventListener("DOMContentLoaded", () => {
    // Find all checkboxes that auto-submit
    document.querySelectorAll("input[type=checkbox][data-autosubmit]").forEach(cb => {
        cb.addEventListener("change", function() {
            // Give browser a chance to visually update
            requestAnimationFrame(() => {
                setTimeout(() => {this.form.submit()}, 200)
            });
        });
    });
});