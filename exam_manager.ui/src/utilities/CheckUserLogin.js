import { Outlet, Navigate } from "react-router-dom";

function FindToken() {
  const token = sessionStorage.getItem("token");
  if (token === null) {
    return true;
  } else {
    return false;
  }
}

export default function CheckUserLogin() {
  let auth = { token: FindToken() };

  const role = sessionStorage.getItem("role");
  if (role !== "Admin") {
    return !auth.token ? <Outlet /> : <Navigate to="/login" />;
  } else {
    return (
      !auth.token && (
        <>
          {" "}
          <Outlet /> <Navigate to="/admin" />{" "}
        </>
      )
    );
  }
}
