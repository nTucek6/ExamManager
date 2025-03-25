import { Outlet, Navigate } from "react-router-dom";

function FindRole() {
  const role = sessionStorage.getItem("role");
  if (role === "Professor") {
    return true;
  } else {
    return false;
  }
}

export default function CheckProfessorRole() {
  let auth = { role: FindRole() };

  return auth.role ? <Outlet /> : <Navigate to="/" />;
}
