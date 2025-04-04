import { Link, Outlet, useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";
import "./Navbar.css";
import GetPage from "../utilities/GetPage";

export default function Navbar() {
  const navigate = useNavigate();
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [username, setUsername] = useState("");
  const [userRole, setUserRole] = useState("");

  useEffect(() => {
    if (sessionStorage.getItem("token") !== null) {
      setIsAuthenticated(true);
      const email = sessionStorage.getItem("email");
      if (email) {
        const username = email.split("@")[0];
        setUsername(username);
      }
      setUserRole(sessionStorage.getItem("role"));
    }
  }, []);

  const handleLogout = () => {
    sessionStorage.clear();
    setIsAuthenticated(false);
    navigate("/login");
  };

  const handleChangePassword = () => {
  };

  return (
    <>
      <nav className="navbar">
        <div className="navbar-container">
          <Link to="/" className="logo">
            Examio
          </Link>
          <ul className="nav-links">
            <li>
              <Link to="/">Dashboard</Link>
            </li>
            <li>
              <Link to="/about">About</Link>
            </li>
            {isAuthenticated ? (
              <>
                <li>
                  <span>Hello, {username}</span> {/* Greet the user */}
                </li>
                <li>
                  <button
                    className="change-password-button"
                    onClick={handleChangePassword}
                  >
                    Change Password
                  </button>
                </li>
                <li>
                  <button className="logout-button" onClick={handleLogout}>
                    Logout
                  </button>
                </li>
              </>
            ) : (
              <li>
                <Link to="/login">Login</Link>
              </li>
            )}
          </ul>
        </div>
      </nav>
      {isAuthenticated ? (
        <nav className="sidebar">
          <div className="sidebar-container">
            {userRole === "Professor" ? (
              <ul className="nav-links">
                <li>
                  <Link to="/addexam">Dodaj ispitni rok</Link>
                </li>
              </ul>
            ) : (
              <ul className="nav-links sidebar-links">
                <li>
                  <Link to="/exams">Pregled upisanih ispita</Link>
                </li>
                <li>
                  <Link to="/exams">Prijava ispita</Link>
                </li>
                <li>
                  <Link to="/exams">Odjava ispita</Link>
                </li>
              </ul>
            )}
          </div>
        </nav>
      ) : (
        <></>
      )}
      <div className={isAuthenticated ? "container" : ""}>
        {isAuthenticated ? (
          <div id="page-name-container">
            <GetPage />
          </div>
        ) : (
          <></>
        )}
        <Outlet />
      </div>
    </>
  );
}
