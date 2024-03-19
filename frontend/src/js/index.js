const host = "/api";

window.addEventListener("load", async (event) => {
    let repsonse = await fetch(host+"/weatherforecast",{
        method: "GET",
    });
    let data = await repsonse.json();
    console.log(data);

    for (const forecast in data) {
        let div = document.createElement("div");
        div.innerHTML = `<strong>${data[forecast].date}</strong> - ${data[forecast].temperatureC}°C`;
        document.body.appendChild(div);
    }
});