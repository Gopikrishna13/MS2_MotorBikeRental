document.addEventListener("DOMContentLoaded", () => {
    const apiurl = 'http://localhost:5156/api/Report/InventoryReport'; // Update this with your actual API endpoint

    fetch(apiurl)
        .then(response => {
            if (!response.ok) {
                throw new Error('Response was not ok');
            }
            return response.json();
        })
        .then(data => {
            populateTable(data);
        })
        .catch(error => {
            console.error('Fetch error:', error);
        });
});

function populateTable(data) {
    const tableBody = document.getElementById('bikeInventoryTable').getElementsByTagName('tbody')[0];
    let rows = '';
    data.forEach(item => {
        rows += `
            <tr>
                <td>${item.bikeId}</td>
                <td>${item.brand}</td>
                <td>${item.model}</td>
                <td>${item.registrationNumber}</td>
                <td>${item.status}</td>
            </tr>
        `;
    });
    tableBody.innerHTML = rows;
}



