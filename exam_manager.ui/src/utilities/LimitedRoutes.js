import { Outlet, Navigate } from "react-router-dom";


function FindToken() {
    const token = sessionStorage.getItem("token");
    if (token === null) {
        return true;
    }
    else {
        return false;
    }
}

export default function LimitedRoutes() {
    let auth = { 'token': FindToken() }

    return (
        auth.token ? <Outlet /> : <Navigate to="/" />
    );

}