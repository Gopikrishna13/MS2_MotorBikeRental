document.addEventListener("DOMContentLoaded", () => {
    const id = Number(sessionStorage.getItem("ID"));

    const apiurl = `http://localhost:5156/api/Bike/GetById?id=${parseInt(id)}`;
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
                displayBikes(bike); 
            } else {
                console.error("Bike Not Found!");
            }
        })
        .catch(error => {
            console.error('Fetch error:', error);
        });
});



function displayBikes(bike) {
    let firstImagePath = bike.images && bike.images.length > 0 ? bike.images[1].imagePath : ''; 

    let formHTML = `
    <div class="signup">
        <form id="update_form" class="updateBike">
            <label for="bike_model_update">Model</label>
            <input type="text" id="bike_model_update" value="${bike.model}" required><br><br>
    
            <label for="bike_brand_update">Brand</label>
            <input type="text" id="bike_brand_update" value="${bike.brand}" required><br><br>
    
            <label for="bike_rent_update">Rent</label>
            <input type="number" id="bike_rent_update" value="${bike.rent}" required><br><br>
    
            <label for="bike_reg_update">Registration Number</label>
            <input type="text" id="bike_reg_update" value="${bike.registrationNumber}" required><br><br>
           
            <label for="bike_img_update">Upload New Image</label>
            <input type="file" id="bike_img_update"><br><br>
    
            <input type="submit" id="update_detail" class="userCreateBtn" value="Update">
            <input type="button" id="update_detail_cancel" class="userCreateBtn" value="Cancel">
        </form>

    </div>
    `;

    document.getElementById("update_bike").innerHTML = formHTML;


    document.getElementById("update_form").addEventListener("submit", function(e) {
        e.preventDefault();
    
        const bikeModel = document.getElementById("bike_model_update").value;
        const bikeBrand = document.getElementById("bike_brand_update").value;
        const bikeRent = document.getElementById("bike_rent_update").value;
        const bikeReg = document.getElementById("bike_reg_update").value;
       // const bikeYear = document.getElementById("bike_year_update").value || 0; // Provide default if year is not provided
       // const bikeStatus = document.getElementById("bike_status_update").value || 0; // Provide default if status is not provided
        const bikeImg = document.getElementById("bike_img_update").files[0];
        const bikeId = parseInt(sessionStorage.getItem("ID"));
        
        if (!bikeId) {
            console.error('Bike ID is missing');
            return;
        }
    
        const handleImageUpload = (newImagePath) => {
            const updatedBikeDetails = {
                bikeId: bikeId,
                model: bikeModel,
                brand: bikeBrand,
                rent: parseInt(bikeRent),
                units: [
                    {
                        registrationNumber: bikeReg,
                        year: 2020,
                        images: [
                            {
                                imagePath: newImagePath 
                            }
                        ],
                        status: 0
                    }
                ]
            };
    
            fetch(`http://localhost:5156/api/Bike/UpdateBike?BikeId=${bikeId}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(updatedBikeDetails)
            })
            .then(response => {
                if (!response.ok) {
                    return response.text().then(text => {
                        console.error(`Error ${response.status}: ${response.statusText}`);
                        console.error('Response body:', text);
                        throw new Error('Error updating bike');
                    });
                }
                return response.json();
            })
            .then(() => {
                alert("Bike details updated successfully!");
                window.location.href = "inventory.html";
            })
            .catch(error => {
                console.error('Update failed:', error);
            });
        };
    
        if (bikeImg) {
            const reader = new FileReader();
            reader.onload = function(event) {
                handleImageUpload(event.target.result);
            };
            reader.readAsDataURL(bikeImg);
        } else {
            handleImageUpload();
        }
    });
    
    
   
    document.getElementById("update_detail_cancel").addEventListener("click", () => {
        window.location.href = "inventory.html"; 
    });
}
