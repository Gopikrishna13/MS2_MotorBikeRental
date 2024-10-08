const managerName = sessionStorage.getItem("Manager");
const apiUrl = `http://localhost:5156/api/Admin/GetByUserName?username=${managerName}`;

fetch(apiUrl)
    .then(response => {
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        return response.json();
    })
    .then(manager => {
        if (manager) {
            displayForm(manager);
            console.log(manager);
        } else {
            console.error("Manager Not Found!");
        }
    })
    .catch(error => {
        console.error('Fetch error:', error); // Log fetch errors
    });

function displayForm(manager) {
    const formHTML = `
        <form id="update_form">
            <label>Manager First Name:</label>
            <label>${manager.firstName}</label>
            <br><br>
            <label>Manager Last Name:</label>
            <label>${manager.lastName}</label>
            <br><br>
            <label>Username:</label>
            <label>${manager.userName}</label>
            <br><br>
            <label for="email">Email:</label>
             <label>${manager.email}</label>
            <br><br>

            <label for="nic">NIC:</label>
           <label>${manager.nic}</label>
            <br><br>
            <input type="checkbox" id="option" onclick="clicked()">
            <label for="option">Change Password</label>
            <br><br>
            <div id="password"></div>
            <input type="submit" id="update_detail" value="Update">
        </form>`;

    document.getElementById("content").innerHTML = formHTML;

    // Attach the event listener here after the form is created
    document.getElementById("update_form").addEventListener('submit', function (e) {
        e.preventDefault();
        handleSubmit(manager);
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

function handleSubmit(manager) {
    const oldpassword = document.getElementById("old") ? document.getElementById("old").value : null;
    const newpassword = document.getElementById("new") ? document.getElementById("new").value : null;
    const confirmpassword = document.getElementById("confirm") ? document.getElementById("confirm").value : null;

    if (oldpassword && newpassword && confirmpassword) {
        const E_password = encryptpassword(oldpassword);
        const confirm = manager.password === E_password;

        if (confirm) {
            if (newpassword === confirmpassword) {
                const E_Newpassword = encryptpassword(newpassword);
                if (E_Newpassword.length >= 8) {
                    update_Manager(manager.firstName, manager.lastName, manager.userName, E_Newpassword, manager.email, manager.nic, manager.adminId);
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
        update_Manager(manager.firstName, manager.lastName, manager.userName, E_Newpassword, manager.email, manager.nic, manager.adminId);
    }
}

function encryptpassword(password) {
    return btoa(encodeURIComponent(password));
}

function update_Manager(firstname, lastname, username, password, email, nic, id) {
    const updatedManager = {
        FirstName: firstname,
        LastName: lastname,
        UserName: username,
        Password: password,
        NIC: nic,
        Email: email
    };

    console.log("Updating manager with ID:", id); // Debug log to check ID
    console.log(updatedManager);
    

    fetch(`http://localhost:5156/api/Admin/UpdateAdmin?Id=${id}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(updatedManager)
    })
    .then(response => {
      
        if (!response.ok) {
          
            return response.text().then(text => {
                throw new Error(`Failed to update manager: ${response.status} - ${text}`);
            });
        }
        return response.json(); 
    })
    .then(data => {
        alert("Details updated successfully");
       
    })
    .catch(error => {
    
        console.error('Error updating manager:', error.message);
    });
}    
