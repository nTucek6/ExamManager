import { useEffect, useState } from "react";
import { ToastContainer } from "react-toastify";
import ToastInfo from "../../../components/toastInfo";
import { BarLoader } from "react-spinners";

import authenticationService from "../../../services/authenticationService";
import { Link } from "react-router";

export default function ForgotPassword() {
  const [Email, setEmail] = useState();
  const [formErrors, setFormErrors] = useState({});

  const [isValidatedCompleted, setIsValidatedCompleted] = useState(false);

  useEffect(() => {
    if (Object.keys(formErrors).length === 0 && isValidatedCompleted) {
      handleEmailSend();
    } else {
      setIsValidatedCompleted(false);
    }
  }, [formErrors]);

  const handleSubmit = (e) => {
    e.preventDefault();
    setFormErrors({});
    setIsValidatedCompleted(true);
    setFormErrors(validate());
  };

  const handleEmailSend = async () => {
    const result = await authenticationService.sendPasswordRestartEmail(Email);
    if (result.data === true) {
      ToastInfo("Reset link sent successfully");
      setIsValidatedCompleted(false);
    } else {
      ToastInfo("An error has occurred");
      setIsValidatedCompleted(false);
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

    return errors;
  };

  return (
    <div className="custom-wrapper">
      <form onSubmit={handleSubmit} className="custom-form">
        <h2>Unesite Email za račun od kojega ste zaboravili lozinku</h2>
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
        <div className="form-group form-link">
          <Link to="/login" id="forgot-password">
            Povratak na login?
          </Link>
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
