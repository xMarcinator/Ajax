const host = "/api";
const temp = document.getElementById("temp");
window.addEventListener("load", async (event) => {
    let repsonse = await fetch(API_PREFIX+"/weatherforecast",{
        method: "GET",
    });
    let data = await repsonse.json();
    console.log(data);

    // Clear the list
    temp.replaceChildren()
    
    for (const forecast in data) {
        let div = document.createElement("div");
        div.innerHTML = `<strong>${data[forecast].date}</strong> - ${data[forecast].temperatureC}°C`;
        temp.appendChild(div);
    }
});


const list = document.getElementById("List");

window.addEventListener("load", async (event) => {
    let repsonse = await fetch(API_PREFIX+"/person/all",{
        method: "GET",
    });
    
    let data = await repsonse.json();
    console.log(data);

    // Clear the list
    list.replaceChildren()
    
    if (data.length === 0) {
        let div = document.createElement("div");
        div.innerHTML = `<strong>No data found</strong>`;
        document.body.appendChild(div);
        return;
    }
    
    for (const person of data) {
        instertHTMLPerson(person);
    }
});

document.getElementById("AddForm").addEventListener("submit", addPerson);

async function addPerson(event) {
    event.preventDefault();

    var formData = new FormData(event.target);
    var data = {
        name: formData.get("name"),
        phone: formData.get("phone"),
    }

    // output as an object
    console.log(Object.fromEntries(formData));

    fetch(API_PREFIX+"/person", {
        method: "POST",
        headers: {
            "Content-Type":  'application/json; charset=utf-8',
        },
        body: JSON.stringify(data),
    })
    .then(async (res) => {
        
        console.log(res);
        
        data.personId = await res.json();
        console.log(data);
        
        instertHTMLPerson(data);
    });
}

document.getElementById("List").addEventListener("click", handleDeletePerson);

async function handleDeletePerson(event) {
    console.log("click", event.target,event.currentTarget);
    
    if (event.target.tagName !== "BUTTON") return;
    
    let personId = event.target.parentElement.id.split("-")[1];
    
    console.log(personId);

    deletePerson(personId);
}
    

function instertHTMLPerson(person) {
    let div = document.createElement("div");
    div.innerHTML = `<strong>${person.name}</strong> - ${person.phone}<button>Delete</button>`;
    div.setAttribute("id", `person-${person.personId}`);
    list.appendChild(div);
}

function deletePerson(id) {
    fetch(API_PREFIX+"/person/"+id, {
        method: "DELETE",
    })
    .then((res) => {
        console.log(res);
        document.getElementById(`person-${id}`).remove();
    });
}