const userName = sessionStorage.getItem("Customer_Name");
const apiUrl = `http://localhost:5156/api/User/GetByUserName?username=${userName}`;

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



function displayForm(user) {
    const formHTML =`
     
    <div class="signup">
        <form id="update_form">
            <label>Manager First Name:</label>
            <label>${user.firstName}</label>

            <label>Manager Last Name:</label>
            <label>${user.lastName}</label>

            <label>Username:</label>
            <label>${user.userName}</label>

            <label for="email">Email:</label>
             <label>${user.email}</label>

            <label for="nic">NIC:</label>
           <label>${user.nic}</label>

            <label for="license">License:</label>
           <label>${user.licenseNumber}</label>

            

            <label class="container">Change Password
                <input type="checkbox" id="option" onclick="clicked()">
                <span class="checkmark"></span>
              </label>

            <div id="password"></div>
            <input type="submit" class="userCreateBtn" id="update_detail" value="Update">
        </form>
    </div>
    `;

    document.getElementById("content").innerHTML = formHTML;

    // Attach the event listener here after the form is created
    document.getElementById("update_form").addEventListener('submit', function (e) {
        e.preventDefault();
        handleSubmit(user);
    });
}

function clicked() {
    document.getElementById("password").innerHTML = `
        <label for="old">Old Password:</label>
        <input type="password" id="old">
        <br><br>
        <label for="new">New Password:</label>
        <input type="password" id="new">
        <br><br>
        <label for="confirm">Confirm:</label>
        <input type="password" id="confirm">
    `;
}

function handleSubmit(user) {
    const oldpassword = document.getElementById("old") ? document.getElementById("old").value : null;
    const newpassword = document.getElementById("new") ? document.getElementById("new").value : null;
    const confirmpassword = document.getElementById("confirm") ? document.getElementById("confirm").value : null;

    if (oldpassword && newpassword && confirmpassword) {
        const E_password = encryptpassword(oldpassword);
        const confirm = user.password === E_password;

        if (confirm) {
            if (newpassword === confirmpassword) {
                const E_Newpassword = encryptpassword(newpassword);
                if (E_Newpassword.length >= 8) {
                    update_User(user.firstName, user.lastName,user.userName, E_Newpassword, user.email, user.nic, user.userId,user.licenseNumber);
                } else {
                    alert("New password should be at least 8 characters long");
                }
            } else {
                alert("Passwords do not match");
            }
        } else {
            alert("Incorrect old password");
        }
    } else {
        // If password fields are not filled out, only update other details
        update_User(user.firstName, user.lastName,user.userName, E_Newpassword, user.email, user.nic, user.userId,user.licenseNumber);
    }
}

function encryptpassword(password) {
    return btoa(encodeURIComponent(password));
}

function update_User(firstname, lastname, username, password, email, nic, id,license) {
    const updatedUser = {
        FirstName: firstname,
        LastName: lastname,
        UserName: username,
        Password: password,
        NIC: nic,
        Email: email,
        LicenseNumber:license
    };

    console.log("Updating user with ID:", id); // Debug log to check ID
    console.log(updatedUser);
    

    fetch(`http://localhost:5156/api/User/UpdateUser?Id=${id}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(updatedUser)
    })
    .then(response => {
      
        if (!response.ok) {
          
            return response.text().then(text => {
                throw new Error(`Failed to update user: ${response.status} - ${text}`);
            });
        }
        return response.json(); 
    })
    .then(data => {
        alert("Details updated successfully");
       
    })
    .catch(error => {
    
        console.error('Error updating user:', error.message);
    });
}    
