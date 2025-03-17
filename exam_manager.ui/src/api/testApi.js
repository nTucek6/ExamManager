export default async function TestApi({setData}) {
    fetch('https://localhost:7299/api/Authentication/Login',   
        {method: 'POST', // Specify the HTTP method
        headers: {
            'Content-Type': 'application/json' // Tell the server you're sending JSON
        }},)
    .then(response => response.text())
    .then(text => setData(text))
    .catch(error => console.error(error));
}