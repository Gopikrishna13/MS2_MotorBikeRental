document.addEventListener("DOMContentLoaded", () => {
    fetchReturnBikeDetails();
});

async function fetchReturnBikeDetails() {
    try {
        const response = await fetch('http://localhost:5156/api/Rental/AllReturnBike');
        if (!response.ok) {
            throw new Error('Failed to fetch return bike details');
        }

        const returnBikeDetails = await response.json();
        const bikedetail=Array.isArray(returnBikeDetails)?returnBikeDetails:[returnBikeDetails];
        renderReturnBikeTable(bikedetail);
    } catch (error) {
        console.error('Error fetching return bike details:', error);
        alert('Error fetching return bike details: ' + error.message);
    }
}

function renderReturnBikeTable(returnBikeDetails) {
    let table = `
        <table>
            <tr> 
                <th>Return ID</th>
                <th>User ID</th>
                <th>Bike ID</th>
                <th>Registration Number</th>
                <th>Rented Date</th>
                <th>To</th>
                <th>Due</th>
                <th>Status</th>
                <th>Action</th>
            </tr>`;

    for (const details of returnBikeDetails) {
        table += `
            <tr>
                <td>${details.returnId}</td>
                <td>${details.userId}</td>
                <td>${details.bikeId}</td>
                <td>${details.registrationNumber}</td>
                <td>${new Date(details.rentedDate).toLocaleString()}</td>
                <td>${new Date(details.to).toLocaleString()}</td>
                <td>${new Date(details.due).toLocaleString()}</td>
                <td>${details.status}</td>
                <td>`;

        if (details.status !== "Returned") {
            table += `<button class="action-btn" onclick="handleReturn(${details.returnId})">Returned</button>`;
        } else {
            table += ` `; 
        }

        table += `
                </td>
            </tr>`;
    }

    table += `</table>`;
    document.getElementById("tables").innerHTML = table;
}


async function handleReturn(returnId) {

    try {
        const Id=Number(returnId);
        const response = await fetch(`http://localhost:5156/api/Rental/UpdateReturn?Id=${Id}`, {
            method: 'PUT', 
            headers: { 'Content-Type': 'application/json' },
        });

        if (!response.ok) {
            throw new Error('Failed to return');
        }

        const result = await response.json();
        if (result) {
            alert(`Bike with Return ID ${returnId} has been marked as returned.`);
            fetchReturnBikeDetails(); 
        } else {
            alert('Failed ');
        }
    } catch (error) {
        console.error( error);
        alert('Error: ' + error.message);
    }
}