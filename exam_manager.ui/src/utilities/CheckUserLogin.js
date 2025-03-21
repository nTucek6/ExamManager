import { Outlet, Navigate } from "react-router-dom";

function FindToken() {
  const token = sessionStorage.getItem("token");
  //const userToken = JSON.parse(token);
  if (token === null) {
    return true;
  } else {
    return false;
  }
}

export default function CheckUserLogin() {
  let auth = { token: FindToken() };

  return !auth.token ? <Outlet /> : <Navigate to="/login" />;
}
