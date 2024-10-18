


document.addEventListener("DOMContentLoaded", async () => {
    const bike_price = await fetchBikeDetails(); 
    displayBikes(bike_price);
});

async function fetchBikeDetails() {
    try {
        const response = await fetch('http://localhost:5156/api/Bike/AllBikes'); 
        if (!response.ok) {
            const responseBody = await response.text();
            console.error('Response was not ok:', responseBody);
            throw new Error('Network response was not ok');
        }
        const data = await response.json(); 
        return data; 
    } catch (error) {
        console.error('Fetch error:', error);
    }
}

function searchBike() {
    const model = document.getElementById("search_model").value;
    const brand = document.getElementById("search_brand").value;
    const rent = Number(document.getElementById("Rent").value);
    let params = new URLSearchParams();

    params.append('Rent', isNaN(rent) ? 0 : rent);
    params.append('Brand', brand ? encodeURIComponent(brand) : '%22%22');
    params.append('Model', model ? encodeURIComponent(model) : '%22%22');

    const searchUrl = `http://localhost:5156/api/Bike/SearchBikes?${params.toString()}`;

    fetch(searchUrl)
        .then(response => {
            if (!response.ok) {
                return response.text();
            }
            return response.json();
        })
        .then(bike => {
            const bikes = Array.isArray(bike) ? bike : [bike];
            if (bikes.length > 0) {
                displayBikes(bikes);
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
        let imageSrc = 'default_image_path.jpg'; 
        if (bike.images && bike.images.length > 0) {
            const firstImage = bike.images[1].imagePath; 
         
            if (firstImage) {
                if (firstImage.startsWith('data:image/jpeg;base64,') || firstImage.startsWith('data:image/png;base64,')) {
                    imageSrc = firstImage; 
                } else {
                    imageSrc = `data:image/jpeg;base64,${firstImage}`; 
                }
            } else {
                console.error('Image data is missing or invalid for bike:', bike.bikeId);
            }
        }
        content += `
            <div class="bikes">
                <div class="gallery">
                    <img src="${imageSrc}" alt="${escapeHtml(bike.model)} Image" loading="lazy">
                </div>
                <div class="details">
                    <h1>${escapeHtml(bike.model)}</h1>
                    <h2>${escapeHtml(bike.brand)}</h2>
                    <p style="color: black;">Bike Reg No: <span>${escapeHtml(bike.registrationNumber)}</span></p>
                    <p style="color: black;">Bike Rent Price: <span>${escapeHtml(bike.rent)}</span></p>
                    <p style="text-align: center; color: gray;">Renting a motorbike offers several advantages for travelers and locals alike...</p>
                    <div class="hourRent">
                        <button class="rentBtn" onclick="viewBike(${bike.bikeId})">View</button>
                    </div>
                </div>
            </div>
        `;
    });
    document.getElementById('bike_details').innerHTML = content; // Ensure you have this element
}


function escapeHtml(html) {
    const text = document.createElement('textarea');
    text.textContent = html;
    return text.innerHTML;
}

function viewBike(id) {
    sessionStorage.setItem("BikeID", id); 
    window.location.href = "Userview.html"; 
}

