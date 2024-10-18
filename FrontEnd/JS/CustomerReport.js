document.addEventListener("DOMContentLoaded", function() {
    displayCustomerReport();
});

async function displayCustomerReport() {
    try {
        const response = await fetch('http://localhost:5156/api/Report/GetCustomerReport'); // Update this URL to your actual endpoint
        if (!response.ok) {
            const error = await response.text();
            throw new Error(error);
        }
        const customers = await response.json();

        let table = `
            
                <tr>
                    <th>Customer ID</th>
                    <th>Name</th>
                    <th>Rental History (Bike ID)</th>
                </tr>
            `;

        customers.forEach(customer => {
            const rentalHistories = customer.rentalHistories.map(rental => 
                `${rental.bikeId} (from ${new Date(rental.rentedDate).toLocaleDateString()} to ${new Date(rental.to).toLocaleDateString()})`).join('<br>');

            table += `
                <tr>
                    <td>${customer.userId}</td>
                    <td>${customer.userName}</td>
                    <td>${rentalHistories || 'No Rentals'}</td>
                </tr>`;
        });

        table += `</table>`;
        document.getElementById("customer_report").innerHTML = table;

    } catch (error) {
        console.error('Error fetching customer report:', error);
    }
}
