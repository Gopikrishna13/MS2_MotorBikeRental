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
    container.innerHTML = ""; // Clear previous inputs

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
    const type = document.getElementById("type_search").value.toLowerCase();
    const brand = document.getElementById("brand_search").value.toLowerCase();
    const year = document.getElementById("year_search").value.toLowerCase();
    const id = Number(document.getElementById("idsearch").value);

    let bike = JSON.parse(localStorage.getItem("Bike_Details")) || [];
    bike = bike.filter(item => 
        (id === 0 || item.ID === id) &&
        (type === "" || item.Type.toLowerCase().includes(type)) &&
        (brand === "" || item.Brand.toLowerCase().includes(brand)) &&
        (year === "" || item.Year.toLowerCase().includes(year))
    );

    let table = `
        <table>  
            <tr>
                <th>ID</th>
                <th>Image</th>
                <th>Type</th>
                <th>Brand</th>
                <th>Year</th>
                <th>Registration No</th>
                <th>Rent</th>
                <th>Quantity</th>
                <th>Action</th>
            </tr>`;

    for (const data of bike) {
        table += `
            <tr>
                <td>${data.ID}</td>
                <td><img src="${data.Image}" width="50"></td>
                <td>${data.Type}</td>
                <td>${data.Brand}</td>
                <td>${data.Year}</td>
                <td>${data.Registration_Number}</td>
                <td>${data.Rent}</td>
                <td>${data.Quantity}</td>
                <td>
                    <button id="upd_btn" onclick="updateData(${data.ID})">Update</button>
                    <button id="dlt_btn" onclick="deleteData(${data.ID})">Delete</button>
                </td>
            </tr>`;
    }

    table += `</table>`;
    document.getElementById("Bike_table").innerHTML = table;
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
                console.error(`Failed to read file ${file.name}:`, error);
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
            const errorText = await response.text(); // Read the response text for error details
            throw new Error(`Failed to add bike: ${errorText}`);
        }

        alert('Bike added successfully');
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




function handleImageUpload(event) {
    const file = event.target.files;
    if (file) {
        const reader = new FileReader();
        reader.onloadend = function() {
            
        };
        reader.readAsDataURL(file);
    }
}







function deleteData(id) {
    let existingBikes = JSON.parse(localStorage.getItem("Bike_Details")) || [];
    existingBikes = existingBikes.filter(bike => bike.ID !== id);
    localStorage.setItem("Bike_Details", JSON.stringify(existingBikes));
    displayBikes();
}

function updateData(id) {
    console.log("Update button clicked for ID:", id);
    sessionStorage.setItem("ID", id);
    window.location.href = "update_bike.html";
}



















function displayBikes() {
    let bike = JSON.parse(localStorage.getItem("Bike_Details")) || [];

    let table = `
        <table>  
            <tr>
                <th>ID</th>
                <th>Image</th>
                <th>Type</th>
                <th>Brand</th>
                <th>Year</th>
                <th>Registration No</th>
                <th>Rent</th>
                
                <th>Action</th>
            </tr>`;

    for (const data of bike) {
        table += `
            <tr>
                <td>${data.ID}</td>
                <td><img src="${data.Image}" width="50"></td>
                <td>${data.Type}</td>
                <td>${data.Brand}</td>
                <td>${data.Year}</td>
                <td>${data.Registration_Number}</td>
                <td>${data.Rent}</td>
              
                <td>
                    <button id="upd_btn" onclick="updateData(${data.ID})">Update</button>
                    <button id="dlt_btn" onclick="deleteData(${data.ID})">Delete</button>
                </td>
            </tr>`;
    }

    table += `</table>`;
    document.getElementById("Bike_table").innerHTML = table;
}









