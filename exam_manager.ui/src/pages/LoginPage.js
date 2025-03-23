import { useState } from "react";
import authenticationService from "../services/authenticationService";
import { useNavigate } from "react-router";
import "./Login.css";
export default function Login() {
  const navigate = useNavigate();

  const [Email, setEmail] = useState();
  const [Password, setPassword] = useState();

  const handleLogin = async (e) => {
    e.preventDefault();
    const result = await authenticationService.login(Email, Password);
    sessionStorage.setItem("token", "4346ghfhzu76");
    sessionStorage.setItem("email", Email);
    console.log(result);
    navigate(0);
  };

  return (
    <div className="login-wrapper">
      <form onSubmit={handleLogin} className="login-form">
        <h2>Login</h2>
        <div className="form-group">
          <label>Email:</label>
          <input
            type="text"
            onChange={(d) => setEmail(d.target.value)}
            placeholder="Enter your email"
            className="form-input"
          />
        </div>
        <div className="form-group">
          <label>Password:</label>
          <input
            type="password"
            onChange={(d) => setPassword(d.target.value)}
            placeholder="Enter your password"
            className="form-input"
          />
        </div>
        <button type="submit" className="login-button">
          Login
        </button>
      </form>
    </div>
  );
}
