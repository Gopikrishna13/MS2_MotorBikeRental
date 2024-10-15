document.addEventListener("DOMContentLoaded", async () => {
    const bike_price = await fetchBikeDetails(); 
    displayBikes(bike_price);
});

async function fetchBikeDetails() {
    try {
        const response = await fetch('http://localhost:5156/api/Bike/AllBikes'); 
        console.log(response); 
        if (!response.ok) {
        
            const responseBody = await response.text();
            console.error('response was not ok');
          ;
        }
        const data = await response.json(); 
        console.log(data);
        
        return data; 
    } catch (error) {
        console.error( error);
        return []; 
    }
}


function displayBikes(bikes) {
    let content = ''; 

    bikes.forEach(bike => {
        
        let imageSrc ;
        if (bike.images.length > 0) {
            const firstImage = bike.images[1].imagePath;
         
            imageSrc = `data:image/jpeg;base64,${firstImage}`;
            
        }

        content += `
        <div class="bike_card">
            <img src="${imageSrc}" alt="${bike.model} Image" style="width:100px;height:100px;"><br>
            <strong>Model: ${bike.model}</strong><br>
            <em>Brand: ${bike.brand}</em><br>
     
            Reg. No: ${bike.registrationNumber}<br>
            Rent: ${bike.rent}<br>
            <button onclick="viewBike(${bike.bikeId})">View</button>
        </div>`;
    });
  
    document.getElementById("bike_details").innerHTML = content;
}



function viewBike(id) {
    sessionStorage.setItem("BikeID", id); 
    window.location.href = "Userview.html"; 
}