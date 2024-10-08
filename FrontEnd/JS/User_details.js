


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



//const user_details = JSON.parse(localStorage.getItem("User_Details")) || [];
document.addEventListener("DOMContentLoaded",displayUsers(user_details));

function displayForm(users)
{
    let table = `<tr>
            <th>ID</th>
            <th>First Name</th>
            <th>Last Name</th>
            <th>User Name</th>
            <th>NIC</th>
            <th>License Number</th>
           
            <th>Email</th>
        </tr>`;

     for(const user of users)
     {
        table+=`
        <tr>
        
         <td>
         ${user.userId}
         </td>
         <td>
                ${user.firstName}
                </td>

                 <td>
                ${user.lastName}
                </td>

            
                <td>
                ${user.userName}</td>
                <td>
                ${user.nic}</td>
                <td>
                  ${user.licenseNumber}  </td>
                 
                   <td>
                   ${user.email} </td>
                </tr>`;
     }

        document.getElementById("user_table").innerHTML = table;
}
      

// document.getElementById("search_btn").addEventListener('click', () => {
//             const u_Id = Number(document.getElementById("ID_search").value);
//             const u_Name = document.getElementById("UName_search").value.toLowerCase();

          
//             const find_user = user_details.filter(user => 
//         ( u_Id === 0 || user.ID === u_Id) &&
//         (u_Name === "" || user.UserName.toLowerCase().includes(u_Name))
//     );
            
//                 displayUsers(find_user);
           
//         });