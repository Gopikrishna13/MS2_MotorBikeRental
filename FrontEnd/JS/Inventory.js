document.addEventListener("DOMContentLoaded", function() {
    displayBikes();

    document.getElementById('add_btn').addEventListener('click', function() {
        var form = document.getElementById('bike_form');
        form.style.display = form.style.display === 'none' ? 'block' : 'none';
    });

    document.getElementById("bike_quantity").addEventListener("input", generateRegNumberFields);

    document.getElementById("search_btn").addEventListener("click", searchBike);

    document.getElementById("bike_form").addEventListener("submit", function(e) {
        e.preventDefault();
        createBike();
    });

    document.getElementById("bike_detail_cancel").addEventListener("click", function() {
        document.getElementById("bike_form").style.display = "none";
    });

    //document.getElementById("image_input").addEventListener("change", handleImageUpload);
});

function generateRegNumberFields() {
    const quantity = Number(document.getElementById("bike_quantity").value);
    const container = document.getElementById("reg_numbers_container");
    container.innerHTML = "";

    for (let i = 0; i < quantity; i++) {
        const label = document.createElement("label");
        label.textContent = `Registration Number ${i + 1}`;
        container.appendChild(label);

        const input = document.createElement("input");
        input.type = "text";
        input.required = true;
        input.className = "bike_reg";
        container.appendChild(input);

        const input_year = document.createElement("input");
        input_year.placeholder = "Year";
        input_year.type = "text";
        input_year.required = true;
        input_year.className = "bike_year";
        container.appendChild(input_year);

        const input_img = document.createElement("input");
        input_img.type = "file";
        input_img.required = true; 
        input_img.multiple=true;
        input_img.className = "bike_img";
        container.appendChild(input_img);

        container.appendChild(document.createElement("br"));
        container.appendChild(document.createElement("br"));
    }
}

function searchBike() {
    let model = document.getElementById("type_search").value.trim();
    let brand = document.getElementById("brand_search").value.trim();
    let rent = document.getElementById("idsearch").value;

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




async function createBike() {
    const bike_model = document.getElementById("bike_model").value;
    const bike_brand = document.getElementById("bike_brand").value;
    const bike_year_elems = document.querySelectorAll(".bike_year");
    const bike_price = parseFloat(document.getElementById("bike_price").value);
    const bike_reg_elems = document.querySelectorAll(".bike_reg");
    const imageElems = document.querySelectorAll(".bike_img");

    const bikeUnits = [];

    for (let i = 0; i < bike_reg_elems.length; ++i) {
        const bike_reg = bike_reg_elems[i].value;
        const bike_year = parseInt(bike_year_elems[i].value);
        const bike_images = imageElems[i].files;

        const images = [];


        for (let j = 0; j < bike_images.length; ++j) {
            const file = bike_images[j];

            try {
                const imageData = await getBase64(file); 

                images.push({
                    ImagePath: imageData 
                });
            } catch (error) {
                console.error( error);
            }
        }

        const newBikeUnit = {
            RegistrationNumber: bike_reg,
            Year: bike_year,
            Status: 0,
            Images: images 
        };

        bikeUnits.push(newBikeUnit);
    }

    const newBike = {
        Model: bike_model,
        Brand: bike_brand,
        Rent: bike_price,
        Units: bikeUnits 
    };

    try {
        const response = await fetch('http://localhost:5156/api/Bike/AddBike', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(newBike)
        });

        if (!response.ok) {
            const errorText = await response.text(); 
            throw new Error(errorText);
        }

        alert('Bike added successfully');
        location.reload();
        document.getElementById("bike_form").reset();

    } catch (error) {
        alert(`Error: ${error.message}`);
    }
}

function getBase64(file) {
    return new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = () => resolve(reader.result);
        reader.onerror = error => reject(error);
    });
}


function deleteData(id) {
    // let existingBikes = JSON.parse(localStorage.getItem("Bike_Details")) || [];
    // existingBikes = existingBikes.filter(bike => bike.ID !== id);
    // localStorage.setItem("Bike_Details", JSON.stringify(existingBikes));
    const apiDeleteUrl = `http://localhost:5156/api/Bike/DeleteBike?Id=${parseInt(id)}`;

fetch(apiDeleteUrl,{
    method:'DELETE',
    headers:{'Content-Type':'application/json'}
}).then (response=>{
    if(!response.ok)
    {
        throw new Error('response not ok');
    }
    return response.text();
}).then(data=>{
    alert("Bike deleted successfully!");
    console.log(data);
    location.reload();
    //displayBikes();
}).catch(error=>{
    console.error(error);
})

    
}

function updateData(id) {
    console.log("Update button clicked for ID:", id);
    sessionStorage.setItem("ID", id);
    window.location.href = "update_bike.html";
}
const apiUrl = `http://localhost:5156/api/Bike/AllBikes`;

fetch(apiUrl)
    .then(response => {
        if (!response.ok) {
            throw new Error('Response was not ok');
        }
        return response.json();
    })
    .then(bike => {
      
        let bikes = Array.isArray(bike) ? bike : [bike]; 
        if (bikes.length > 0) {
            displayBikes(bikes);
            //console.log(bikes);
        } else {
            console.error("Bike Not Found!");
        }
    })
    .catch(error => {
        console.error('Fetch error:', error); 
    });

    function displayBikes(bikes) {
       
        // let table = `
        //     <table>
        //         <tr>
        //             <th>ID</th>
        //             <th>Image</th>
        //             <th>Brand</th>
        //             <th>Model</th>
               
        //             <th>Registration No</th>
        //             <th>Rent</th>
        //             <th>Action</th>
        //         </tr>`;

                let table = `
                 <tr>
                     <th>ID</th>
                    <th>Image</th>
                    <th>Brand</th>
                    <th>Model</th>
                    <th>Registration No</th>
                    <th>Rent</th>
                    <th>Action</th>
                </tr>
                
               `;
    
                if (Array.isArray(bikes) && bikes.length > 0) {
            for (const data of bikes) {
              
                const imagePaths = data.images.map(img => img.imagePath); 
    
                // table += `
                //     <tr>
                //         <td>${data.bikeId}</td>
                //         <td>${imagePaths.length > 0 ? `<img src="data:image/jpg;base64,${imagePaths[1]}" width="50">` : 'No Image'}</td>
                //         <td>${data.brand}</td>
                //         <td>${data.model}</td>
                       
                //         <td>${data.registrationNumber}</td>
                //         <td>${data.rent}</td>
                //         <td> 
                //             <button id="upd_btn" onclick="updateData(${data.bikeId})">Update</button>
                //             <button id="dlt_btn" onclick="deleteData(${data.bikeId})">Delete</button>
                //         </td>
                //     </tr>`;

                table += `
                <tr>
                    <td>${data.bikeId}</td>
                    <td>${imagePaths.length > 0 ? `<img src="data:image/jpg;base64,${imagePaths[1]}" width="50">` : 'No Image'}</td>
                    <td>${data.brand}</td>
                    <td>${data.model}</td>
                   
                    <td>${data.registrationNumber}</td>
                    <td>${data.rent}</td>
                    <td> 
                        <button id="upd_btn" onclick="updateData(${data.bikeId})">Update</button>
                        <button id="dlt_btn" onclick="deleteData(${data.bikeId})">Delete</button>
                    </td>
                </tr>`;
            }
        }
    
        table += `</table>`;
        document.getElementById("Bike_table").innerHTML = table;
    }
 






