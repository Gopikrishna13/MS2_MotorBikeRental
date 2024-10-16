document.addEventListener("DOMContentLoaded", () => {
    const Id = Number(sessionStorage.getItem("BikeID"));
    const userName = sessionStorage.getItem("Customer_Name");
   // const bike_details = JSON.parse(localStorage.getItem("Bike_Details")) || [];

   const apiurl=`http://localhost:5156/api/Bike/GetById?id=${Id}`;

   fetch(apiurl)
   fetch(apiurl)
   .then(response => {
       if (!response.ok) {
           throw new Error('Response was not ok');
       }
       return response.json();
   })
   .then(bike => {
       if (bike) {
           console.log(bike);
           displayBikes(bike,userName); 
       } else {
           console.error("Bike Not Found!");
       }
   })
   .catch(error => {
       console.error('Fetch error:', error);
   });


   // const display = bike_details.find(bike => bike.ID == Id);
 
});


function displayBikes(bike,userName)
{
    if (bike) {

        const firstImage = bike.images[1].imagePath;
         
        imageSrc = `data:image/jpeg;base64,${firstImage}`;

        document.getElementById("image").innerHTML = `<img src="${imageSrc}" width="100">`;
        document.getElementById("type").innerHTML = `Type: ${bike.model}`;
        document.getElementById("brand").innerHTML = `Brand: ${bike.brand}`;
        //document.getElementById("year").innerHTML = `Year: ${display.Year}`;
        document.getElementById("Reg").innerHTML = `Reg No: ${bike.registrationNumber}`;
        document.getElementById("Rent").innerHTML = `Rent: ${bike.rent}`;
        document.getElementById("From").innerHTML = `<label>From:</label><input type="date" id="date">`;
        document.getElementById("Time").innerHTML = `<label>Time:</label><input type="time" id="time">`;
        document.getElementById("return").innerHTML = `<button onclick="Request(${bike.BikeID}, '${userName}')">Request</button>`;
        document.getElementById("To").innerHTML = `<label>Return:</label><p id="return"></p>`;
    } else {
        window.location.href = "User.html";
    }

}
function Request(id, uName) {
    const return_info = returnDate();
    const request_details = JSON.parse(localStorage.getItem("Request_Info")) || [];
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

    // Filter requests with Status == 1
    const validRequests = request_details.filter(req => req.BikeID == id && req.Status == 1);

    // Check if the requested date is valid
    const isValidRequest = validRequests.every(req => {
        const bookedDate = new Date(req.Return);
        const twoDaysBeforeBookedDate = new Date(bookedDate);
        twoDaysBeforeBookedDate.setDate(bookedDate.getDate() - 2);
        twoDaysBeforeBookedDate.setHours(0, 0, 0, 0); // Set to start of the day for accurate comparison

        console.log("Booked Date:", bookedDate);
        console.log("Two Days Before Booked Date:", twoDaysBeforeBookedDate);
        console.log("Today:", today);

        // Check if requestedDate is before the two days before booked date and not before today
        const valid = requestedDate < twoDaysBeforeBookedDate ;
        console.log("Is Requested Date Valid:", valid);
        return valid;
    });

    if (isValidRequest) {
        const newRequest = {
            RequestID: Math.floor(Math.random() * (1000000 - 1)) + 1,
            BikeID: id,
            User: uName,
            From: dateInput,
            Time: timeInput,
            Return: return_info,
            Status: 0
        };

        request_details.push(newRequest);
        localStorage.setItem("Request_Info", JSON.stringify(request_details));
    } else {
        alert("Cannot book. The requested date is too close to an already booked date or is in the past.");
    }
}


function returnDate() {
    const date = document.getElementById("date").value;
    const time = document.getElementById("time").value;
    const date_time = date + 'T' + time;
    const date_timeObj = new Date(date_time);

    // Add 24 hours to the requested date for return date
    date_timeObj.setHours(date_timeObj.getHours() + 24);

    const return_details = date_timeObj.toUTCString();

    document.getElementById("return").innerHTML = return_details;
    return return_details;
}
