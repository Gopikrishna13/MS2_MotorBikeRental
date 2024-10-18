document.addEventListener("DOMContentLoaded", () => {
    const apiurl = 'http://localhost:5156/api/Report/FrequentRent'; // Update with your actual API endpoint

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

const userCountUrl = `http://localhost:5156/api/User/UserCount`;
let count = 0;
 fetch (userCountUrl)
.then(response => {
    if (!response.ok) {
        throw new Error('Response was not ok');
    }
    return response.json();
})
.then(data => {
    count=data;
    document.getElementById('totalUsers').textContent=count;
})


const totalBikes = `http://localhost:5156/api/Bike/BikesCount`;
let bikeCount = 0;
 fetch (totalBikes)
.then(response => {
    if (!response.ok) {
        throw new Error('Response was not ok');
    }
    return response.json();
})
.then(data => {
    bikeCount=data;
    document.getElementById('totalBikes').textContent=bikeCount;
})

const bikeRevenue = `http://localhost:5156/api/Rental/Revenue`;
let bikeRevenu = 0.00;
 fetch (bikeRevenue)
.then(response => {
    if (!response.ok) {
        throw new Error('Response was not ok');
    }
    return response.json();
})
.then(data => {
    bikeRevenu=data;
    document.getElementById('BikesInHand').textContent= `Rs. ${bikeRevenu}`;
})

const pendingBikes = `http://localhost:5156/api/Bike/PendingCount`;
let pendingBike = 0;
 fetch (pendingBikes)
.then(response => {
    if (!response.ok) {
        throw new Error('Response was not ok');
    }
    return response.json();
})
.then(data => {
    pendingBike=data;
    document.getElementById('BookedBikes').textContent=pendingBike;
})

// User Count

//http://localhost:5156/api/User/UserCount

//http://localhost:5156/api/Rental/Revenue

//http://localhost:5156/api/Bike/BikesCount

//http://localhost:5156/api/Bike/PendingCount


function populateTable(data) {
    const tableBody = document.getElementById('frequentRentTable').getElementsByTagName('tbody')[0];
    data.forEach(item => {
        const row = document.createElement('tr');

        const cellBikeId = document.createElement('td');
        cellBikeId.textContent = item.bikeId;
        row.appendChild(cellBikeId);

        const cellBrand = document.createElement('td');
        cellBrand.textContent = item.brand;
        row.appendChild(cellBrand);

        const cellModel = document.createElement('td');
        cellModel.textContent = item.model;
        row.appendChild(cellModel);

        const cellRegistrationNumber = document.createElement('td');
        cellRegistrationNumber.textContent = item.registrationNumber;
        row.appendChild(cellRegistrationNumber);

        // const cellRentCount = document.createElement('td');
        // cellRentCount.textContent = item.RentCount;
        // row.appendChild(cellRentCount);

        tableBody.appendChild(row);
    });
}