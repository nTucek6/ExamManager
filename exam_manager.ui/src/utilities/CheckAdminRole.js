import { Outlet, Navigate } from "react-router-dom";

function FindRole() {
  const role = sessionStorage.getItem("role");
  if (role === "Admin") {
    return true;
  } else {
    return false;
  }
}

export default function CheckAdminRole() {
  let auth = { role: FindRole() };

  return auth.role ? <Outlet /> : <Navigate to="/" />;
}
