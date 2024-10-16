document.addEventListener("DOMContentLoaded", async () => {
    const Id = Number(sessionStorage.getItem("BikeID"));
    const userName = sessionStorage.getItem("Customer_Name");

    // Fetch bike details from API instead of localStorage
    const response = await fetch(`/api/bikes/${Id}`);
    const display = await response.json();

    if (display) {
        document.getElementById("image").innerHTML = `<img src="${display.Image}" width="100">`;
        document.getElementById("type").innerHTML = `Type: ${display.Type}`;
        document.getElementById("brand").innerHTML = `Brand: ${display.Brand}`;
        document.getElementById("year").innerHTML = `Year: ${display.Year}`;
        document.getElementById("Reg").innerHTML = `Reg No: ${display.Registration_Number}`;
        document.getElementById("Rent").innerHTML = `Rent: ${display.Rent}`;
        document.getElementById("From").innerHTML = `<label>From:</label><input type="date" id="date">`;
        document.getElementById("Time").innerHTML = `<label>Time:</label><input type="time" id="time">`;
        document.getElementById("return").innerHTML = `<button onclick="Request(${Id}, '${userName}')">Request</button>`;
        document.getElementById("To").innerHTML = `<label>Return:</label><p id="return"></p>`;
    } else {
        window.location.href = "User.html";
    }
});

async function Request(id, uName) {
    const return_info = returnDate();
    const dateInput = document.getElementById("date").value;
    const timeInput = document.getElementById("time").value;
    const requestedDate = new Date(dateInput + 'T' + timeInput);

    // Get today's date for comparison
    const today = new Date();
    today.setHours(0, 0, 0, 0);

    // Validate if requestedDate is in the past
    if (requestedDate < today) {
        alert("The selected date cannot be in the past.");
        location.reload();
    }

    console.log("Requested Date:", requestedDate);

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

function returnDate() {
    const date = document.getElementById("date").value;
    const time = document.getElementById("time").value;
    const date_time = date + 'T' + time;
    const date_timeObj = new Date(date_time);
    // Add 24 hours to the requested date for return date
    date_timeObj.setHours(date_timeObj.getHours() + 24);
    const return_details = date_timeObj.toISOString(); // Using ISO string format
    document.getElementById("return").innerHTML = new Date(return_details).toLocaleString();
    return return_details;
}

function getUserId() {
    // Placeholder function to return user ID
    return 1;  // Replace with actual logic to get user ID
}
