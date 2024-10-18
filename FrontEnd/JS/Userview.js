


document.addEventListener("DOMContentLoaded", () => {
    const Id = Number(sessionStorage.getItem("BikeID"));
    const userName = sessionStorage.getItem("Customer_Name");
    const apiurl = `http://localhost:5156/api/Bike/GetById?id=${Id}`;

    fetchUserId(userName)
        .then(userId => {
            if (userId) {
                fetchBikeData(apiurl, userId);
            } else {
                console.error("User not found!");
                window.location.href = "User.html";
            }
        })
        .catch(error => {
            console.error('Error fetching user ID:', error);
            window.location.href = "User.html";
        });
});

async function fetchUserId(username) {
    const userApiUrl = `http://localhost:5156/api/User/GetByUserName?username=${username}`;
    const response = await fetch(userApiUrl);
    if (!response.ok) {
        throw new Error('Failed to fetch user data');
    }
    const userData = await response.json();
    return userData.userId;
}

async function fetchBikeData(apiurl, userId) {
    const response = await fetch(apiurl);
    if (!response.ok) {
        throw new Error('Response was not ok');
    }
    const bike = await response.json();
    if (bike) {
        console.log(bike);
        displayBikes(bike, userId);
    } else {
        console.error("Bike Not Found!");
        window.location.href = "User.html";
    }
}

function displayBikes(bike, userId) {
    const firstImage = bike.images[1].imagePath;
    const imageSrc = `data:image/jpeg;base64,${firstImage}`;
    document.getElementById("image").innerHTML =` <img src="${imageSrc}" width="100" loading=lazy >`;
    document.getElementById("type").innerHTML = `Type: ${bike.model}`;
    document.getElementById("brand").innerHTML = `Brand: ${bike.brand}`;
    document.getElementById("Reg").innerHTML = `Reg No: ${bike.registrationNumber}`;
    document.getElementById("Rent").innerHTML = `Rent: ${bike.rent}`;
    document.getElementById("From").innerHTML =` <label>From:</label><input type="date" id="date">`;
    document.getElementById("Time").innerHTML =` <label>Time:</label><input type="time" id="time">`;
    document.getElementById("To").innerHTML = `<label>Return:</label><p id="return"></p>`;
    document.getElementById("return").innerHTML = `<button onclick="requestRental(${bike.bikeId}, ${userId}, '${bike.registrationNumber}')">Request</button>`;
}

async function requestRental(bikeId, userId, regno) {
    const return_info = calculateReturnDate();
    const dateInput = document.getElementById("date").value;
    const timeInput = document.getElementById("time").value;
    const requestedDate = new Date(dateInput + 'T' + timeInput);
    const today = new Date();
    today.setHours(0, 0, 0, 0);

    if (requestedDate < today) {
        alert("The selected date cannot be in the past.");
        location.reload();
        return;

    }

    const requestBody = {
        userId: userId,
        bikeId: bikeId,
        registrationNumber: regno,
        status: "Waiting",
        rentedDate: requestedDate.toISOString(),
        returnDate: return_info.toISOString(),
    };

    console.log("Request Body:", requestBody);

    try {
        const availability = await checkAvailability(regno, requestedDate, return_info);
        console.log("Availability Response:", availability);

        if (availability === true) {
            const bookingResponse = await sendRentalRequest(requestBody);
           
            alert("Request  Sent successfully!");
        } else {
           
            alert("The bike is not available for the selected dates.");
        }
    } catch (error) {
        console.error('Error booking bike:', error);
        alert(`Booking failed: ${error.message}`);
    }
}

async function checkAvailability(regno, requestedDate, return_info) {


    const response = await fetch(`http://localhost:5156/api/Rental/CheckAvailability?registrationNumber=${regno}&reqdate=${requestedDate.toISOString()}&retdate=${return_info.toISOString()}`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
    });

    if (!response.ok) {
        const errorText = await response.text();
        throw new Error(errorText);
    }

    const availability = await response.json();
 
    return availability;
}

async function sendRentalRequest(requestBody) {
    console.log("Sending rental request...");
    const response = await fetch('http://localhost:5156/api/Rental/RentalRequest', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(requestBody)
    });

    if (!response.ok) {
        const errorText = await response.text();
        throw new Error(errorText);
    }

    return await response.json();
}

function calculateReturnDate() {
    const date = document.getElementById("date").value;
    const time = document.getElementById("time").value;
    const date_time = new Date(date + 'T' + time);
    date_time.setHours(date_time.getHours() + 24);
    document.getElementById("return").innerHTML = date_time.toUTCString();
    console.log("Calculated Return Date:", date_time.toISOString());
    return date_time;
}
