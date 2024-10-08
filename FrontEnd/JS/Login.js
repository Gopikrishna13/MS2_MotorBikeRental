document.getElementById("user_login").addEventListener("submit", login_validate);

async function login_validate(event) {
    event.preventDefault();
    let username = document.getElementById("Username").value;
    let password = document.getElementById("password").value;

    if (username.startsWith("MT")) {
        const m_login = await manager_Login(username, password);
        if (m_login) {
           
            window.location.href = "Manager_Dashboard.html"; 
            sessionStorage.setItem("Manager",username);
        } else {
            document.getElementById("response").innerHTML = "Login Failed";
        }
    } else if (username.startsWith("UT")) {
        const u_login = user_Login(username, password);
        if (u_login) {
            sessionStorage.setItem("Customer_Name", username);
            window.location.href = "User.html"; 
        } else {
            document.getElementById("response").innerHTML = "Login Failed";
        }
    } else {
        document.getElementById("response").innerHTML = "Invalid Login";
    }
}


async function manager_Login(username, password) {
    const E_mpasswd = encrypt_password(password); 

    const loginData = {
        UserName: username,
        Password: E_mpasswd
    };

   // console.log("Logging in with:", loginData); a

    try {
        const response = await fetch('http://localhost:5156/api/Admin/Login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(loginData),
        });

        if (response.ok) {
            const data = await response.json();
            //console.log("Server response:", data); 
            
           
            if (typeof data === "boolean" && data) {
                return true; 
            } else {
                return false; 
            }
        } else {
            console.error("Response error:", response.status);
            return false; 
        }
    } catch (error) {
        console.error("Fetch error:", error);
        return false;
    }
}


// function user_Login(username, password) {
//     //check user name exist
//     const user_details = JSON.parse(localStorage.getItem("User_Details"));
//     if (!user_details) {
//         console.error("No user details found in localStorage");
//         return false;
//     }

//     const E_Upasswd = encrypt_password(password);
// //if exist check password
//     for (const user of user_details) {
//         if (user.UserName === username && user.Password === E_Upasswd) {
//             return true;
//         }
//     }
//     return false;
// }

function encrypt_password(password) {
    return btoa(encodeURIComponent(password));
}

