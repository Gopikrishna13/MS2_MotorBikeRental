document.addEventListener("DOMContentLoaded", () => {
    const bike_price = JSON.parse(localStorage.getItem("Bike_Details")) || [];
    const bike_rent = bike_price.map(bike => bike.Rent);
    let min = Math.min(...bike_rent);
    let max = Math.max(...bike_rent);
    const request = JSON.parse(localStorage.getItem("Request_Info")) || [];

    document.getElementById("Range").innerHTML = `<input type="range" min="${min}" max="${max}" id="range">`;
    document.getElementById("Rent").value = document.getElementById("range").value;

    document.addEventListener("input", () => {
        document.getElementById("Rent").value = document.getElementById("range").value;
    });

    displayBikes(bike_price, request);
});

function displayBikes(bikes, request) {
    let content = '';

    const availablebike = bikes.filter(bike => bike.Status != 1);
    const request_details = request.filter(req => req.Status == 1);

    availablebike.forEach(bike => {
        content += `
        <div class="bike_card">
            <img src="${bike.Image}" ><br>
            <strong>${bike.Type}</strong>
            <em>${bike.Brand}</em>
            Year: ${bike.Year}<br>
            Reg. No: ${bike.Registration_Number}<br>
            Rent: ${bike.Rent}<br>
            <button onclick="viewBike(${bike.ID})">View</button>
        </div>`;
    });

    const joined_Detail = bikes.map(bike => {
        const matching_requests = request_details.filter(req => req.BikeID == bike.ID);
        
        const minFromDate = matching_requests.reduce((minDate, req) => {
            const fromDate = new Date(req.From);
            const twoDaysBeforeFromDate = new Date(fromDate);
            twoDaysBeforeFromDate.setDate(fromDate.getDate() - 2);

            const today = new Date();
            today.setHours(0, 0, 0, 0);

            const adjustedMinDate = minDate === null || twoDaysBeforeFromDate < minDate ? twoDaysBeforeFromDate : minDate;

            return adjustedMinDate < today ? today : adjustedMinDate;
        }, null);

        const latestReturnDate = matching_requests.reduce((latestDate, req) => {
            const returnDate = new Date(req.Return);
            return latestDate === null || returnDate > latestDate ? returnDate : latestDate;
        }, null);

        let availableAfterDate = null;
        if (latestReturnDate) {
            availableAfterDate = new Date(latestReturnDate);
            availableAfterDate.setDate(latestReturnDate.getDate() + 3);
            availableAfterDate = availableAfterDate.toISOString().split('T')[0];
        }

        return {
            ...bike,
            minFromDate: minFromDate ? minFromDate.toISOString().split('T')[0] : null,
            availableAfterDate: availableAfterDate
        };
    });

    joined_Detail.forEach(bike => {
        if (bike.minFromDate) {
            content += `
            <div class="bike_card">
                <img src="${bike.Image}" ><br>
                <strong>${bike.Type}</strong>
                <em>${bike.Brand}</em>
                Year: ${bike.Year}<br>
                Reg. No: ${bike.Registration_Number}<br>
                Rent: ${bike.Rent}<br>
                <p style="color: red;">Available Until ${bike.minFromDate}</p>
                ${bike.availableAfterDate ? `<p style="color: green;">Available After ${bike.availableAfterDate}</p>` : ''}
                <button onclick="viewBike(${bike.ID})">View</button>
            </div>`;
        }
    });

    document.getElementById("bike_details").innerHTML = content;
}


function searchBike() {
    const bike_type = document.getElementById("search_type").value.toLowerCase();
    const bike_brand = document.getElementById("search_brand").value.toLowerCase();
    const bike_rent = Number(document.getElementById("Rent").value);

    const search_bikes = JSON.parse(localStorage.getItem("Bike_Details")) || [];

    const find_bikes = search_bikes.filter(bike => 
        (bike_type === "" || bike.Type.toLowerCase().includes(bike_type)) &&
        (bike_brand === "" || bike.Brand.toLowerCase().includes(bike_brand)) &&
        (bike_rent === 0 || bike.Rent <= bike_rent)
    );

    displayBikes(find_bikes, JSON.parse(localStorage.getItem("Request_Info")) || []);
}

function viewBike(id) {
    sessionStorage.setItem("BikeID", id);
    window.location.href = "Userview.html";
}
