import { useEffect } from "react";
import { Outlet, useNavigate } from "react-router";


export default function Navigation() {

const navigate = useNavigate();

useEffect(()=>{
    if(sessionStorage.getItem('token')== null)
    {
        navigate('/login');
    }
},[])

 const handleLogout = () =>{
    sessionStorage.clear();
    navigate('/login');
 }


  return (
    <div>
      <h1>Welcome</h1>
      <button onClick={handleLogout}>Logout</button>

      <Outlet />
    </div>
  );
}
