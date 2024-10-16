document.addEventListener("DOMContentLoaded", async () => {
    // document.getElementById("Range").innerHTML=`<input type="range" min=${50} max=${100000} id="range"`;
    // document.getElementById("Rent").value=document.getElementById("range").value;
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
      
    }
}

function searchBike()
{
   const model=document.getElementById("search_model").value;
   const brand=document.getElementById("search_brand").value;
   const rent=Number(document.getElementById("Rent").value);
   let params = new URLSearchParams();
    
   if (!rent) {
       params.append('Rent', 0);
   } else {
       params.append('Rent', Number(rent));
   }

   if (!brand) {
       params.append('Brand', "%22%22");
   } else {
       params.append('Brand', encodeURIComponent(brand));
   }

   if (!model) {
       params.append('Model', "%22%22");
   } else {
       params.append('Model', encodeURIComponent(model));
   }

   const searchUrl = `http://localhost:5156/api/Bike/SearchBikes?${params.toString()}`;

   fetch(searchUrl)
       .then(response => {
           if (!response.ok) {
               return response.text();
           }
           return response.json();
       })
       .then(bike => {
           let bikes = Array.isArray(bike) ? bike : [bike];
           if (bikes.length > 0) {
               displayBikes(bikes);
               console.log(bikes);
           } else {
               console.error("Bike Not Found!");
           }
       })
       .catch(error => {
           console.error('Fetch error:', error);
       });

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