import { useState } from "react";
import authenticationService from "../services/authenticationService";
import { useNavigate } from "react-router";

export default function Login() {

  const navigate = useNavigate();

  const [Email, setEmail] = useState();
  const [Password, setPassword] = useState();

  const handleLogin = async e => {
    e.preventDefault();
    const result = await authenticationService.login(Email, Password);
    sessionStorage.setItem('token', "4346ghfhzu76");
    console.log(result);
    navigate(0);
  };

  return (
    <form onSubmit={handleLogin}>
      <div>
        <label>Email:</label>
        <input type="text" onChange={(d) => setEmail(d.target.value)} />
      </div>
      <div>
        <label>Password:</label>
        <input type="password" onChange={(d) => setPassword(d.target.value)} />
      </div>
      <button type="submit">Login</button>
    </form>
  );
}
