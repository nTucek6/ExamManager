const URL = process.env.REACT_APP_API_URL;

export default async function TestApi({ setData }) {
  fetch(`${URL}/Authentication/Login`, {
    method: "POST", // Specify the HTTP method
    headers: {
      "Content-Type": "application/json", // Tell the server you're sending JSON
    },
  })
    .then((response) => response.json())
    .then((json) => setData(json))
    .catch((error) => console.error(error));
}
