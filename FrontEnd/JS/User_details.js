


const apiUrl = `http://localhost:5156/api/User/AllUsers`;

fetch(apiUrl)
    .then(response => {
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        return response.json();
    })
    .then(user => {
        if (user) {
            displayForm(user);
            console.log(user);
        } else {
            console.error("user Not Found!");
        }
    })
    .catch(error => {
        console.error('Fetch error:', error); // Log fetch errors
    });






function displayForm(users) {
    let table = `
     
        <tr>
            <th>ID</th>
            <th>First Name</th>
            <th>Last Name</th>
            <th>User Name</th>
            <th>NIC</th>
            <th>License Number</th>
            <th>Email</th>
        </tr>
    
                `;

    for (const user of users) {
        table += `<tr>       
                     <td>${user.userId}</td>
                     <td>${user.firstName}</td>
                     <td>${user.lastName}</td>
                     <td>${user.userName}</td>
                     <td>${user.nic}</td>
                     <td>${user.licenseNumber}</td>
                     <td>${user.email} </td>
                </tr>`;
    }


    document.getElementById("user_table").innerHTML = table;
}


document.getElementById("search_btn").addEventListener('click', () => {
    const u_Name = document.getElementById("UName_search").value;

    const GetapiUrl = `http://localhost:5156/api/User/GetByUserName?username=${u_Name}`;

    fetch(GetapiUrl)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(user => {
            let users = Array.isArray(user) ? user : [user];
            if (user) {

                displayForm(users);
                console.log(user);
            } else {
                console.error("user Not Found!");
            }
        })
        .catch(error => {
            console.error('Fetch error:', error); // Log fetch errors
        });

});



