document.addEventListener("DOMContentLoaded", () => {
    fetchRequestDetails();
});

async function fetchRequestDetails() {
    try {
        const response = await fetch('http://localhost:5156/api/Rental/AllRequest');
        if (!response.ok) {
            throw new Error('Failed to fetch request details');
        }

        const request_details = await response.json();
        const details = Array.isArray(request_details) ? request_details : [request_details];
        renderTable(details);
    } catch (error) {
        console.error('Error fetching request details:', error);
        alert('Error fetching request details: ' + error.message);
    }
}

function renderTable(request_details) {
    let table = `
        <table>
            <tr> 
                <th>Request ID</th>
                <th>BikeID</th>
                <th>Registration Number</th>
                <th>User ID</th>
                <th>From</th>
                <th>To</th>
                <th>Action</th>
            </tr>`;

    for (const details of request_details) {
        table += `
            <tr>
                <td>${details.requestId}</td>
                <td>${details.bikeId}</td>
                <td>${details.registrationNumber}</td>
                <td>${details.userId}</td>
                <td>${details.rentedDate}</td>
                <td>${details.returnDate}</td>
                <td>
                    <div class="action" data-id="${details.requestId}">`;

        if (details.status === "Waiting") {
            table += `
                <button class="action-btn" onclick="AcceptRequest(${details.requestId})">Accept</button>
                <button class="action-btn" onclick="DeclineRequest(${details.requestId})" style="background-color:red">Decline</button>`;
        } else if (details.status === 1) {
            table += Accepted;
        } else if (details.status === -1) {
            table += Declined;
        }

        table += `
                    </div>
                </td>
            </tr>`;
    }

    table +=` </table>`;
    document.getElementById("request_details").innerHTML = table;
}

async function AcceptRequest(reqID) {
    try {
        const requestId = Number(reqID);
        const response = await fetch(`http://localhost:5156/api/Rental/UpdateRequest?code=1&Id=${requestId}`, {
            method: 'PUT', // Change POST to PUT
            headers: { 'Content-Type': 'application/json' },
        });

        if (!response.ok) {
            throw new Error('Failed to update request status');
        }

        const result = await response.json();
        if (result) {
            updateRequestStatus(reqID, 1, "Accepted");
            alert(`Status Accepted for ${reqID}`);
        } else {
            alert('Failed to accept request');
        }
    } catch (error) {
        console.error('Error accepting request:', error);
        alert('Error accepting request: ' + error.message);
    }
}

async function DeclineRequest(reqID) {
    try {
        const response = await fetch(`http://localhost:5156/api/Rental/UpdateRequest?code=-1&Id=${reqID}`, {
            method: 'PUT', // Change POST to PUT
            headers: { 'Content-Type': 'application/json' },
        });

        if (!response.ok) {
            throw new Error('Failed to update request status');
        }

        const result = await response.json();
        if (result) {
            updateRequestStatus(reqID, -1, "Declined");
            alert(`Status Declined for ${reqID}`);
        } else {
            alert('Failed to decline request');
        }
    } catch (error) {
        console.error('Error declining request:', error);
        alert('Error declining request: ' + error.message);
    }
}



function updateRequestStatus(reqID, status, displayStatus) {
    const actionDiv = document.querySelector(`.action[data-id="${reqID}"]`);
    if (actionDiv) {
        actionDiv.innerHTML = displayStatus;
    }
}