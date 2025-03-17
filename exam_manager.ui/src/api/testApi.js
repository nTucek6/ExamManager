export default async function TestApi({setData}) {
    fetch('https://localhost:7299/api/Authentication/Login',   
        {method: 'POST', // Specify the HTTP method
        headers: {
            'Content-Type': 'application/json' // Tell the server you're sending JSON
        }},)
    .then(response => response.json())
    .then(json => setData(json))
    .catch(error => console.error(error));
}