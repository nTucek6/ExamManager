import "./Register.css";
import { Link } from "react-router";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router";
import { jwtDecode } from "jwt-decode";

import authenticationService from "../../services/authenticationService";
import { BarLoader } from "react-spinners";

export default function Register() {
  const navigate = useNavigate();
  const [Email, setEmail] = useState("");
  const [Password, setPassword] = useState("");
  const [ConfirmPassword, setConfirmPassword] = useState("");
  const [Name, setName] = useState("");
  const [Surname, setSurname] = useState("");

  const [isRegisterCompleted, setIsRegisterCompleted] = useState(false);
  const [formErrors, setFormErrors] = useState({});

  useEffect(() => {
    if (Object.keys(formErrors).length === 0 && isRegisterCompleted) {
      handleRegister();
    } else {
      setIsRegisterCompleted(false);
    }
  }, [formErrors]);

  const handleRegister = async () => {
    const result = await authenticationService.register(
      Email,
      Password,
      Name,
      Surname
    );
    console.log(result);
    if (result.status === 200) {
      sessionStorage.setItem("token", result.data);
      sessionStorage.setItem("email", jwtDecode(result.data).Email);
      sessionStorage.setItem("userId", jwtDecode(result.data).Id);
      sessionStorage.setItem("role", jwtDecode(result.data).Role);
      navigate("/");
    }
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
    if (!Password) {
      errors.Password = "Lozinka je obavezna!";
    } else if (Password !== ConfirmPassword) {
      errors.Password = "Lozinke se ne podudaraju!";
    }

    if (Name.trim().length === 0) {
      errors.Name = "Unesite svoje ime!";
    }
    if (Surname.trim().length === 0) {
      errors.Surname = "Unesite svoje prezime!";
    }

    return errors;
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setFormErrors({});
    setIsRegisterCompleted(true);
    setFormErrors(validate());
  };

  return (
    <div className="login-wrapper">
      <form className="login-form" onSubmit={handleSubmit}>
        <h2>Registracija u sustav</h2>
        <div className="form-group">
          <label>E-mail:</label>
          <input
            type="text"
            autoComplete="off"
            onChange={(d) => setEmail(d.target.value)}
            name="email"
            placeholder="Unesite email"
            className="form-input"
          />
          <span className="register-error">{formErrors.Email}</span>
        </div>
        <div className="form-group">
          <label>Lozinka:</label>
          <input
            type="password"
            autoComplete="off"
            onChange={(d) => setPassword(d.target.value)}
            name="password"
            placeholder="Unesite svoju lozinku"
            className="form-input"
          />
          <span className="register-error">{formErrors.Password}</span>
        </div>
        <div className="form-group">
          <label>Potvrdite lozinku:</label>
          <input
            type="password"
            autoComplete="off"
            onChange={(d) => setConfirmPassword(d.target.value)}
            name="confirmPassword"
            placeholder="Potvrdite lozinku"
            className="form-input"
          />
        </div>
        <div className="form-group">
          <label>Unesite svoje ime:</label>
          <input
            type="text"
            autoComplete="off"
            onChange={(d) => setName(d.target.value)}
            name="name"
            placeholder="Unesite svoje ime"
            className="form-input"
          />
          <span className="register-error">{formErrors.Name}</span>
        </div>
        <div className="form-group">
          <label>Unesite svoje prezime:</label>
          <input
            type="text"
            autoComplete="off"
            onChange={(d) => setSurname(d.target.value)}
            name="name"
            placeholder="Unesite svoje prezime"
            className="form-input"
          />
          <span className="register-error">{formErrors.Surname}</span>
        </div>
        <div className="form-group register-link">
          <Link to="/login">Imate već račun - povratak na prijavu</Link>
        </div>

        {isRegisterCompleted ? (
          <BarLoader />
        ) : (
          <button type="submit" className="login-button">
            Registracija
          </button>
        )}
      </form>
    </div>
  );
}
