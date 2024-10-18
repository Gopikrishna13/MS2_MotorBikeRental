document.getElementById("select_Role").addEventListener("change", () => {
    let role = document.getElementById("select_Role").value;
    if (role == "Manager") {
        document.getElementById("User_details").style.display = "none";
        document.getElementById("Manager_details").style.display = "block";
    } else if (role == "User") {
        document.getElementById("Manager_details").style.display = "none";
        document.getElementById("User_details").style.display = "block";
    }
});

document.getElementById("user_creation").addEventListener("submit", validate);

async function validate(e) {
    e.preventDefault();

    let role = document.getElementById("select_Role").value;
    if (role == "Manager") {
        let Manager_Name=document.getElementById("M_Name").value.trim();
        let Manager_lName=document.getElementById("M_lName").value.trim();

        let Manager_username = document.getElementById("M_username").value.trim();
        let Manager_passwd = document.getElementById("M_password").value.trim();
        let Manager_nic = document.getElementById("M_NIC").value.trim();
        //let Manager_mobile = document.getElementById("M_Mobile").value.trim();
        let Manager_email = document.getElementById("M_Email").value.trim();

        // if (checkManager(Manager_username, Manager_nic)) {
        //     alert("Username or NIC already exists! Please choose another.");
        //     location.reload();
        //     return;
        // }

        if (Manager_username.startsWith("MT")) {
            if (Manager_passwd.length >= 8) {
                await Store_Manager(Manager_Name,Manager_lName,Manager_username, Manager_passwd, Manager_nic,  Manager_email);
                alert("Manager account created successfully!");
                window.location.href = "Login.html";
            } else {
                document.getElementById("manager_password").textContent = "Invalid Password Length (should be at least 8 characters)";
            }
        } else {
            document.getElementById("manager_username").textContent = "Invalid User Name (should start with 'M')";
        }
    } else if (role == "User") {
        let User_FName=document.getElementById("U_Name").value.trim();
        let User_LName=document.getElementById("U_lName").value.trim();
        let User_Name = document.getElementById("U_username").value.trim();
        let User_Pwd = document.getElementById("U_password").value.trim();
        let NIC = document.getElementById("U_NIC").value.trim();
        let License = document.getElementById("License").value.trim();
        //let User_mobile = document.getElementById("U_Mobile").value.trim();
        let User_email = document.getElementById("U_Email").value.trim();

        // if (checkUser(User_Name, NIC)) {
        //     alert("Username or NIC already exists! Please choose another.");
        //     location.reload();
        //     return;
        // }

        if (User_Name.startsWith("UT")) {
            if (User_Pwd.length >= 8) {
                await Store_User(User_FName,User_LName,User_Name, User_Pwd, NIC, License,  User_email);
                alert("User account created successfully!");
                window.location.href = "Login.html";
            } else {
                document.getElementById("user_password").textContent = "Invalid Password Length (should be at least 8 characters)";
            }
        } else {
            document.getElementById("user_username").textContent = "Invalid User Name (should start with 'U')";
        }
    }
}


async function Store_Manager(Manager_Name,Manager_lName,Manager_username, Manager_passwd, Manager_nic, Manager_Email) {
    const M_password = encrypt_password(Manager_passwd); // Encrypt password
    const Manager = {
        FirstName:Manager_Name,
        LastName:Manager_lName,
        UserName: Manager_username,
        Password: M_password,
        NIC: Manager_nic,
        //Mobile: Manager_mobile,
        Email: Manager_Email
    };

    try {
        const response = await fetch('http://localhost:5156/api/Admin/AddAdmin', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json', 
            },
            body: JSON.stringify(Manager), 
        });

        if (!response.ok) {
            alert('Error in creating Manager! Status: ' + response.status);
            return;
        }

        alert('Manager account created successfully!');
        window.location.href = "Login.html";

    } catch (error) {
        console.error('Error:', error);
        
    }
}
// Encrypt Password
function encrypt_password(password) {
    return window.btoa(password); // Base64 encoding
}

async function Store_User(User_FName,User_LName,User_Name, User_Pwd, NIC, License,  User_email)
{
    const U_password = encrypt_password(User_Pwd); // Encrypt password
    const User = {
        FirstName:User_FName,
        LastName:User_LName,
        UserName: User_Name,
        Password: U_password,
        NIC: NIC,
        Email: User_email,
        LicenseNumber:License,
        //Mobile: Manager_mobile,
       
    };

    try {
        const response = await fetch('http://localhost:5156/api/User/AddUser', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json', 
            },
            body: JSON.stringify(User), 
        });

        if (!response.ok) {
            alert('Error in creating User! Status: ' + response.status);
            return;
        }

        alert('User account created successfully!');
        window.location.href = "Login.html";

    } catch (error) {
        console.error('Error:', error);
        
    }

}