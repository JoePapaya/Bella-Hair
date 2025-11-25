window.printFaktura = (elementId) => {
    const originalContent = document.body.innerHTML;                 // Gem hele siden
    const printElement = document.getElementById(elementId);         // Find fakturaen

    if (!printElement) return;

    // Lav en kopi af fakturaen
    const cloned = printElement.cloneNode(true);

    // Tilføj Tailwind kun i print-mode
    const tailwind = `<script src="https://cdn.tailwindcss.com"></` + `script>`;

    // Lav et helt nyt body-indhold KUN til print
    document.body.innerHTML = `
        ${tailwind}
        <div class="p-8">
            ${cloned.innerHTML}
        </div>
    `;

    // Åbn print-dialogen
    window.print();

    // Efter print (eller Cancel), genskab siden igen
    document.body.innerHTML = originalContent;

    // Genskab Blazor-events
    window.location.reload();
};
