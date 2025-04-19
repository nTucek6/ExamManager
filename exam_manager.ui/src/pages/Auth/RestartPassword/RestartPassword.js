import { useNavigate, useParams } from "react-router";
import { useEffect, useState } from "react";
import ToastInfo from "../../../components/toastInfo";
import { ToastContainer } from "react-toastify";
import { BarLoader } from "react-spinners";

import authenticationService from "../../../services/authenticationService";

export default function RestartPassword() {
  const { token } = useParams();

  const navigate = useNavigate();

  const [Password, setPassword] = useState();
  const [ConfirmPassword, setConfirmPassword] = useState();

  const [formErrors, setFormErrors] = useState({});

  const [isValidatedCompleted, setIsValidatedCompleted] = useState(false);

  useEffect(() => {
    if (Object.keys(formErrors).length === 0 && isValidatedCompleted) {
      handleRestartPassword();
    } else {
      setIsValidatedCompleted(false);
    }
  }, [formErrors]);

  const validate = () => {
    const errors = {};

    if (!Password) {
      errors.Password = "Lozinka je obavezna!";
    } else if (Password !== ConfirmPassword) {
      errors.ConfirmPassword = "Lozinke se ne podudaraju!";
    }

    return errors;
  };

  const handleRestartPassword = async () =>{
    const response = await authenticationService.restartPassword(
        Password,
        token
      );
    setIsValidatedCompleted(false);
    ToastInfo("Password changed successfully");
    navigate("/login");
  }

  const handleSubmit = (e) => {
    e.preventDefault();
    setFormErrors({});
    setIsValidatedCompleted(true);
    setFormErrors(validate());
  };

  return (
    <div className="custom-wrapper">
      <form onSubmit={handleSubmit} className="custom-form">
        <h2>Reset lozinke</h2>
        <div className="form-group">
          <label>Lozinka:</label>
          <input
            type="password"
            onChange={(d) => setPassword(d.target.value)}
            name="password"
            autoComplete="off"
            placeholder="Unesite lozinku:"
            className="form-input"
          />
          <span className="form-error">{formErrors.Password}</span>
        </div>
        <div className="form-group">
          <label>Lozinka:</label>
          <input
            type="password"
            onChange={(d) => setConfirmPassword(d.target.value)}
            name="confirm-password"
            autoComplete="off"
            placeholder="Ponovite lozinku:"
            className="form-input"
          />
          <span className="form-error">{formErrors.ConfirmPassword}</span>
        </div>

        {isValidatedCompleted ? (
          <BarLoader />
        ) : (
          <button type="submit" className="form-button">
            Pošaljite zahtjev
          </button>
        )}
      </form>
      <ToastContainer />
    </div>
  );
}
