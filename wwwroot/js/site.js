// Initialize all tooltips
window.initializeTooltips = () => {
    console.log('Initializing tooltips...');
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);  // bootstrap must be defined here
    });
};