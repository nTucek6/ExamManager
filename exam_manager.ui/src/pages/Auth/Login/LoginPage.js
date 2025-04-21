import { useState, useEffect } from "react";
import { Link } from "react-router";
import authenticationService from "../../../services/authenticationService";
import { useNavigate } from "react-router";
import { jwtDecode } from "jwt-decode";
import "./Login.css";

import { BarLoader } from "react-spinners";

export default function Login() {
  const navigate = useNavigate();

  const [Email, setEmail] = useState("");
  const [Password, setPassword] = useState("");

  const [formErrors, setFormErrors] = useState({});

  const [isLoginCompleted, setIsLoginCompleted] = useState(false);

  useEffect(() => {
    if (Object.keys(formErrors).length === 0 && isLoginCompleted) {
      handleLogin();
    } else {
      setIsLoginCompleted(false);
    }
  }, [formErrors]);

  const handleLogin = async () => {
    const result = await authenticationService.login(Email, Password);
    if (result.status === 200) {
      sessionStorage.setItem("token", result.data);
      sessionStorage.setItem("email", jwtDecode(result.data).Email);
      sessionStorage.setItem("userId", jwtDecode(result.data).Id);
      sessionStorage.setItem("role", jwtDecode(result.data).Role);
      
      navigate("/");
      const token = sessionStorage.getItem("token");
      if (token) {
        //console.log(jwtDecode(token));
      }
    } else if (result.status === 500) {
      setIsLoginCompleted(false);
      alert(result.Message);

    }
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    setFormErrors({});
    setIsLoginCompleted(true);

    setFormErrors(validate());
  };

  const validate = () => {
    const errors = {};
    const regex =
      /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/;

    if (Email.trim().length === 0) {
      errors.Email = "Email je potreban!";
    } else if (!regex.test(Email)) {
      errors.Email = "Email je neispravan!";
    }

    if (Password.trim().length === 0) {
      errors.Password = "Lozinka je obavezna!";
    }

    return errors;
  };

  return (
    <div className="custom-wrapper">
      <form onSubmit={handleSubmit} className="custom-form">
        <h2>Login</h2>
        <div className="form-group">
          <label>Email:</label>
          <input
            type="text"
            onChange={(d) => setEmail(d.target.value)}
            name="email"
            autoComplete="email"
            placeholder="Unesite Email:"
            className="form-input"
          />
          <span className="form-error">{formErrors.Email}</span>
        </div>
        <div className="form-group">
          <label>Lozinka:</label>
          <input
            type="password"
            onChange={(d) => setPassword(d.target.value)}
            name="password"
            autoComplete="current-password"
            placeholder="Unesite lozinku:"
            className="form-input"
          />
          <span className="form-error">{formErrors.Password}</span>
        </div>
        <div className="form-group form-link">
          <Link to="/forgot-password" id="forgot-password">
            Zaboravili ste lozinku?
          </Link>
          <Link to="/register">Nemate račun - registrirajte se ovdje</Link>
        </div>

        {isLoginCompleted ? (
          <BarLoader />
        ) : (
          <button type="submit" className="form-button">
            Prijava
          </button>
        )}
      </form>
    </div>
  );
}
