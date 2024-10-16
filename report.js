document.addEventListener("DOMContentLoaded", function() {
    displayCustomerReport();
});

async function displayCustomerReport() {
    try {
        const response = await fetch('/api/your-endpoint'); // Update this URL to your actual endpoint
        if (!response.ok) {
            const error = await response.text();
            throw new Error(error);
        }
        const customers = await response.json();

        let table = `
            <table>
                <tr>
                    <th>Customer ID</th>
                    <th>Name</th>
                    <th>Rental History (Bike ID)</th>
                </tr>`;

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


async function Request(id, uName) {
    const return_info = returnDate();
    const dateInput = document.getElementById("date").value;
    const timeInput = document.getElementById("time").value;
    const requestedDate = new Date(dateInput + 'T' + timeInput);
    const today = new Date();
    today.setHours(0, 0, 0, 0);

    if (requestedDate < today) {
        alert("The selected date cannot be in the past.");
        return;
    }

    const requestBody = {
        userId: getUserId(),  // Implement this function to get the user ID
        bikeId: id,
        requestedDate: requestedDate.toISOString(),
        returnDate: return_info.toISOString(),
    };

    try {
        // Send booking request to the API
        const response = await fetch('/api/bookings/check', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(requestBody)
        });

        if (!response.ok) {
            const error = await response.text();
            throw new Error(error);
        }

        const availability = await response.json();

        if (!availability.isAvailable) {
            alert("Cannot book. The requested date overlaps with an existing booking or is in the past.");
            return;
        }

        // Proceed with booking if available
        const bookingResponse = await fetch('/api/bookings', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(requestBody)
        });

        if (!bookingResponse.ok) {
            const error = await bookingResponse.text();
            throw new Error(error);
        }

        alert("Bike booked successfully!");
    } catch (error) {
        console.error('Error booking bike:', error);
        alert(`Booking failed: ${error.message}`);
    }
}

function getUserId() {
    // Placeholder function to return user ID
    return 1;  // Replace with actual logic to get user ID
}
